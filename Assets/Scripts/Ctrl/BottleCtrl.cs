﻿using System;
using DG.Tweening;
using GameDefine;
using QFramework;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using static LevelCreateCtrl;
using static UnityEngine.GraphicsBuffer;

public class BottleCtrl : MonoBehaviour, IController, ICanSendEvent, ICanRegisterEvent
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

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

    public List<BottleRecord> moveRecords = new List<BottleRecord>();
    public int topIdx
    {
        get
        {
            return waters.Count - 1;
        }
    }

    public int bottleIdx;
    public int unlockClear = 0;
    public Button bottle;

    BottleProperty originProperty;
    // Start is called before the first frame update
    void Start()
    {
        bottle.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        if(!LevelManager.Instance.isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
        {
            GameCtrl.Instance.OnSelect(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(BottleProperty property, int idx)
    {
        //foreach (var anim in spine.SkeletonData.Animations)
        //{
        //    Debug.Log("动画名称 " + anim.name);

        //}
        originProperty = property;
        isFinish = false; isFreeze = false; isClearHideAnim = false;
        finishGo.SetActive(isFinish);
        waters = new List<int>(property.waterSet);
        hideWaters = new List<bool>(property.isHide);
        waterItems = new List<WaterItem>(property.waterItem);
        isClearHide = property.isClearHide;
        isNearHide = property.isNearHide;
        isFreeze = property.isFreeze;
        unlockClear = property.lockType;
        limitColor = property.limitColor;
        bottleIdx = idx;

        nearHide.gameObject.SetActive(isNearHide);
        if(nearHide)
        {
            nearHide.AnimationState.SetAnimation(0, "idle", true);
        }

        foreach (var bottle in waterImg)
        {
            bottle.waterImg.fillAmount = 1;
        }

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

        //if(unlockClear != 0)
        //{
        //    LevelManager.Instance.cantChangeColorList.Add(unlockClear);
        //}

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
        //while (spinePosIdx > 0 && waters[spinePosIdx - 1] > 1000)
        //{
        //    spinePosIdx -= 1;
        //}
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

        freezeGo.gameObject.SetActive(isFreeze);
        if(limitColor != 0)
        {
            ImgLimit.color = LevelManager.Instance.waterColor[limitColor - 1];
        }
        else
        {
            ImgLimit.color = new Color(1,1,1,0);
        }


        if(isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "idle", false);
        }

         Debug.Log("名字 " + name);
     
    }

    public void MoveBottle(BottleCtrl bottleCtrl)
    {
        isFinish = bottleCtrl.isFinish; isFreeze = bottleCtrl.isFreeze;
        waters = new List<int>(bottleCtrl.waters);
        hideWaters = new List<bool>(bottleCtrl.hideWaters);
        waterItems = new List<WaterItem>(bottleCtrl.waterItems);
        isClearHide = bottleCtrl.isClearHide;
        isNearHide = bottleCtrl.isNearHide;
        unlockClear = bottleCtrl.unlockClear;
        bottleIdx = bottleCtrl.bottleIdx;
        nearHide.gameObject.SetActive(isNearHide);
        SetClearHide();
        SetBottleColor(true);
        int spinePosIdx = topIdx + 1;
        //while (spinePosIdx > 0 && waters[spinePosIdx - 1] > 1000)
        //{
        //    spinePosIdx -= 1;
        //}
        SetNowSpinePos(spinePosIdx);
        PlaySpineWaitAnim();
        if (topIdx < 0)
        {
            spineGo.gameObject.SetActive(false);
        }
        foreach (var item in waterItems)
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

    public void RemovHide()
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
        if (waters.Count >= maxNum)
        {

        }
        else
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

        if (color < 1000)///判断非道具
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
        isClearHideAnim = true;
        yield return new WaitForSeconds(1.5f);
        switch (unlockClear)
        {
            case 1:
                clearHide.AnimationState.SetAnimation(0, "disapear_cl", false);
                break;
            case 2:
                clearHide.AnimationState.SetAnimation(0, "disapear_dh", false);
                break;
            case 3:
                clearHide.AnimationState.SetAnimation(0, "disapear_fh", false);
                break;
            case 4:
                clearHide.AnimationState.SetAnimation(0, "disapear_gl", false);
                break;
            case 5:
                clearHide.AnimationState.SetAnimation(0, "disapear_hl", false);
                break;
            case 6:
                clearHide.AnimationState.SetAnimation(0, "disapear_hs", false);
                break;
            case 7:
                clearHide.AnimationState.SetAnimation(0, "disapear_jh", false);
                break;
            case 8:
                clearHide.AnimationState.SetAnimation(0, "disapear_lh", false);
                break;
            case 9:
                clearHide.AnimationState.SetAnimation(0, "disapear_sl", false);
                break;
            case 10:
                clearHide.AnimationState.SetAnimation(0, "disapear_ze", false);
                break;
            case 11:
                clearHide.AnimationState.SetAnimation(0, "disapear_zs", false);
                break;
            case 12:
                clearHide.AnimationState.SetAnimation(0, "disapear_mh", false);
                break;

        }
        isClearHide = false;
        CheckFinish();

        yield return new WaitForSeconds(3.7f);
        clearHide.gameObject.SetActive(false);
        isClearHideAnim = false;
    }

    void SetClearHide()
    {
        if(!isClearHideAnim)
        {
            clearHide.gameObject.SetActive(isClearHide);
            //ImgClearHide.gameObject.SetActive(isClearHide);
            if (isClearHide)
            {
                switch (unlockClear)
                {
                    case 1:
                        clearHide.AnimationState.SetAnimation(0, "idle_cl", false);
                        break;
                    case 2:
                        clearHide.AnimationState.SetAnimation(0, "idle_dh", false);
                        break;
                    case 3:
                        clearHide.AnimationState.SetAnimation(0, "idle_fh", false);
                        break;
                    case 4:
                        clearHide.AnimationState.SetAnimation(0, "idle_gl", false);
                        break;
                    case 5:
                        clearHide.AnimationState.SetAnimation(0, "idle_hl", false);
                        break;
                    case 6:
                        clearHide.AnimationState.SetAnimation(0, "idle_hs", false);
                        break;
                    case 7:
                        clearHide.AnimationState.SetAnimation(0, "idle_jh", false);
                        break;
                    case 8:
                        clearHide.AnimationState.SetAnimation(0, "idle_lh", false);
                        break;
                    case 9:
                        clearHide.AnimationState.SetAnimation(0, "idle_sl", false);
                        break;
                    case 10:
                        clearHide.AnimationState.SetAnimation(0, "idle_ze", false);
                        break;
                    case 11:
                        clearHide.AnimationState.SetAnimation(0, "idle_zs", false);
                        break;
                    case 12:
                        clearHide.AnimationState.SetAnimation(0, "idle_mh", false);
                        break;

                }
            }
        }
       
    }

    public int GetLeftEmpty()
    {
        return maxNum - 1 - topIdx;
    }

    public void MoveTo(BottleCtrl other)
    {
        int moveNum = other.GetLeftEmpty();
        int sameNum = 1;

        for (int i = topIdx - 1; i >= 0; i--)
        {
            if (waters[i] == GetMoveOutTop() && waterItems[i] != WaterItem.Ice)
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
        MoveToOtherAnim(other, color);
        PlayOutAnim(moveNum, topIdx, color);

        for (int i = 0; i < moveNum; i++)
        {
            other.ReceiveWater(color, GetMoveOutItemTop());
            int idx = topIdx;
            if (waters.Count > 0)
            {
                waterImg[idx].wenhaoFxGo.SetActive(false);
                waterImg[idx].HideGo.SetActive(false);
                waters.RemoveAt(idx);
                waterItems.RemoveAt(idx);
                hideWaters.RemoveAt(idx);
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
            waters.Add(water);
            waterItems.Add(item);
            hideWaters.Add(false);
        }
        CheckFinish();
    }

    public void CheckFinish(bool isChange = false)
    {

        if (topIdx > 0 && !isNearHide && !isClearHide && !isFinish)
        {
            var topColor = waters[topIdx];
            if (topIdx == maxNum - 1)
            {
                for(int i = 3; i >=0; i--)
                {
                    var water = waters[i];
                    if (water != topColor || waterItems[i] == WaterItem.Ice)
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
        isFinish = true;

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

    
        finishGo.SetActive(isFinish);
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

        if (isFinish)
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
                waterImg[i].color = LevelManager.Instance.waterColor[useColor];
                waterImg[i].broomItemGo.SetActive(false);
                waterImg[i].createItemGo.SetActive(false);
                waterImg[i].changeItemGo.SetActive(false);
                waterImg[i].magnetItemGo.SetActive(false);
            }
            else
            {
                switch (waters[i])
                {
                    case (int)ItemType.ClearItem:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                        break;
                    case (int)ItemType.MagnetItem:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(true);
                        waterImg[i].magnetSpine.AnimationState.SetAnimation(0, "idle", false);
                        break;
                    case (int)ItemType.MakeColorItem:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(true);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].createSpine.AnimationState.SetAnimation(0, "idle", false);

                        break;
                    case (int)ItemType.ChangeGreen:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                        //waterImg[i].color = new Color(1, 1, 1, 0);
                        break;
                    case (int)ItemType.ChangeOrange:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_jh", false);

                        //waterImg[i].color = new Color(1, 1, 1, 0);
                        break;
                    case (int)ItemType.ChangePink:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_fs", false);

                        //waterImg[i].color = new Color(1, 1, 1, 0);
                        break;
                    case (int)ItemType.ChangePurple:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_zs", false);

                        break;
                    case (int)ItemType.ChangeYellow:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_hs", false);

                        break;
                    case (int)ItemType.ChangeDarkBlue:
                        waterImg[i].broomItemGo.SetActive(false);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(true);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_sl", false);

                        break;
                    case (int)ItemType.ClearPink:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_fh", false);

                        break;
                    case (int)ItemType.ClearOrange:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_jh", false);

                        break;
                    case (int)ItemType.ClearBlue:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_gl", false);

                        break;
                    case (int)ItemType.ClearYellow:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_hs", false);

                        break;
                    case (int)ItemType.ClearDarkGreen:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_sl", false);

                        break;
                    case (int)ItemType.ClearRed:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_dh", false);

                        break;
                    case (int)ItemType.ClearGreen:
                        waterImg[i].broomItemGo.SetActive(true);
                        waterImg[i].createItemGo.SetActive(false);
                        waterImg[i].changeItemGo.SetActive(false);
                        waterImg[i].magnetItemGo.SetActive(false);
                        waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                        break;
                }
                //var checkColor = LevelManager.Instance.waterColor[useColor - 1000];
                waterImg[i].color = LevelManager.Instance.ItemColor;

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
        //while (spinePosIdx > 0 && waters[spinePosIdx - 1] > 1000)
        //{
        //    spinePosIdx -= 1;
        //}
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
        string spineAnimName = "";
        var color = GetMoveOutTop();
        switch (color)
        {
            case 1:
                spineAnimName = "daoshui_cl";
                break;
            case 2:
                spineAnimName = "daoshui_dh";
                break;
            case 3:
                spineAnimName = "daoshui_fh";
                break;
            case 4:
                spineAnimName = "daoshui_gl";
                break;
            case 5:
                spineAnimName = "daoshui_hl";
                break;
            case 6:
                spineAnimName = "daoshui_hs";
                break;
            case 7:
                spineAnimName = "daoshui_jh";
                break;
            case 8:
                spineAnimName = "daoshui_lh";
                break;
            case 9:
                spineAnimName = "daoshui_sl";
                break;
            case 10:
                spineAnimName = "daoshui_ze";
                break;
            case 11:
                spineAnimName = "daoshui_zs";
                break;
            case 12:
                spineAnimName = "daoshui_mh";
                break;  
        }

        if(color < 1000 && color != 0)
        {
            spine.AnimationState.SetAnimation(0, spineAnimName, false);
        }
    }
    public void PlaySpineWaitAnim(int useColor = -1)
    {
        string spineAnimName = "";
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
            switch (color)
            {
                case 1:
                    spineAnimName = "ruchanghuangdong_cl";
                    break;
                case 2:
                    spineAnimName = "ruchanghuangdong_dh";
                    break;
                case 3:
                    spineAnimName = "ruchanghuangdong_fh";
                    break;
                case 4:
                    spineAnimName = "ruchanghuangdong_gl";
                    break;
                case 5:
                    spineAnimName = "ruchanghuangdong_hl";
                    break;
                case 6:
                    spineAnimName = "ruchanghuangdong_hs";
                    break;
                case 7:
                    spineAnimName = "ruchanghuangdong_jh";
                    break;
                case 8:
                    spineAnimName = "ruchanghuangdong_lh";
                    break;
                case 9:
                    spineAnimName = "ruchanghuangdong_sl";
                    break;
                case 10:
                    spineAnimName = "ruchanghuangdong_ze";
                    break;
                case 11:
                    spineAnimName = "ruchanghuangdong_zs";
                    break;
                case 12:
                    spineAnimName = "ruchanghuangdong_mh";
                    break;
            }
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
        else
        {
            //spineGo.gameObject.SetActive(false);
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

    public void MoveToOtherAnim(BottleCtrl other, int useColor = -1)
    {
        LevelManager.Instance.isPlayAnim = true;

        var bottleRenderUpdate = bottleAnim.GetComponent<BottleRenderUpdate>();
        bottleRenderUpdate.SetMoveBottleRenderState(true);
        if(useColor < 1000)
        {
            bottleAnim.Play("BottleOut");
        }
        else
        {
            bottleAnim.Play("BottleItemOut");
        }
        //modelGo.transform.DOMove(other.leftMovePlace.position, 0.67f).SetEase(Ease.Linear).OnComplete(() =>
        modelGo.transform.DOMove(other.leftMovePlace.position, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetDownWaterSp(useColor);
            if (useColor < 1000) ////非道具动画播放
            {
                PlayWaterDown();

                modelGo.transform.DOMove(other.leftMovePlace.position, 0.62f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    modelGo.transform.DOLocalMove(Vector3.zero, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        LevelManager.Instance.isPlayAnim = false;
                        bottleRenderUpdate.SetMoveBottleRenderState(false);
                    });
                });
            }
            else
            {
                modelGo.transform.DOMove(other.leftMovePlace.position, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    modelGo.transform.DOLocalMove(Vector3.zero, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        LevelManager.Instance.isPlayAnim = false;
                        bottleRenderUpdate.SetMoveBottleRenderState(false);
                    });
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

    private void LateUpdate()
    {
        fillWaterGoAnim.transform.localRotation = Quaternion.Inverse(fillWaterGoAnim.transform.parent.rotation);
    }
}
