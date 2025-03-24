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
using System.Linq;

public class BottleCtrl : MonoBehaviour, IController, ICanSendEvent
{
    private class BottleData
    {
        public bool IsFinish { get; set; }
        public bool IsFreeze { get; set; }
        public bool IsClearHide { get; set; }
        public bool IsNearHide { get; set; }
        public bool IsPlayAnim { get; set; }
        public bool IsSelect { get; set; }
        public bool IsClearHideAnim { get; set; }
        
        public List<int> Waters { get; set; } = new List<int>();
        public List<bool> HideWaters { get; set; } = new List<bool>();
        public List<WaterItem> WaterItems { get; set; } = new List<WaterItem>();
        public int MaxNum { get; set; } = 4;
        public int LimitColor { get; set; }
        public int UnlockClear { get; set; }
        public int BottleIdx { get; set; }
        
        public int TopIdx => Waters.Count - 1;
    }

    private readonly BottleData _data = new BottleData();

    [Header("UI Components")]
    public Transform spineGo;
    public Transform modelGo;
    public Transform leftMovePlace;
    public Transform freezeGo;
    public Animator bottleAnim;
    public Animator fillWaterGoAnim;
    public SkeletonGraphic spine;
    public SkeletonGraphic finishSpine;
    public SkeletonGraphic freezeSpine;
    public Image ImgWaterTop;
    public Image ImgWaterDown;
    public Image ImgLimit;
    public GameObject finishGo;
    public GameObject waterTopSurface;

    [Header("Water Controls")]
    public List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
    public List<Transform> spineNode = new List<Transform>();
    public List<Transform> waterNode = new List<Transform>();

    public bool isFinish, isFreeze, isClearHide, isNearHide, isPlayAnim, isSelect, isClearHideAnim;
    public List<int> waters = new List<int>();
    public List<bool> hideWaters = new List<bool>();
    public List<WaterItem> waterItems = new List<WaterItem>();
    public List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
    public List<Transform> spineNode = new List<Transform>();
    public List<Transform> waterNode = new List<Transform>();
    public Transform spineGo, modelGo, leftMovePlace, freezeGo;
    public Animator bottleAnim, fillWaterGoAnim;
    public SkeletonGraphic spine, finishSpine, freezeSpine;
    public int maxNum = 4, limitColor = 0;
    public Image ImgWaterTop, ImgWaterDown, ImgLimit;
    public SkeletonGraphic nearHide, clearHide, thunder;
    public bool isUp;
    public GameObject finishGo;
    public GameObject waterTopSurface;// 倒水的过程中，水面的最高高度不会超过这个线。
    
    public List<BottleRecord> moveRecords = new List<BottleRecord>();
    
    public int bottleIdx;
    public int unlockClear = 0;
    public Button bottle;
    
    // 存储每种情况的水的旋转角度。index表示瓶子剩余水的个数。
    private Vector3[] _waterRotations = new Vector3[4];
    private BottleRenderUpdate _bottleRenderUpdate;
    
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

    private int topIdx
    {
        get
        {
            return waters.Count - 1;
        }
    }

    BottleProperty originProperty;
    
    public void Init(BottleProperty property, int idx)
    {
        InitializeBottleData(property, idx);
        InitializeVisuals();
        InitializeWaterControls();
        UpdateBottleState();
    }

    private void InitializeBottleData(BottleProperty property, int idx)
    {
        _data.IsFinish = false;
        _data.IsFreeze = property.isFreeze;
        _data.IsClearHide = property.isClearHide;
        _data.IsNearHide = property.isNearHide;
        _data.Waters = new List<int>(property.waterSet);
        _data.HideWaters = new List<bool>(property.isHide);
        _data.WaterItems = new List<WaterItem>(property.waterItem);
        _data.UnlockClear = property.lockType;
        _data.LimitColor = property.limitColor;
        _data.BottleIdx = idx;
    }

    private void InitializeVisuals()
    {
        finishGo.SetActive(_data.IsFinish);
        nearHide.gameObject.SetActive(_data.IsNearHide);
        freezeGo.gameObject.SetActive(_data.IsFreeze);
        
        if (_data.IsNearHide)
        {
            nearHide.AnimationState.SetAnimation(0, "idle", true);
        }
        
        if (_data.IsFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "idle", false);
        }
        
        UpdateLimitColorVisual();
    }

    private void UpdateLimitColorVisual()
    {
        ImgLimit.color = _data.LimitColor != 0 
            ? LevelManager.Instance.waterColor[_data.LimitColor - 1] 
            : new Color(1, 1, 1, 0);
    }

    private void UpdateBottleState()
    {
        for (int i = 0; i < waters.Count; i++)
        {
            if(isClearHide || isNearHide || waterItems[i] == WaterItem.Ice)
            {
                if (!LevelManager.Instance.cantClearColorList.Contains(waters[i]))
                {
                    LevelManager.Instance.cantClearColorList.Add(waters[i]);
                }
            }
        }

        for (int i = 0; i < waters.Count; i++)
        {
            var color = waters[i];
            if (isClearHide || isNearHide || waterItems[i] == WaterItem.Ice)
            {
                LevelManager.Instance.cantChangeColorList.Add(color);
            }
        }
        
        if (topIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
        SetBottleColor(true, true);
        int spinePosIdx = topIdx + 1;
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();

        foreach (var item in waterItems)
        {
            if (item == WaterItem.Ice)
            {
                LevelManager.Instance.iceBottles.Add(this);
            }
        }
        CheckFinish();

        if(isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "idle", false);
        }

        Debug.Log("名字 " + name);
    }

    public void MoveBottle(BottleCtrl bottleCtrl)
    {
        _data.IsFinish = bottleCtrl._data.IsFinish;
        _data.IsFreeze = bottleCtrl._data.IsFreeze;
        _data.Waters = new List<int>(bottleCtrl._data.Waters);
        _data.HideWaters = new List<bool>(bottleCtrl._data.HideWaters);
        _data.WaterItems = new List<WaterItem>(bottleCtrl._data.WaterItems);
        _data.IsClearHide = bottleCtrl._data.IsClearHide;
        _data.IsNearHide = bottleCtrl._data.IsNearHide;
        _data.UnlockClear = bottleCtrl._data.UnlockClear;
        _data.BottleIdx = bottleCtrl._data.BottleIdx;
        nearHide.gameObject.SetActive(_data.IsNearHide);
        SetClearHide();
        SetBottleColor(true);
        int spinePosIdx = topIdx + 1;
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();
        if (topIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
        foreach (var item in _data.WaterItems)
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
        for (int i = 0; i < waters.Count; i++)
        {
            if (waters[i] == from)
            {
                StartCoroutine(waterImg[i].ChangeShine());
                StartCoroutine(waterImg[i].ShowThunder(target));
            }
        }
        StartCoroutine(CheckChange(from,  to, target)); 

    }

    IEnumerator CheckChange(int from, int to, Transform target)
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < waters.Count; i++)
        {
            if (waters[i] == from)
            {
                waters[i] = to;
            }
        }
        SetBottleColor();
        PlaySpineWaitAnim();
        CheckFinish();

        if (isFinish)
        {
            LevelManager.Instance.CheckFinishChange(to);
        }
    }

    public void RemoveHide()
    {
        for (int i = 0; i < hideWaters.Count; i++)
        {
            hideWaters[i] = false;
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
        for(int i = 0; i < hideWaters.Count; i++) 
        {
            hideWaters[i] = false;
        }

        for (int i = 0; i < waterItems.Count; i++)
        {
            waterItems[i] = WaterItem.None;
        }

        if(isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "attack", false);
        }

        isFreeze = false;
        isNearHide = false;
        isClearHide = false;
        StartCoroutine(CoroutinePlayClearHide(true));
        SetBottleColor(false, true);
        CheckFinish();
        //CheckWaterItem();
    }

    /// <summary>
    /// 增加颜色
    /// </summary>
    /// <returns></returns>
    public void AddColor(int color, Vector3 fromPos)
    {
        if (waters.Count < maxNum)
        {
            waters.Add(color);
            var fx = GameObject.Instantiate(LevelManager.Instance.createFx[color - 1], fromPos, Quaternion.identity);
            fx.transform.SetParent(LevelManager.Instance.gameCanvas);
            //Debug.Log("fx " + waterImg[topIdx].transform.name);
            var useIdx = topIdx;

            var tween = fx.transform.DOMove(waterNode[useIdx].transform.position, 1f);
                tween.OnComplete(() =>
                {
                    Destroy(fx);
                })
                .OnUpdate(() =>
                {
                    tween.SetTarget(waterNode[useIdx].transform.position);
                });
                 
            waterItems.Add(WaterItem.None);
        }

    }

    public IEnumerator FinishHide()
    {
        LevelManager.Instance.isPlayAnim = true;
        yield return new WaitForSeconds(1);
        Debug.Log("fx " + waters.Count + " " + name);
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
        ////判断是否被冰冻,隐藏或者完成
        if ((isFreeze && needUp) || isClearHide || isNearHide || isFinish || LevelManager.Instance.isPlayAnim)
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
        isSelect = false;
    }

    public bool CheckMoveOut()
    {
        if (topIdx < 0 || waterItems[topIdx] == WaterItem.Ice)
        {
            return false;
        }

        return true;
    }

    public int GetMoveOutTop()
    {
        if (topIdx < 0)
        {
            return 0;
        }
        return waters[topIdx];
    }

    public WaterItem GetMoveOutItemTop()
    {
        if (topIdx < 0)
        {
            return WaterItem.None;
        }
        return waterItems[topIdx];
    }

    public bool CheckMoveIn(int color)
    {
        var top = GetMoveOutTop();
        if(isClearHide || isNearHide || isFinish || GetLeftEmpty() == 0 || (limitColor != 0 && limitColor != color))
        {
            return false;
        }

        if (color < 1000///判断非道具
        {
            if (color != top && top != 0)
            {
                return false;
            }
        }
        else
        {
            if (top > 1000)////判断自身顶部是否为道具 
            {
                return top == color;////相同道具才可放置
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
        if (Mathf.Abs(bottleIdx - idx) == 1 && LevelManager.Instance.nowBottles[idx].isUp == isUp)
        {
            foreach (var item in waters)
            {
                LevelManager.Instance.cantChangeColorList.Remove(item);
            }

            isNearHide = false;
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
        if (isClearHide)
        {
            if (unlockClear == color)
            {
                foreach (var item in waters)
                {
                    LevelManager.Instance.cantChangeColorList.Remove(item);
                }
                LevelManager.Instance.cantChangeColorList.Remove(color);
                StartCoroutine( HideClearHide());
            }
        }

        //SetClearHide();
        //CheckFinish();
    }

    IEnumerator HideClearHide()
    {
        _data.IsClearHideAnim = true;
        yield return new WaitForSeconds(1.5f);
        var animName = DATA.GetDescription<EDisappearAnim>((EDisappearAnim)unlockClear);
        clearHide.AnimationState.SetAnimation(0, animName, false);
        
        _data.IsClearHide = false;
        CheckFinish();

        yield return new WaitForSeconds(3.7f);
        clearHide.gameObject.SetActive(false);
        _data.IsClearHideAnim = false;
    }

    void SetClearHide()
    {
        if(!_data.IsClearHideAnim)
        {
            clearHide.gameObject.SetActive(_data.IsClearHide);
            //ImgClearHide.gameObject.SetActive(isClearHide);
            if (_data.IsClearHide)
            {
                var animName = DATA.GetDescription<EClearHideAnim>((EClearHideAnim)unlockClear);
                clearHide.AnimationState.SetAnimation(0, animName, false);
            }
        }
       
    }

    public int GetLeftEmpty()
    {
        return maxNum - 1 - topIdx;
    }

    public void MoveTo(BottleCtrl other)
    {
        int moveAmount = CalculateMoveAmount(other);
        if (moveAmount <= 0) return;
        
        ExecuteWaterMove(other, moveAmount);
    }

    private int CalculateMoveAmount(BottleCtrl other)
    {
        int availableSpace = other.GetLeftEmpty();
        int sameColorCount = CountSameColorWaters();
        return Mathf.Min(availableSpace, sameColorCount);
    }

    private int CountSameColorWaters()
    {
        int count = 1;
        int topColor = GetMoveOutTop();
        
        for (int i = topIdx - 1; i >= 0; i--)
        {
            if (waters[i] == topColor && waterItems[i] != WaterItem.Ice)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        
        return count;
    }

    private void ExecuteWaterMove(BottleCtrl other, int moveAmount)
    {
        int color = GetMoveOutTop();
        MoveToOtherAnim(other, waters.Count - moveAmount, color);
        PlayOutAnim(moveAmount, topIdx, color);
        
        for (int i = 0; i < moveAmount; i++)
        {
            TransferWater(other);
        }
        
        OnCancelSelect();
        other.PlayFillAnim(moveAmount, color);
    }

    private void TransferWater(BottleCtrl other)
    {
        int color = GetMoveOutTop();
        WaterItem item = GetMoveOutItemTop();
        
        other.ReceiveWater(color, item);
        RemoveTopWater();
        GameCtrl.Instance.control = false;
    }

    private void RemoveTopWater()
    {
        if (topIdx >= 0)
        {
            waters.RemoveAt(topIdx);
            waterItems.RemoveAt(topIdx);
            hideWaters.RemoveAt(topIdx);
        }
    }

    public void ReceiveWater(int water, WaterItem item)
    {
        if (water > 0)
        {
            waters.Add(water);
            waterItems.Add(item);
            hideWaters.Add(false);
        }
        CheckFinish();
    }

    public void CheckFinish(bool isChange = false)
    {
        if (!ShouldCheckFinish()) return;
        
        if (IsBottleComplete())
        {
            OnFinish();
        }
    }

    private bool ShouldCheckFinish()
    {
        return topIdx > 0 && 
               !_data.IsNearHide && 
               !_data.IsClearHide && 
               !_data.IsFinish;
    }

    private bool IsBottleComplete()
    {
        if (topIdx != maxNum - 1) return false;
        
        int topColor = waters[topIdx];
        return waters.Take(4).All(w => w == topColor) &&
               waterItems.Take(4).All(i => i != WaterItem.Ice);
    }

    public void OnFinish()
    {
        _data.IsFinish = true;

        LevelManager.Instance.FinishClear(GetMoveOutTop(), bottleIdx);

        for (int i = 0; i < waterItems.Count; i++)
        {
            if (waterItems[i] == WaterItem.BreakIce)
            {
                LevelManager.Instance.BreakIce();

                waterItems[i] = WaterItem.None;
            }

            if (waterItems[i] == WaterItem.Bomb)
            {
                LevelManager.Instance.CancelBomb();
                waterItems[i] = WaterItem.None;
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

        if (isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "attack", false);
        }

        yield return new WaitForSeconds(0.2f);

    
        finishGo.SetActive(_data.IsFinish);
    }

    public void UnlockIceWater()
    {
        for (int i = 0; i < waterItems.Count; i++)
        {
            if (waterItems[i] == WaterItem.Ice)
            {
                waterItems[i] = WaterItem.None;
                CheckFinish();
                break;
            }
        }
        CheckWaterItem();
    }

    public void CheckHide(bool isFirst = false)
    {
        if (hideWaters.Count > waters.Count)
        {
            while (hideWaters.Count > waters.Count)
            {
                hideWaters.RemoveAt(hideWaters.Count - 1);
            }
        }
        else if (hideWaters.Count < waters.Count)
        {
            while (hideWaters.Count < waters.Count)
            {
                hideWaters.Add(false);
            }
        }

        if(!isFirst)
        {
            if (hideWaters.Count > 0 && waters.Count > 0)
            {
                hideWaters[waters.Count - 1] = false;

                for (int i = waters.Count - 1; i >= 0; i--)
                {
                    if ((topIdx - 1 >= 0) && (waters[i] == waters[topIdx - 1]))
                    {
                        hideWaters[i] = false;
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

        if (_data.IsFinish)
        {
            for (int i = 0; i < hideWaters.Count; i++)
            {
                hideWaters[i] = false;
            }
        }

        for (int i = 0; i < waters.Count; i++)
        {
            var useColor = waters[i] - 1;
            if (useColor < 1000)
            {
                Debug.Log("UseColor " + useColor);
                waterImg[i].SetColorState(ItemType.UseColor, LevelManager.Instance.waterColor[useColor]);
            }
            else
            {
                waterImg[i].SetColorState((ItemType)waters[i], LevelManager.Instance.ItemColor);
                //var checkColor = LevelManager.Instance.waterColor[useColor - 1000];
            }

            if (hideWaters.Count > 0)
            {
                waterImg[i].SetHide(hideWaters[i], nowaitHide);
            }
            waterImg[i].waterColor = useColor;
        }

        for (int i = 0; i < waterImg.Count; i++)
        {
            Debug.Log(name + "显示水 " + (i < waters.Count || waterImg[i].isPlayItemAnim) + " i " + i + "  waters.Count " + waters.Count + " isPlayItemAnim " + waterImg[i].isPlayItemAnim);
            waterImg[i].gameObject.SetActive(i < waters.Count || waterImg[i].isPlayItemAnim);
            //waterImg[i].spineGo.SetActive(false);
        }

        CheckWaterItem();
        SetClearHide();

        int spinePosIdx = topIdx + 1;
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();
    }

    void CheckWaterItem()
    {
        for (int i = 0; i < waterItems.Count; i++)
        {
            switch (waterItems[i])
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
        int startIdx = topIdx + 1 - num;
        if (color < 1000)
        {
            spineGo.gameObject.SetActive(true);
            SetNowSpinePos(startIdx);
            spineGo.DOMove(spineNode[topIdx + 1].position, fillAlltime).SetEase(Ease.Linear);
        }
        else
        {
            if(startIdx >= 0)
            {
                SetNowSpinePos(startIdx);
            }
            //spineGo.transform.position = spineNode[topIdx + 1].position;
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
        int spinePosIdx = topIdx;

        if (topIdx >= 0)
        {
            for(int i = spinePosIdx; i >= 0; i--)
            {
                if (waters[spinePosIdx] < 1000)
                {
                    spinePosIdx = i;
                    break;
                }
            }
            //while (spinePosIdx > 0 && waters[spinePosIdx] > 1000)
            //{
            //    spinePosIdx--;
            //}
            var color = waters[spinePosIdx];
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
        if(useNode - 1 < waters.Count)
        {
            for (int i = node - 1; i >= 0; i--)
            {
                if (waters[i] < 1000)
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
            if (topIdx < 0)
            {
                spineGo.gameObject.SetActive(false);
            }
        }
        else
        {
            spineGo.DOLocalMove(spineNode[useIdx + 1 - num].localPosition, fillAlltime).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (topIdx < 0)
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

                modelGo.transform.DORotate(_waterRotations[leftWater], 0.62f).SetEase(Ease.Linear).OnComplete(() =>
                //modelGo.transform.DORotate(_waterRotations[leftWater], 10.0f).SetEase(Ease.Linear).OnComplete(() =>
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


        for (int i = 0; i < waters.Count; i++)
        {
            var waterColor = waters[i];

            if(waterColor > 1000)
            {
                if(itemId == 0)
                {
                    itemPlace = i;
                    itemId = waterColor;
                }
                else
                {
                    if(waters[i] == itemId)
                    {
                        switch (waters[i])
                        {
                            case (int)ItemType.ClearItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(waters[i]);
                                    waterImg[i - 1].PlayUseBroom(waterImg[i]);
                                    //waterImg[i].gameObject.SetActive(true);
                                    //StartCoroutine(waterImg[i - 1].ShowBroomAfter());

                                    waters[i] = 0;
                                    waters[itemPlace] = 0;
                                }
                                break;
                            case (int)ItemType.MakeColorItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(waters[i]);
                                    waterImg[i - 1].PlayUseCreate(this, waterImg[i]);
                                    waters[i] = 0;
                                    waters[itemPlace] = 0;
                                    waterImg[i].color =new Color(1,1,1,0);
                                    waterImg[i].broomItemGo.SetActive(false);
                                    waterImg[i].createItemGo.SetActive(false);
                                    waterImg[i].changeItemGo.SetActive(false);
                                    waterImg[i].magnetItemGo.SetActive(false);
                                }
                                break;
                            case (int)ItemType.MagnetItem:
                                if (itemId == waterColor && i - itemPlace == 1)
                                {
                                    items.Add(waters[i]);
                                    waterImg[i - 1].PlayUseMagnet(waterImg[i]);
                                    waters[i] = 0;
                                    waters[itemPlace] = 0;
                                }
                                break;
                            default:
                                if(waters[i] > 2000 && waters[i] < 3000)
                                {
                                    if (itemId == waterColor && i - itemPlace == 1)
                                    {
                                        items.Add(waters[i]);
                                        waterImg[i - 1].PlayUseChange(waterImg[i]);
                                        waters[i] = 0;
                                        waters[itemPlace] = 0;
                                    }
                                }
                                else if (waters[i] > 3000)
                                {
                                    if (itemId == waterColor && i - itemPlace == 1)
                                    {
                                        items.Add(waters[i]);
                                        waterImg[i - 1].PlayUseBroom(waterImg[i]);
                                        //StartCoroutine(waterImg[i - 1].ShowBroomAfter());

                                        waters[i] = 0;
                                        waters[itemPlace] = 0;
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

        for(int i = 0; i < waters.Count;i++)
        {
            if (waters[i] != 0)
            {
                temp.Add(waters[i]);
                tempItem.Add(waterItems[i]);
            }
        }
        waters = temp;
        waterItems = tempItem;

        for (int i = 0; i < items.Count; i++)
        {
            int useItem = items[i];
            LevelManager.Instance.UseItem(useItem, waterImg[itemPlace].transform);
        }


        spineGo.gameObject.SetActive(topIdx >= 0);
        PlaySpineWaitAnim();
        SetNowSpinePos(topIdx + 1);
    }

    public void PlayBroomBullet(int color, Vector3 fromPos)
    {
        List<BottleWaterCtrl> list = new List<BottleWaterCtrl>();
        for (int i = 0; i < waters.Count; i++)
        {
            if (waters[i] == color)
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

        //int count = waters.Count;
        List<int> list = new List<int>();
        List<WaterItem> items = new List<WaterItem>();
        List<bool> hides = new List<bool>();
        for(int i = 0; i < waters.Count; i++)
        {
            if (waters[i] == color)
            {
                StartCoroutine(PlayShine(i));
            }
            else
            {
                list.Add(waters[i]);
                items.Add(waterItems[i]);
                hides.Add(hideWaters[i]);
            }
        }

        waterItems = items;
        waters = list;
        hideWaters = hides;


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

        if (topIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
    }

    public void RecordLast()
    {
        var record = new BottleRecord();
        record.isFinish = isFinish;
        record.isNearHide = isNearHide;
        record.isClearHide = isClearHide;
        record.isFreeze = isFreeze;
        record.waters = new List<int>(waters);
        record.hideWaters = new List<bool>(hideWaters);
        record.waterItems = new List<WaterItem>(waterItems);

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

        Init(temp, bottleIdx);
        isFinish = record.isFinish;

        //if (!isFinish)
        //{
        //    var trackEntry = finishSpine.AnimationState.GetCurrent(0); // 获取轨道0上的当前动画条目
        //    if (trackEntry != null)
        //    {
        //        // 重置时间为0
        //        trackEntry.MixDuration = 0;
        //        trackEntry.TrackTime = 0;
        //        trackEntry.TimeScale = 0f;
        //    }
        //}

        moveRecords.Remove(record);
        finishGo.SetActive(isFinish);
    }
}
