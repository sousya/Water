using System;
using DG.Tweening;
using GameDefine;
using QFramework;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelCreateCtrl;

public class BottleCtrl : MonoBehaviour, IController, ICanSendEvent
{
    public List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
    public List<Transform> spineNode = new List<Transform>();
    public List<Transform> waterNode = new List<Transform>();
    public Transform spineGo, modelGo, leftMovePlace, freezeGo;
    public Animator bottleAnim, fillWaterGoAnim;
    public SkeletonGraphic spine, finishSpine, freezeSpine;

    public Image ImgWaterTop, ImgWaterDown, ImgLimit;
    public SkeletonGraphic nearHide, clearHide, thunder;
    public bool isUp;
    public GameObject finishGo;
    public GameObject waterTopSurface;// 倒水的过程中，水面的最高高度不会超过这个线。
    
    public List<BottleRecord> moveRecords = new List<BottleRecord>();
    
    public Button bottle;
    
    // 存储每种情况的水的旋转角度。index表示瓶子剩余水的个数。
    private Vector3[] _waterRotations = new Vector3[4];
    private BottleRenderUpdate _bottleRenderUpdate;
    public BottleData bottleData;
    
    // 删除本地属性定义，直接使用 BottleData 中的变量
    // public bool isFinish => bottleData.IsFinish;
    // public bool isFreeze => bottleData.IsFreeze;
    // public bool isNearHide => bottleData.IsNearHide;
    // public bool isClearHide => bottleData.IsClearHide;
    // public bool isClearHideAnim => bottleData.IsClearHideAnim;
    // public bool isSelect => bottleData.IsSelect;
    // public int bottleIdx => bottleData.BottleIdx;
    // public int maxNum => bottleData.MaxNum;
    // public int limitColor => bottleData.LimitColor;
    // public int topIdx => bottleData.TopIdx;
    // public int unlockClear => bottleData.UnlockClear;
    
    // public List<int> waters => bottleData.Waters;
    // public List<bool> hideWaters => bottleData.HideWaters;
    // public List<WaterItem> waterItems => bottleData.WaterItems;
    
    // Start is called before the first frame update
    void Start()
    {
        bottle.onClick.AddListener(OnSelected);
        
        // 计算瓶子的旋转角度(根据三角形公式推导)
        var bottleCenterPos = this.gameObject.transform.position;
        var sinEdge = Mathf.Abs(waterTopSurface.transform.position.x - bottleCenterPos.x);
        _bottleRenderUpdate = bottleAnim.GetComponent<BottleRenderUpdate>();
        
        var waterRenderUpdaters = _bottleRenderUpdate.GetComponentsInChildren<WaterRenderUpdater>();
        for (int i = waterRenderUpdaters.Length - 1; i >= 1; i--)
        {
            var position = waterRenderUpdaters[i].waterSurface[0].position;
            
            var cosEdge = Mathf.Abs(waterTopSurface.transform.position.y - position.y);
            _waterRotations[i] = GetBottleRotation(sinEdge, cosEdge);
        }
        // 倒完水使用120度写死角度。
        //_waterRotations[0] = Quaternion.Euler(0, 0, -120); 
        _waterRotations[0] = new Vector3(0, 0, -120);
    }
    
    private void LateUpdate()
    {
        fillWaterGoAnim.transform.localRotation = Quaternion.Inverse(fillWaterGoAnim.transform.parent.rotation);
    }
    
    private void OnSelected()
    {
        if(!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
        {
            GameCtrl.Instance.OnSelect(this);
        }
    }
    
    private Vector3 GetBottleRotation(float sinEdge, float cosEdge)
    {
        float angle = Mathf.Atan(sinEdge / cosEdge);
        angle = Mathf.PI / 2 - angle;
        return new Vector3(0, 0, -angle * Mathf.Rad2Deg);
    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }


    private BottleProperty originProperty;
    
    public void Init(BottleProperty property, int idx)
    {
        originProperty = property;
        InitializeBottleData(property, idx);
        finishGo.SetActive(bottleData.IsFinish);
        nearHide.gameObject.SetActive(bottleData.IsNearHide);
        if(nearHide)
        {
            nearHide.AnimationState.SetAnimation(0, "idle", true);
        }

        foreach (var bottle in waterImg)
        {
            bottle.waterImg.fillAmount = 1;
        }

        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            if(bottleData.IsClearHide || bottleData.IsNearHide || bottleData.WaterItems[i] == WaterItem.Ice)
            {
                if (!LevelManager.Instance.cantClearColorList.Contains(bottleData.Waters[i]))
                {
                    LevelManager.Instance.cantClearColorList.Add(bottleData.Waters[i]);
                }
            }
        }

        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            var color = bottleData.Waters[i];
            if (bottleData.IsClearHide || bottleData.IsNearHide || bottleData.WaterItems[i] == WaterItem.Ice)
            {
                LevelManager.Instance.cantChangeColorList.Add(color);
            }
        }
        
        if (bottleData.TopIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
        SetBottleColor(true, true);
        int spinePosIdx = bottleData.TopIdx + 1;

        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();

        foreach (var item in bottleData.WaterItems)
        {
            if (item == WaterItem.Ice)
            {
                LevelManager.Instance.iceBottles.Add(this);
            }
        }
        CheckFinish();

        freezeGo.gameObject.SetActive(bottleData.IsFreeze);
        if(bottleData.LimitColor != 0)
        {
            ImgLimit.color = LevelManager.Instance.waterColor[bottleData.LimitColor - 1];
        }
        else
        {
            ImgLimit.color = new Color(1,1,1,0);
        }


        if(bottleData.IsFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "idle", false);
        }

        Debug.Log("名字 " + name);
    }

    private void InitializeBottleData(BottleProperty property, int idx)
    {
        bottleData = new BottleData();
        bottleData.Initialize(property, idx);
    }

    public void MoveBottle(BottleCtrl bottleCtrl)
    {
        bottleData.IsFinish = bottleCtrl.bottleData.IsFinish;
        bottleData.IsFreeze = bottleCtrl.bottleData.IsFreeze;
        bottleData.Waters = new List<int>(bottleCtrl.bottleData.Waters);
        bottleData.HideWaters = new List<bool>(bottleCtrl.bottleData.HideWaters);
        bottleData.WaterItems = new List<WaterItem>(bottleCtrl.bottleData.WaterItems);
        bottleData.IsClearHide = bottleCtrl.bottleData.IsClearHide;
        bottleData.IsNearHide = bottleCtrl.bottleData.IsNearHide;
        bottleData.UnlockClear = bottleCtrl.bottleData.UnlockClear;
        bottleData.BottleIdx = bottleCtrl.bottleData.BottleIdx;
        
        nearHide.gameObject.SetActive(bottleData.IsNearHide);
        SetClearHide();
        SetBottleColor(true);
        int spinePosIdx = bottleData.TopIdx + 1;
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();
        if (bottleData.TopIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
        foreach (var item in bottleData.WaterItems)
        {
            if (item == WaterItem.Ice)
            {
                LevelManager.Instance.iceBottles.Add(this);
                LevelManager.Instance.iceBottles.Remove(bottleCtrl);
            }
        }
    }

    /// <summary>
    /// 改变颜色
    /// </summary>
    /// <param name="from">被替换</param>
    /// <param name="to">替换</param>
    public void ChangeColor(int from, int to, Transform target)
    {
        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            if (bottleData.Waters[i] == from)
            {
                StartCoroutine(waterImg[i].ChangeShine());
                StartCoroutine(waterImg[i].ShowThunder(target));
            }
        }
        StartCoroutine(CheckChange(from, to, target)); 
    }

    IEnumerator CheckChange(int from, int to, Transform target)
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            if (bottleData.Waters[i] == from)
            {
                bottleData.Waters[i] = to;
            }
        }
        SetBottleColor();
        PlaySpineWaitAnim();
        CheckFinish();

        if (bottleData.IsFinish)
        {
            LevelManager.Instance.CheckFinishChange(to);
        }
    }

    public void RemoveHide()
    {
        for (int i = 0; i < bottleData.HideWaters.Count; i++)
        {
            bottleData.HideWaters[i] = false;
        }

        SetBottleColor();
        CheckFinish();
    }

    /// <summary>
    /// 清除所有特殊情况
    /// </summary>
    /// <returns></returns>
    public void SetNormal()
    {
        for(int i = 0; i < bottleData.HideWaters.Count; i++) 
        {
            bottleData.HideWaters[i] = false;
        }

        for (int i = 0; i < bottleData.WaterItems.Count; i++)
        {
            bottleData.WaterItems[i] = WaterItem.None;
        }

        if(bottleData.IsFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "attack", false);
        }

        bottleData.IsFreeze = false;
        bottleData.IsNearHide = false;
        bottleData.IsClearHide = false;
        StartCoroutine(CoroutinePlayClearHide(true));
        SetBottleColor(false, true);
        CheckFinish();
    }

    /// <summary>
    /// 增加颜色
    /// </summary>
    /// <returns></returns>
    public void AddColor(int color, Vector3 fromPos)
    {
        if (bottleData.Waters.Count < bottleData.MaxNum)
        {
            bottleData.Waters.Add(color);
            var fx = GameObject.Instantiate(LevelManager.Instance.createFx[color - 1], fromPos, Quaternion.identity);
            fx.transform.SetParent(LevelManager.Instance.gameCanvas);
            var useIdx = bottleData.TopIdx;

            var tween = fx.transform.DOMove(waterNode[useIdx].transform.position, 1f);
            tween.OnComplete(() =>
            {
                Destroy(fx);
            })
            .OnUpdate(() =>
            {
                tween.SetTarget(waterNode[useIdx].transform.position);
            });
                 
            bottleData.WaterItems.Add(WaterItem.None);
        }
    }

    public IEnumerator FinishHide()
    {
        LevelManager.Instance.isPlayAnim = true;
        yield return new WaitForSeconds(1);
        Debug.Log("fx " + bottleData.Waters.Count + " " + name);
        CheckItem();
        SetBottleColor();
        CheckFinish();
        foreach (var item in waterImg)
        {
            item.waterImg.fillAmount = 1;
        }

        LevelManager.Instance.isPlayAnim = false;
    }

    /// <summary>
    /// 判断是否能选中 如果能 则选中
    /// </summary>
    /// <returns></returns>
    public bool OnSelect(bool needUp)
    {
        if ((bottleData.IsFreeze && needUp) || bottleData.IsClearHide || bottleData.IsNearHide || bottleData.IsFinish || LevelManager.Instance.isPlayAnim)
        {
            return false;
        }
        if (needUp)
        {
            modelGo.transform.localPosition += Vector3.up * 100;
        }
        return true;
    }

    public void OnCancelSelect()
    {
        modelGo.transform.localPosition = Vector3.zero;
        bottleData.IsSelect = false;
    }

    public bool CheckMoveOut()
    {
        if (bottleData.TopIdx < 0 || bottleData.WaterItems[bottleData.TopIdx] == WaterItem.Ice)
        {
            return false;
        }
        return true;
    }

    public int GetMoveOutTop()
    {
        if (bottleData.TopIdx < 0)
        {
            return 0;
        }
        return bottleData.Waters[bottleData.TopIdx];
    }

    public WaterItem GetMoveOutItemTop()
    {
        if (bottleData.TopIdx < 0)
        {
            return WaterItem.None;
        }
        return bottleData.WaterItems[bottleData.TopIdx];
    }

    public bool CheckMoveIn(int color)
    {
        var top = GetMoveOutTop();
        if(bottleData.IsClearHide || bottleData.IsNearHide || bottleData.IsFinish || GetLeftEmpty() == 0 || (bottleData.LimitColor != 0 && bottleData.LimitColor != color))
        {
            return false;
        }

        if (color < 1000)
        {
            if (color != top && top != 0)
            {
                return false;
            }
        }
        else
        {
            if (top > 1000)
            {
                return top == color;
            }
            else
            {
                return true;
            }
        }

        return true;
    }

    public void CheckNearHide(int idx)
    {
        if (Mathf.Abs(bottleData.BottleIdx - idx) == 1 && LevelManager.Instance.nowBottles[idx].isUp == isUp)
        {
            foreach (var item in bottleData.Waters)
            {
                LevelManager.Instance.cantChangeColorList.Remove(item);
            }

            bottleData.IsNearHide = false;
            SetClearHide();
            CheckFinish();
            StartCoroutine(CoroutinePlayClearHide());
        }
    }

    IEnumerator CoroutinePlayClearHide(bool nowait = false)
    {
        if(!nowait)
        {
            yield return new WaitForSeconds(2f);
        }
        nearHide.AnimationState.SetAnimation(0, "jingji_xiaoshi", false);
        yield return new WaitForSeconds(1.7f);
        nearHide.gameObject.SetActive(false);
    }

    public void CheckUnlockHide(int color)
    {
        if (bottleData.IsClearHide)
        {
            if (bottleData.UnlockClear == color)
            {
                foreach (var item in bottleData.Waters)
                {
                    LevelManager.Instance.cantChangeColorList.Remove(item);
                }
                LevelManager.Instance.cantChangeColorList.Remove(color);
                StartCoroutine(HideClearHide());
            }
        }
    }

    IEnumerator HideClearHide()
    {
        bottleData.IsClearHideAnim = true;
        yield return new WaitForSeconds(1.5f);
        var animName = DATA.GetDescription<EDisappearAnim>((EDisappearAnim)bottleData.UnlockClear);
        clearHide.AnimationState.SetAnimation(0, animName, false);
        
        bottleData.IsClearHide = false;
        CheckFinish();

        yield return new WaitForSeconds(3.7f);
        clearHide.gameObject.SetActive(false);
        bottleData.IsClearHideAnim = false;
    }

    void SetClearHide()
    {
        if(!bottleData.IsClearHideAnim)
        {
            clearHide.gameObject.SetActive(bottleData.IsClearHide);
            if (bottleData.IsClearHide)
            {
                var animName = DATA.GetDescription<EClearHideAnim>((EClearHideAnim)bottleData.UnlockClear);
                clearHide.AnimationState.SetAnimation(0, animName, false);
            }
        }
    }

    public int GetLeftEmpty()
    {
        return bottleData.MaxNum - 1 - bottleData.TopIdx;
    }

    public void MoveTo(BottleCtrl other)
    {
        int moveNum = other.GetLeftEmpty();
        int sameNum = 1;

        for (int i = bottleData.TopIdx - 1; i >= 0; i--)
        {
            if (bottleData.Waters[i] == GetMoveOutTop() && bottleData.WaterItems[i] != WaterItem.Ice)
            {
                sameNum++;
            }
            else
            {
                break;
            }
        }

        if (moveNum > sameNum)
        {
            moveNum = sameNum;
        }

        var color = GetMoveOutTop();
        MoveToOtherAnim(other, bottleData.Waters.Count - moveNum, color);
        PlayOutAnim(moveNum, bottleData.TopIdx, color);

        for (int i = 0; i < moveNum; i++)
        {
            other.ReceiveWater(color, GetMoveOutItemTop());
            int idx = bottleData.TopIdx;
            if (bottleData.Waters.Count > 0)
            {
                waterImg[idx].wenhaoFxGo.SetActive(false);
                waterImg[idx].HideGo.SetActive(false);
                bottleData.Waters.RemoveAt(idx);
                bottleData.WaterItems.RemoveAt(idx);
                bottleData.HideWaters.RemoveAt(idx);
            }
            GameCtrl.Instance.control = false;
        }

        OnCancelSelect();
        other.PlayFillAnim(moveNum, color);
    }

    public void ReceiveWater(int water, WaterItem item)
    {
        if (water > 0)
        {
            bottleData.Waters.Add(water);
            bottleData.WaterItems.Add(item);
            bottleData.HideWaters.Add(false);
        }
        CheckFinish();
    }

    public void CheckFinish(bool isChange = false)
    {
        if (bottleData.TopIdx > 0 && !bottleData.IsNearHide && !bottleData.IsClearHide && !bottleData.IsFinish)
        {
            var topColor = bottleData.Waters[bottleData.TopIdx];
            if (bottleData.TopIdx == bottleData.MaxNum - 1)
            {
                for(int i = bottleData.MaxNum - 1; i >= 0; i--)
                {
                    var water = bottleData.Waters[i];
                    if (water != topColor || bottleData.WaterItems[i] == WaterItem.Ice)
                    {
                        return;
                    }
                }

                OnFinish();
            }
        }
    }

    public void OnFinish()
    {
        bottleData.IsFinish = true;

        LevelManager.Instance.FinishClear(GetMoveOutTop(), bottleData.BottleIdx);

        for (int i = 0; i < bottleData.WaterItems.Count; i++)
        {
            if (bottleData.WaterItems[i] == WaterItem.BreakIce)
            {
                LevelManager.Instance.BreakIce();
                bottleData.WaterItems[i] = WaterItem.None;
            }

            if (bottleData.WaterItems[i] == WaterItem.Bomb)
            {
                LevelManager.Instance.CancelBomb();
                bottleData.WaterItems[i] = WaterItem.None;
            }
        }

        CheckWaterItem();
        StartCoroutine(ShowFinish());
    }

    IEnumerator ShowFinish()
    {
        var trackEntry = finishSpine.AnimationState.GetCurrent(0); // 获取轨道0上的当前动画条目
        if (trackEntry != null)
        {
            //trackEntry.TimeScale = 1f;
            finishSpine.Initialize(true);
        }
        else
        {
            // 如果当前没有动画，直接设置动画
            finishSpine.AnimationState.SetAnimation(0, "animation", false);
        }
        yield return new WaitForSeconds(0.8f);

        if (bottleData.IsFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "attack", false);
        }

        yield return new WaitForSeconds(0.2f);

    
        finishGo.SetActive(bottleData.IsFinish);
    }

    public void UnlockIceWater()
    {
        for (int i = 0; i < bottleData.WaterItems.Count; i++)
        {
            if (bottleData.WaterItems[i] == WaterItem.Ice)
            {
                bottleData.WaterItems[i] = WaterItem.None;
                CheckFinish();
                break;
            }
        }
        CheckWaterItem();
    }

    public void CheckHide(bool isFirst = false)
    {
        if (bottleData.HideWaters.Count > bottleData.Waters.Count)
        {
            while (bottleData.HideWaters.Count > bottleData.Waters.Count)
            {
                bottleData.HideWaters.RemoveAt(bottleData.HideWaters.Count - 1);
            }
        }
        else if (bottleData.HideWaters.Count < bottleData.Waters.Count)
        {
            while (bottleData.HideWaters.Count < bottleData.Waters.Count)
            {
                bottleData.HideWaters.Add(false);
            }
        }

        if(!isFirst)
        {
            if (bottleData.HideWaters.Count > 0 && bottleData.Waters.Count > 0)
            {
                bottleData.HideWaters[bottleData.Waters.Count - 1] = false;

                for (int i = bottleData.Waters.Count - 1; i >= 0; i--)
                {
                    if ((bottleData.TopIdx - 1 >= 0) && (bottleData.Waters[i] == bottleData.Waters[bottleData.TopIdx - 1]))
                    {
                        bottleData.HideWaters[i] = false;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void SetBottleColor(bool isFirst = false, bool nowaitHide = false)
    {
        CheckHide(isFirst);

        if (bottleData.IsFinish)
        {
            for (int i = 0; i < bottleData.HideWaters.Count; i++)
            {
                bottleData.HideWaters[i] = false;
            }
        }

        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            var useColor = bottleData.Waters[i] - 1;
            if (useColor < 1000)
            {
                Debug.Log("UseColor " + useColor);
                waterImg[i].SetColorState(ItemType.UseColor, LevelManager.Instance.waterColor[useColor]);
            }
            else
            {
                waterImg[i].SetColorState((ItemType)bottleData.Waters[i], LevelManager.Instance.ItemColor);
            }

            if (bottleData.HideWaters.Count > 0)
            {
                waterImg[i].SetHide(bottleData.HideWaters[i], nowaitHide);
            }
            waterImg[i].waterColor = useColor;
        }

        for (int i = 0; i < waterImg.Count; i++)
        {
            Debug.Log(name + "显示水 " + (i < bottleData.Waters.Count || waterImg[i].isPlayItemAnim) + " i " + i + "  waters.Count " + bottleData.Waters.Count + " isPlayItemAnim " + waterImg[i].isPlayItemAnim);
            waterImg[i].gameObject.SetActive(i < bottleData.Waters.Count || waterImg[i].isPlayItemAnim);
        }

        CheckWaterItem();
        SetClearHide();

        int spinePosIdx = bottleData.TopIdx + 1;
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();
    }

    void CheckWaterItem()
    {
        for (int i = 0; i < bottleData.WaterItems.Count; i++)
        {
            switch (bottleData.WaterItems[i])
            {
                case WaterItem.None:
                    waterImg[i].textItem.text = "";
                    break;
                case WaterItem.Ice:
                    waterImg[i].textItem.text = "ICE";
                    break;
                case WaterItem.Bomb:
                    waterImg[i].textItem.text = "BOMB";
                    break;
                case WaterItem.BreakIce:
                    waterImg[i].textItem.text = "BREAKICE";
                    break;
            }
        }
    }

    /// <summary>
    /// 接水动画
    /// </summary>
    /// <param name="num"></param>
    public void PlayFillAnim(int num, int color)
    {
        StartCoroutine(CoroutinePlayFillAnim(num, color));
    }

    IEnumerator CoroutinePlayFillAnim(int num, int color)
    {

        LevelManager.Instance.isPlayAnim = true;
        float fillAlltime = 0.46f;
        yield return new WaitForSeconds(fillAlltime);
        SetBottleColor();
        //float fillAlltime = 1.33f;
        int startIdx = bottleData.TopIdx + 1 - num;
        if (color < 1000)
        {
            spineGo.gameObject.SetActive(true);
            SetNowSpinePos(startIdx);
            spineGo.DOMove(spineNode[bottleData.TopIdx + 1].position, fillAlltime).SetEase(Ease.Linear);
        }
        else
        {
            if(startIdx >= 0)
            {
                SetNowSpinePos(startIdx);
            }
            //spineGo.transform.position = spineNode[bottleData.TopIdx + 1].position;
        }
        PlaySpineAnim();

        float fillTime = fillAlltime / num;
        if(color > 1000)
        {
            fillTime = 0.1f;
        }
        for (int i = 0; i < num; i++)
        {
            waterImg[startIdx + i].waterImg.fillAmount = 0;
        }

        for (int i = 0; i < num; i++)
        {
            waterImg[startIdx + i].PlayFillAnim(fillTime);
            yield return new WaitForSeconds(fillTime);
        }

        LevelManager.Instance.isPlayAnim = false;

        CheckItem();

        CheckFill();

        //if (isFinish)
        //{
        //    //LevelManager.Instance.CheckVictory(bottleIdx);
        //    //yield return new WaitForSeconds(1);
        //    //LevelManager.Instance.VictoryEnter(bottleIdx);
        //}
    }

    void CheckFill()
    {
        for (int i = 0; i < waterImg.Count; i++)
        {
            if (waterImg.Count > i)
            {
                waterImg[i].waterImg.fillAmount = 1;
            }
            else
            {
                waterImg[i].waterImg.fillAmount = 0;
            }
        }
    }

    public void PlaySpineAnim()
    {
        var color = GetMoveOutTop();
        
        var spineAnimName = color is > 0 and < (int) ESpineWaterOutAnimName.ANIM_MAX ? DATA.GetDescription<ESpineWaterOutAnimName>((ESpineWaterOutAnimName)color) : "";

        if(color < 1000 && color != 0)
        {
            spine.AnimationState.SetAnimation(0, spineAnimName, false);
        }
    }
    public void PlaySpineWaitAnim(int useColor = -1)
    {
        //var color = GetMoveOutTop();
        int spinePosIdx = bottleData.TopIdx;

        if (bottleData.TopIdx >= 0)
        {
            for(int i = spinePosIdx; i >= 0; i--)
            {
                if (bottleData.Waters[spinePosIdx] < 1000)
                {
                    spinePosIdx = i;
                    break;
                }
            }
            //while (spinePosIdx > 0 && waters[spinePosIdx] > 1000)
            //{
            //    spinePosIdx--;
            //}
            var color = bottleData.Waters[spinePosIdx];
            if (useColor != -1)
            {
                color = useColor;
            }

            var spineAnimName = color is >= 0 and < (int)ESpinWaitAnimName.ANIM_MAX ? DATA.GetDescription<ESpinWaitAnimName>((ESpinWaitAnimName)color) : "";
            if (color > 0)
            {
                if (color < 1000)
                {
                    spineGo.gameObject.SetActive(true);
                    spine.AnimationState.SetAnimation(0, spineAnimName, false);

                }
                else
                {
                    spineGo.gameObject.SetActive(false);
                }
            }
            else
            {
                spineGo.gameObject.SetActive(false);
            }
        }
    }

    public void SetNowSpinePos(int node)
    {
        var useNode = node;
        Debug.Log("当前节点 " + node);
        if(useNode - 1 < bottleData.Waters.Count)
        {
            for (int i = node - 1; i >= 0; i--)
            {
                if (bottleData.Waters[i] < 1000)
                {
                    useNode = i + 1;
                    break;
                }
            }
        }
        
        spineGo.localPosition = spineNode[useNode].localPosition;
    }


    /// <summary>
    /// 倒水动画
    /// </summary>
    /// <param name="num"></param>
    public void PlayOutAnim(int num, int useIdx, int useColor)
    {
        StartCoroutine(CoroutinePlayOutAnim(num, useIdx, useColor));
    }

    IEnumerator CoroutinePlayOutAnim(int num, int useIdx, int useColor)
    {
        //yield return new WaitForSeconds(1f);
        float fillAlltime = 0.46f;
        yield return new WaitForSeconds(fillAlltime);
        //float fillAlltime = 1.33f;
        spineGo.gameObject.SetActive(true);
        int startIdx = useIdx;
        SetNowSpinePos(startIdx + 1);
        Debug.Log("移动终点  " + useIdx + " " + num);
        if(useColor > 1000)
        {
            spineGo.transform.localPosition = spineNode[useIdx + 1 - num].localPosition;
            if (bottleData.TopIdx < 0)
            {
                spineGo.gameObject.SetActive(false);
            }
        }
        else
        {
            spineGo.DOLocalMove(spineNode[useIdx + 1 - num].localPosition, fillAlltime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (bottleData.TopIdx < 0)
                {
                    spineGo.gameObject.SetActive(false);
                }
            });
        }
   
        PlaySpineWaitAnim(useColor);

        float fillTime = fillAlltime / num;
        if(useColor > 1000)
        {
            fillTime = 0f;
        }
        for (int i = 0; i < num; i++)
        {
            waterImg[startIdx - i].waterImg.fillAmount = 1;
        }

        for (int i = 0; i < num; i++)
        {
            waterImg[startIdx - i].PlayOutAnim(fillTime);
            yield return new WaitForSeconds(fillTime);
        }

        SetBottleColor();
        PlaySpineWaitAnim();

    }

    public void MoveToOtherAnim(BottleCtrl other, int leftWater, int useColor = -1)
    {
        LevelManager.Instance.isPlayAnim = true;

        var bottleRenderUpdate = bottleAnim.GetComponent<BottleRenderUpdate>();
        bottleRenderUpdate.SetMoveBottleRenderState(true);
        
        
        modelGo.transform.DOMove(other.leftMovePlace.position, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetDownWaterSp(useColor);
            if (useColor < 1000) ////非道具动画播放
            {
                PlayWaterDown();

                //modelGo.transform.DORotate(_waterRotations[leftWater], 0.62f).SetEase(Ease.Linear).OnComplete(() =>
                modelGo.transform.DORotate(_waterRotations[leftWater], 10.0f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    modelGo.transform.DOLocalMove(Vector3.zero, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        LevelManager.Instance.isPlayAnim = false;
                        bottleRenderUpdate.SetMoveBottleRenderState(false);
                    });
                    modelGo.transform.DORotate(Vector3.zero, 0.14f).SetEase(Ease.Linear).OnComplete(null);
                });
            }
            else
            {
                modelGo.transform.DORotate(new Vector3(0, 0, -45), 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    modelGo.transform.DOLocalMove(Vector3.zero, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        LevelManager.Instance.isPlayAnim = false;
                        bottleRenderUpdate.SetMoveBottleRenderState(false);
                    });
                    modelGo.transform.DORotate(Vector3.zero, 0.14f).SetEase(Ease.Linear).OnComplete(null);
                });
            }
            
        });
    }

    public void PlayWaterDown()
    {
        fillWaterGoAnim.Play("FillWater");
    }

    public void SetDownWaterSp(int useColor = -1)
    {
        var color = GetMoveOutTop();
        if (useColor != -1)
        {
            color = useColor;
        }

        if(color < 1000)
        {
            ImgWaterTop.sprite = LevelManager.Instance.waterTopSp[color - 1];
            ImgWaterDown.sprite = LevelManager.Instance.waterSp[color - 1];
        }
    }

    public void CheckItem()
    {
        int itemId = 0;
        int itemPlace = 0;
        List<int> items = new List<int>();

        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            var waterColor = bottleData.Waters[i];

            if(waterColor > 1000)
            {
                if(itemId == 0)
                {
                    itemPlace = i;
                    itemId = waterColor;
                }
                else
                {
                    if(bottleData.Waters[i] == itemId)
                    {
                        switch (bottleData.Waters[i])
                        {
                            case (int)ItemType.ClearItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(bottleData.Waters[i]);
                                    waterImg[i - 1].PlayUseBroom(waterImg[i]);
                                    bottleData.Waters[i] = 0;
                                    bottleData.Waters[itemPlace] = 0;
                                }
                                break;
                            case (int)ItemType.MakeColorItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(bottleData.Waters[i]);
                                    waterImg[i - 1].PlayUseCreate(this, waterImg[i]);
                                    bottleData.Waters[i] = 0;
                                    bottleData.Waters[itemPlace] = 0;
                                    waterImg[i].color = new Color(1,1,1,0);
                                    waterImg[i].broomItemGo.SetActive(false);
                                    waterImg[i].createItemGo.SetActive(false);
                                    waterImg[i].changeItemGo.SetActive(false);
                                    waterImg[i].magnetItemGo.SetActive(false);
                                }
                                break;
                            case (int)ItemType.MagnetItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(bottleData.Waters[i]);
                                    waterImg[i - 1].PlayUseMagnet(waterImg[i]);
                                    bottleData.Waters[i] = 0;
                                    bottleData.Waters[itemPlace] = 0;
                                }
                                break;
                            default:
                                if(bottleData.Waters[i] > 2000 && bottleData.Waters[i] < 3000)
                                {
                                    if (itemId == waterColor && i - itemPlace == 1)
                                    {
                                        items.Add(bottleData.Waters[i]);
                                        waterImg[i - 1].PlayUseChange(waterImg[i]);
                                        bottleData.Waters[i] = 0;
                                        bottleData.Waters[itemPlace] = 0;
                                    }
                                }
                                else if (bottleData.Waters[i] > 3000)
                                {
                                    if (itemId == waterColor && i - itemPlace == 1)
                                    {
                                        items.Add(bottleData.Waters[i]);
                                        waterImg[i - 1].PlayUseBroom(waterImg[i]);
                                        bottleData.Waters[i] = 0;
                                        bottleData.Waters[itemPlace] = 0;
                                    }
                                }
                                break;
                        }

                        itemId = 0;
                    }
                    else
                    {
                        itemPlace = i;
                        itemId = waterColor;
                    }
                }
            }
            else
            {
                itemId = 0;
            }
        }

        List<int> temp = new List<int>();
        List<WaterItem> tempItem = new List<WaterItem>();

        for(int i = 0; i < bottleData.Waters.Count; i++)
        {
            if (bottleData.Waters[i] != 0)
            {
                temp.Add(bottleData.Waters[i]);
                tempItem.Add(bottleData.WaterItems[i]);
            }
        }
        bottleData.Waters = temp;
        bottleData.WaterItems = tempItem;

        for (int i = 0; i < items.Count; i++)
        {
            int useItem = items[i];
            LevelManager.Instance.UseItem(useItem, waterImg[itemPlace].transform);
        }

        spineGo.gameObject.SetActive(bottleData.TopIdx >= 0);
        PlaySpineWaitAnim();
        SetNowSpinePos(bottleData.TopIdx + 1);
    }

    public void PlayBroomBullet(int color, Vector3 fromPos)
    {
        List<BottleWaterCtrl> list = new List<BottleWaterCtrl>();
        for (int i = 0; i < bottleData.Waters.Count; i++)
        {
            if (bottleData.Waters[i] == color)
            {
                list.Add(waterImg[i]);
            }
        }

        foreach(var ctrl in list)
        {
            var go = GameObject.Instantiate(LevelManager.Instance.broomBullet);

            var fly = go.GetComponent<FlyCtrl>();
            fly.target = ctrl.transform;
            fly.flyTime = 1.2f;
            go.transform.position = fromPos;

            fly.BeginFly();
        }
    }

    public void RemoveAllOneColor(int color)
    {
        List<int> list = new List<int>();
        List<WaterItem> items = new List<WaterItem>();
        List<bool> hides = new List<bool>();
        for(int i = 0; i < bottleData.Waters.Count; i++)
        {
            if (bottleData.Waters[i] == color)
            {
                StartCoroutine(PlayShine(i));
            }
            else
            {
                list.Add(bottleData.Waters[i]);
                items.Add(bottleData.WaterItems[i]);
                hides.Add(bottleData.HideWaters[i]);
            }
        }

        bottleData.WaterItems = items;
        bottleData.Waters = list;
        bottleData.HideWaters = hides;
    }

    IEnumerator PlayShine(int i)
    {
        var imgcmp = waterImg[i].transform.GetComponent<Image>();
        imgcmp.material = LevelManager.Instance.shineMaterial;
        StartCoroutine( waterImg[i].ShowBroomAfter());

        yield return new WaitForSeconds(2.2f);
        imgcmp.material = null;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        SetBottleColor();
        CheckItem();
        CheckFinish();

        if (bottleData.TopIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
    }

    public void RecordLast()
    {
        var record = new BottleRecord();
        record.isFinish = bottleData.IsFinish;
        record.isNearHide = bottleData.IsNearHide;
        record.isClearHide = bottleData.IsClearHide;
        record.isFreeze = bottleData.IsFreeze;
        record.waters = new List<int>(bottleData.Waters);
        record.hideWaters = new List<bool>(bottleData.HideWaters);
        record.waterItems = new List<WaterItem>(bottleData.WaterItems);

        moveRecords.Add(record);
    }

    public void ReturnLast()
    {
        if(moveRecords.Count < 0)
        {
            return;
        }
        var record = moveRecords[moveRecords.Count - 1];

        var temp = new BottleProperty();

        temp.isFreeze = record.isFreeze;
        temp.isNearHide = record.isNearHide;
        temp.isClearHide = record.isClearHide;
        temp.waterSet = new List<int>(record.waters);
        temp.isHide = new List<bool>(record.hideWaters);
        temp.waterItem = new List<WaterItem>(record.waterItems);

        temp.numCake = originProperty.numCake;
        temp.limitColor = originProperty.limitColor;
        temp.lockType = originProperty.lockType;

        Init(temp, bottleData.BottleIdx);
        bottleData.IsFinish = record.isFinish;

        moveRecords.Remove(record);
        finishGo.SetActive(bottleData.IsFinish);
    }
}
