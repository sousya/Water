using DG.Tweening;
using GameDefine;
using QFramework;
using QFramework.Example;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
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

    // 是否 已完成，结冰，清除隐藏，邻近隐藏，正在播放动画，播放清除隐藏动画 状态
    public bool isFinish, isFreeze, isClearHide, isNearHide, isPlayAnim, isClearHideAnim;
    public bool isUp;       // 区分上下排
    
    public int bottleIdx;   // 瓶子的索引编号
    public int maxNum = 4,  // 瓶子的最大容量（默认4层）
        limitColor = 0,     // 限制可倒入的颜色（0表示无限制）
        unlockClear = 0;    // 解锁清除隐藏需要的颜色编号

    // 存储每层液体的颜色编号（1-12表示颜色，>1000表示道具）
    public List<int> waters = new List<int>();
    // 黑色遮挡
    public List<bool> hideWaters = new List<bool>();
    // 每层液体的附加道具类型（冰块等）
    public List<WaterItem> waterItems = new List<WaterItem>();
    public List<BottleWaterCtrl> waterImg = new List<BottleWaterCtrl>();
    // 操作记录（用于撤销功能）
    public List<BottleRecord> moveRecords = new List<BottleRecord>();
    // Spine动画位置节点（不同水位对应位置）
    public List<Transform> spineNode = new List<Transform>();
    // 水位填充位置节点
    public List<Transform> waterNode = new List<Transform>();

    public Transform
        spineGo,      // 倒水过程水花动画父节点(当前水面位置)
        modelGo,      // 瓶子初始点位
        leftMovePlace,// 向该瓶子倒水时的目标位置 
        freezeGo;     // 冰冻特效挂载点  

    //  瓶子整体动画控制器，模拟倒水动画控制器
    public Animator bottleAnim, fillWaterGoAnim;

    public SkeletonGraphic 
        spine,      // 倒水过程水花动画
        finishSpine,// 完成状态动画
        freezeSpine,// 冰冻效果动画
        bubbleSpine;// 气泡特效动画

    // 水柱 顶部，底部，容量1的瓶子，容量2的瓶子，容量3的瓶子，容量4的瓶子 颜色贴图
    public Image ImgWaterTop, ImgWaterDown, ImgBottleOne, ImgBottleTwo, ImgBottleThree, ImgBottleFour;

    public SkeletonGraphic 
        nearHide,           // 魔法部动画
        clearHide,          // 藤蔓动画 
        limitColorSpine;    // 限制颜色提示动画

    public GameObject finishGo,  // 完成状态提示图标
        bottleOne,  // 容量1的瓶子UI
        bottleTwo,  // 容量2的瓶子UI
        bottleThree,// 容量3的瓶子UI 
        bottleFour; // 容量4的瓶子UI
   
    // 当前顶部水块的索引
    public int topIdx
    {
        get
        {
            // 通过列表长度动态计算
            return waters.Count - 1;
        }
    }

    // 瓶子的点击按钮
    public Button bottle;
    // 瓶子的初始属性配置
    BottleProperty originProperty;

    void Start()
    {
        bottle.onClick.AddListener(OnSelected);

        //初始化瓶子位置
        this.RegisterEvent<GameStartEvent>(e =>
        {
            OnCancelSelect();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnSelected()
    {
        if (!isPlayAnim && !LevelManager.Instance.isPlayFxAnim)
        {
            GameCtrl.Instance.OnSelect(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="property"></param>
    /// <param name="idx"></param>
    public void Init(BottleProperty property, int idx)
    {
        //foreach (var anim in spine.SkeletonData.Animations)
        //{
        //    Debug.Log("动画名称 " + anim.name);

        //}
        originProperty = property;
        isFinish = false; isFreeze = false; isClearHideAnim = false;
        finishGo.SetActive(isFinish);
        bubbleSpine.gameObject.SetActive(false);

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
        if (nearHide)
        {
            nearHide.AnimationState.SetAnimation(0, "idle", true);
        }

        foreach (var bottle in waterImg)
        {
            bottle.waterImg.fillAmount = 1;
        }

        for (int i = 0; i < waters.Count; i++)
        {
            if (isClearHide || isNearHide || waterItems[i] == WaterItem.Ice)
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
        //PlaySpineWaitAnim();//重复调用

        foreach (var item in waterItems)
        {
            if (item == WaterItem.Ice)
            {
                LevelManager.Instance.iceBottles.Add(this);
            }
        }
        CheckFinish();

        freezeGo.gameObject.SetActive(isFreeze);
        if (limitColor != 0)
        {
            limitColorSpine.gameObject.SetActive(true);
            switch (limitColor)
            {
                case 1:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_cl", false);
                    break;
                case 2:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_dh", false);
                    break;
                case 3:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_fh", false);
                    break;
                case 4:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_gl", false);
                    break;
                case 5:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_hl", false);
                    break;
                case 6:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_hs", false);
                    break;
                case 7:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_jh", false);
                    break;
                case 8:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_lh", false);
                    break;
                case 9:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_sl", false);
                    break;
                case 10:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_ze", false);
                    break;
                case 11:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_zs", false);
                    break;
                case 12:
                    limitColorSpine.AnimationState.SetAnimation(0, "idle_mh", false);
                    break;
            }
        }
        else
        {
            limitColorSpine.gameObject.SetActive(false);
        }


        if (isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "idle", false);
        }

        for (int i = 0; i < hideWaters.Count; i++)
        {
            if (hideWaters[i])
            {
                LevelManager.Instance.hideBottleList.Add(this);
                break;
            }
        }
        //Debug.Log("名字 " + name);
        SetMaxBottle();

    }

    /// <summary>
    /// 瓶子移动
    /// </summary>
    /// <param name="bottleCtrl"></param>
    public void MoveBottle(BottleCtrl bottleCtrl)
    {
        //Debug.Log($"当前瓶子:{this.gameObject.name} , 索引：{bottleIdx}" );
        //Debug.Log($"参数瓶子:{bottleCtrl.gameObject.name} , 索引：{bottleCtrl.bottleIdx}");

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
        StartCoroutine(CheckChange(from, to, target));

    }

    /// <summary>
    /// 检测变色
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="target"></param>
    /// <returns></returns>
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

        //会重复触发(暂弃)
        /*if (isFinish)
        //{
        //    LevelManager.Instance.CheckFinishChange(to);
        //}*/
    }

    /// <summary>
    /// 移除黑色水块
    /// </summary>
    public void RemovHide()
    {
        for (int i = 0; i < hideWaters.Count; i++)
        {
            hideWaters[i] = false;
        }
        LevelManager.Instance.hideBottleList.Remove(this);

        SetBottleColor();
        CheckFinish();
    }

    /// <summary>
    /// 清除所有特殊情况
    /// </summary>
    /// <returns></returns>
    public void SetNormal()
    {
        for (int i = 0; i < hideWaters.Count; i++)
        {
            hideWaters[i] = false;
        }

        for (int i = 0; i < waterItems.Count; i++)
        {
            waterItems[i] = WaterItem.None;
        }

        if (isFreeze)
        {
            freezeSpine.AnimationState.SetAnimation(0, "attack", false);
        }

        if (limitColor != 0)
        {
            switch (limitColor)
            {
                case 1:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_cl", false);
                    break;
                case 2:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_dh", false);
                    break;
                case 3:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_fh", false);
                    break;
                case 4:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_gl", false);
                    break;
                case 5:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_hl", false);
                    break;
                case 6:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_hs", false);
                    break;
                case 7:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_jh", false);
                    break;
                case 8:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_lh", false);
                    break;
                case 9:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_sl", false);
                    break;
                case 10:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_ze", false);
                    break;
                case 11:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_zs", false);
                    break;
                case 12:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_mh", false);
                    break;
            }
        }

        isFreeze = false;
        isNearHide = false;
        isClearHide = false;
        StartCoroutine(CoroutinePlayNearHide(true));
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
        isPlayAnim = true;
        yield return new WaitForSeconds(1);
        Debug.Log("fx " + waters.Count + " " + name);
        CheckItem();
        SetBottleColor();
        CheckFinish();
        foreach (var item in waterImg)
        {
            item.waterImg.fillAmount = 1;
        }

        isPlayAnim = false;
    }

    /// <summary>
    /// 判断是否能选中 如果能 则选中
    /// </summary>
    /// <returns></returns>
    public bool OnSelect(bool needUp)
    {
        ////判断是否被冰冻,隐藏或者完成
        if ((isFreeze && needUp) || isClearHide || isNearHide || isFinish || isClearHideAnim)
        {
            return false;
        }
        if (needUp)
        {
            modelGo.transform.localPosition += Vector3.up * 100;
        }
        return true;
    }

    /// <summary>
    /// 取消选中
    /// </summary>
    public void OnCancelSelect()
    {
        modelGo.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 判断能否倒出
    /// </summary>
    /// <returns></returns>
    public bool CheckMoveOut()
    {
        if (topIdx < 0 || waterItems[topIdx] == WaterItem.Ice)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 取得倒出水的颜色
    /// </summary>
    /// <returns></returns>
    public int GetMoveOutTop()
    {
        if (topIdx < 0)
        {
            return 0;
        }
        return waters[topIdx];
    }

    /// <summary>
    /// 取得倒出水的对应WaterItem
    /// </summary>
    /// <returns></returns>
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
        if (isClearHide || isNearHide || isFinish || GetLeftEmpty() == 0 || (limitColor != 0 && limitColor != color))
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

    /// <summary>
    /// 判断临近(荆棘)消除
    /// </summary>
    /// <param name="idx"></param>
    public void CheckNearHide(int idx)
    {
        //可能还需要调整逻辑
        if (Mathf.Abs(bottleIdx - idx) == 1 && LevelManager.Instance.nowBottles[idx].isUp == isUp)
        {
            //Debug.Log($"瓶子名：{this.name}，瓶子索引：{bottleIdx}，完成的瓶子：{idx}");

            //这里可以修改为只判定isNearHide的瓶子
            foreach (var item in waters)
            {
                LevelManager.Instance.cantChangeColorList.Remove(item);
            }

            //isNearHide = false;
            SetClearHide();
            CheckFinish();
            StartCoroutine(CoroutinePlayNearHide());
        }
    }

    /// <summary>
    /// 临近(荆棘)消除动画表现相关
    /// </summary>
    /// <param name="nowait"></param>
    /// <returns></returns>
    IEnumerator CoroutinePlayNearHide(bool nowait = false)
    {
        if (!nowait)
        {
            yield return new WaitForSeconds(2f);
        }
        nearHide.AnimationState.SetAnimation(0, "jingji_xiaoshi", false);
        yield return new WaitForSeconds(1.7f);
        nearHide.gameObject.SetActive(false);
        isNearHide = false;
    }

    /// <summary>
    /// 判断颜色解锁
    /// </summary>
    /// <param name="color"></param>
    public void CheckUnlockHide(int color)
    {
        if (isClearHide)
        {
            if (unlockClear == color)
            {
                ++LevelManager.Instance.playingHideAnimCount;
                UIKit.OpenPanel<UIMask>(UILevel.PopUI);
                foreach (var item in waters)
                {
                    LevelManager.Instance.cantChangeColorList.Remove(item);
                }
                LevelManager.Instance.cantChangeColorList.Remove(color);
                StartCoroutine(HideClearHide());
            }
        }

        //SetClearHide();
        //CheckFinish();
    }

    /// <summary>
    /// 颜色解锁动画
    /// </summary>
    /// <returns></returns>
    IEnumerator HideClearHide()
    {
        isClearHideAnim = true;
        yield return new WaitForSeconds(1.5f);
        //加入事件
        TrackEntry trackEntry = null;
        switch (unlockClear)
        {
            case 1:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_cl", false);
                break;
            case 2:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_dh", false);
                break;
            case 3:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_fh", false);
                break;
            case 4:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_gl", false);
                break;
            case 5:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_hl", false);
                break;
            case 6:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_hs", false);
                break;
            case 7:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_jh", false);
                break;
            case 8:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_lh", false);
                break;
            case 9:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_sl", false);
                break;
            case 10:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_ze", false);
                break;
            case 11:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_zs", false);
                break;
            case 12:
                trackEntry = clearHide.AnimationState.SetAnimation(0, "disapear_mh", false);
                break;

        }
        if (trackEntry != null)
        {
            // 注册动画完成事件
            trackEntry.Complete += (entry) =>
            {
                clearHide.gameObject.SetActive(false);
                isClearHideAnim = false; 
                --LevelManager.Instance.playingHideAnimCount;
                //计数为0且未过关(15关较为特殊)
                if (LevelManager.Instance.ISPlayingHideAnim && LevelManager.Instance.clearList.Count != 0)
                {
                    UIKit.ClosePanel<UIMask>();
                }
            };
        }

        isClearHide = false;
        CheckFinish();
    }

    /// <summary>
    /// 设置清除隐藏Spine动画
    /// </summary>
    void SetClearHide()
    {
        if (!isClearHideAnim)
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

    /// <summary>
    /// 获得剩余空位
    /// </summary>
    /// <returns></returns>
    public int GetLeftEmpty()
    {
        return maxNum - 1 - topIdx;
    }

    /// <summary>
    /// 倒水到另一个瓶子
    /// </summary>
    /// <param name="other"></param>
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

    /// <summary>
    /// 接收水
    /// </summary>
    /// <param name="water"></param>
    /// <param name="item"></param>
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

    /// <summary>
    /// 判断是否完成
    /// </summary>
    /// <param name="isChange"></param>
    public void CheckFinish(bool isChange = false)
    {

        if (topIdx > 0 && !isNearHide && !isClearHide && !isFinish)
        {
            var topColor = waters[topIdx];
            if (maxNum == 4 && topIdx == maxNum - 1)
            {
                for (int i = 3; i >= 0; i--)
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

    /// <summary>
    /// 完成后的处理
    /// </summary>
    public void OnFinish()
    {
        isFinish = true;
        //Debug.Log($"完成后处理-该瓶子：{this.gameObject.name} ,该瓶子索引：{bottleIdx}");
        LevelManager.Instance.FinishClear(GetMoveOutTop(), bottleIdx);
        StartCoroutine(ShowBreakIce());


        for (int i = 0; i < waterItems.Count; i++)
        {
            if (waterItems[i] == WaterItem.Bomb)
            {
                LevelManager.Instance.CancelBomb();
                waterItems[i] = WaterItem.None;
            }
        }

        CheckWaterItem();
        StartCoroutine(ShowFinish());
    }

    /// <summary>
    /// 破冰动画
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBreakIce()
    {
        yield return new WaitForSeconds(1f);
        for (int i = waterItems.Count - 1; i >= 0; i--)
        {
            if (waterItems[i] == WaterItem.BreakIce)
            {
                var breakTo = LevelManager.Instance.BreakIce();

                StartCoroutine(waterImg[i].BreakIce(breakTo));
                waterItems[i] = WaterItem.None;
                CheckWaterItem();

                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    /// <summary>
    /// 完成动画
    /// </summary>
    /// <returns></returns>
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


        yield return new WaitForSeconds(1f);



        if (limitColor != 0)
        {
            switch (limitColor)
            {
                case 1:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_cl", false);
                    break;
                case 2:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_dh", false);
                    break;
                case 3:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_fh", false);
                    break;
                case 4:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_gl", false);
                    break;
                case 5:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_hl", false);
                    break;
                case 6:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_hs", false);
                    break;
                case 7:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_jh", false);
                    break;
                case 8:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_lh", false);
                    break;
                case 9:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_sl", false);
                    break;
                case 10:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_ze", false);
                    break;
                case 11:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_zs", false);
                    break;
                case 12:
                    limitColorSpine.AnimationState.SetAnimation(0, "combine_mh", false);
                    break;
            }
        }

        bubbleSpine.gameObject.SetActive(true);
        var trackEntry1 = bubbleSpine.AnimationState.GetCurrent(0); // 获取轨道0上的当前动画条目
        if (trackEntry1 != null)
        {
            //trackEntry.TimeScale = 1f;
            bubbleSpine.Initialize(true);
        }
        else
        {
            // 如果当前没有动画，直接设置动画
            bubbleSpine.AnimationState.SetAnimation(0, "maopao", false);
        }

    }

    /// <summary>
    /// 破冰
    /// </summary>
    public void UnlockIceWater()
    {
        CheckWaterItem();
        CheckFinish();
    }

    /// <summary>
    /// 找冰
    /// </summary>
    /// <returns></returns>
    public BottleWaterCtrl FindIceWater()
    {
        //从上往下找
        for (int i = waterItems.Count - 1; i >= 0; i--)
        {
            if (waterItems[i] == WaterItem.Ice)
            {
                waterItems[i] = WaterItem.None;
                return waterImg[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 判断黑色水块
    /// </summary>
    /// <param name="isFirst"></param>
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

        if (!isFirst)
        {
            if (hideWaters.Count > 0 && waters.Count > 0)
            {
                //最上层的黑水块显示
                hideWaters[waters.Count - 1] = false;
                //黑水块的颜色与顶层是否相同(相同显示)
                for (int i = waters.Count - 1; i >= 0; i--)
                {
                    if ((topIdx >= 0) && (waters[i] == waters[topIdx]))
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

        //判断该瓶子是否还存在黑水块
        bool hasHide = false;
        for (int i = 0; i < hideWaters.Count; i++)
        {
            if (hideWaters[i])
            {
                hasHide = true;
                break;
            }
        }

        if (!hasHide)
        {
            LevelManager.Instance.hideBottleList.Remove(this);
        }
    }

    /// <summary>
    /// 设置水块颜色
    /// </summary>
    /// <param name="isFirst"></param>
    /// <param name="nowaitHide"></param>
    public void SetBottleColor(bool isFirst = false, bool nowaitHide = false)
    {
        CheckHide(isFirst);

        //已完成，清楚黑水块
        if (isFinish)
        {
            for (int i = 0; i < hideWaters.Count; i++)
            {
                hideWaters[i] = false;
            }
        }

        // 遍历每层水块，设置颜色和状态
        for (int i = 0; i < waters.Count; i++)
        {
            // 计算颜色索引，减一是因为颜色编号从 1 开始，而数组索引从 0 开始
            var useColor = waters[i] - 1;
            // 普通颜色水块
            if (useColor < 1000)
            {
                //Debug.Log("UseColor " + useColor);
                waterImg[i].color = LevelManager.Instance.waterColor[useColor];
                waterImg[i].broomItemGo.SetActive(false);
                waterImg[i].createItemGo.SetActive(false);
                waterImg[i].changeItemGo.SetActive(false);
                waterImg[i].magnetItemGo.SetActive(false);
            }
            // 特殊道具水块
            else
            {
                // 根据道具类型设置对应的显示和动画
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
                // 设置道具的颜色为统一的道具颜色
                waterImg[i].color = LevelManager.Instance.ItemColor;
            }

            //将隐藏水块显示
            if (hideWaters.Count > 0)
            {
                SetHideShow(nowaitHide, i);
            }
            waterImg[i].waterColor = useColor;
        }

        // 更新水块的显示状态
        for (int i = 0; i < waterImg.Count; i++)
        {
            //Debug.Log(name + "显示水 " + (i < waters.Count || waterImg[i].isPlayItemAnim) + " i " + i + "  waters.Count " + waters.Count + " isPlayItemAnim " + waterImg[i].isPlayItemAnim);
            waterImg[i].gameObject.SetActive(i < waters.Count || waterImg[i].isPlayItemAnim);
            //waterImg[i].spineGo.SetActive(false);
        }
        // 检查水块的道具状态
        CheckWaterItem();
        // 更新魔法布遮挡状态
        SetClearHide();

        // 更新水面位置
        int spinePosIdx = topIdx + 1;
        //while (spinePosIdx > 0 && waters[spinePosIdx - 1] > 1000)
        //{
        //    spinePosIdx -= 1;
        //}
        SetNowSpinePos(spinePosIdx);
        // 播放等待动画
        PlaySpineWaitAnim();
    }

    /// <summary>
    /// 设置隐藏水块显示
    /// </summary>
    /// <param name="nowaitHide">是否立即触发</param>
    /// <param name="idx"></param>
    public void SetHideShow(bool nowaitHide, int idx = -1)
    {
        if (idx >= 0)
        {
            if (hideWaters.Count > 0)
            {
                waterImg[idx].SetHide(hideWaters[idx], nowaitHide);
            }
        }
        else
        {
            for (int i = 0; i < waters.Count; i++)
            {
                if (hideWaters.Count > 0)
                {
                    waterImg[i].SetHide(hideWaters[i], nowaitHide);
                }
            }
        }
    }

    /// <summary>
    /// 判断水块道具
    /// </summary>
    public void CheckWaterItem()
    {
        for (int i = 0; i < waterItems.Count; i++)
        {
            if (!waterImg[i].isPlayItemAnim)
            {
                waterImg[i].fireRuneGo.SetActive(false);
                waterImg[i].iceGo.SetActive(false);
            }
            switch (waterItems[i])
            {
                case WaterItem.None:
                    waterImg[i].textItem.text = "";
                    break;
                case WaterItem.Ice:
                    waterImg[i].iceGo.SetActive(true);
                    break;
                case WaterItem.Bomb:
                    waterImg[i].textItem.text = "BOMB";
                    break;
                case WaterItem.BreakIce:
                    waterImg[i].textItem.text = "";
                    waterImg[i].fireRuneGo.SetActive(true);
                    switch (waters[i])
                    {
                        case 1:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_cl", false);
                            break;
                        case 2:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_dh", false);
                            break;
                        case 3:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_fh", false);
                            break;
                        case 4:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_gl", false);
                            break;
                        case 5:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_hl", false);
                            break;
                        case 6:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_hs", false);
                            break;
                        case 7:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_jh", false);
                            break;
                        case 8:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_lh", false);
                            break;
                        case 9:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_sl", false);
                            break;
                        case 10:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_ze", false);
                            break;
                        case 11:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_zs", false);
                            break;
                        case 12:
                            waterImg[i].fireRuneSpine.AnimationState.SetAnimation(0, "idle_mh", false);
                            break;
                    }
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
        if (BottleHasItem())
            UIKit.OpenPanel<UIMask>(UILevel.PopUI);

        isPlayAnim = true;
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
            if (startIdx >= 0)
            {
                SetNowSpinePos(startIdx);
            }
            //spineGo.transform.position = spineNode[topIdx + 1].position;
        }
        PlaySpineAnim();

        float fillTime = fillAlltime / num;
        if (color > 1000)
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

        isPlayAnim = false;
        
        CheckItem();

        CheckFill();
    }

    /// <summary>
    /// 判断是否有水块
    /// </summary>
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

    /// <summary>
    /// 播倒水动画
    /// </summary>
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

        if (color < 1000 && color != 0)
        {
            spine.AnimationState.SetAnimation(0, spineAnimName, false);
        }
    }

    /// <summary>
    /// 入场动画
    /// </summary>
    public void PlaySpineWaitAnim(int useColor = -1)
    {
        string spineAnimName = "";
        //var color = GetMoveOutTop();
        int spinePosIdx = topIdx;

        if (topIdx >= 0)
        {
            for (int i = spinePosIdx; i >= 0; i--)
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

        CheckHide();

    }

    /// <summary>
    /// 设置水面位置
    /// </summary>
    public void SetNowSpinePos(int node)
    {
        var useNode = node;
        //Debug.Log("当前节点 " + node);
        if (useNode - 1 < waters.Count)
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
        //Debug.Log("移动终点  " + useIdx + " " + num);
        if (useColor > 1000)
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
        if (useColor > 1000)
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

        //倒水过程结束
        GameCtrl.Instance.ReducePouringCount();

        SetBottleColor();
        PlaySpineWaitAnim();

    }

    public void MoveToOtherAnim(BottleCtrl other, int useColor = -1)
    {
        isPlayAnim = true;

        if (useColor < 1000)
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
                        isPlayAnim = false;
                    });
                });
            }
            else
            {
                modelGo.transform.DOMove(other.leftMovePlace.position, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    modelGo.transform.DOLocalMove(Vector3.zero, 0.46f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        isPlayAnim = false;
                    });
                });
            }

        });
    }

    public void PlayWaterDown()
    {
        fillWaterGoAnim.Play("FillWater");
    }

    /// <summary>
    /// 设置水柱颜色
    /// </summary>
    /// <param name="useColor"></param>
    public void SetDownWaterSp(int useColor = -1)
    {
        var color = GetMoveOutTop();
        if (useColor != -1)
        {
            color = useColor;
        }

        if (color < 1000)
        {
            ImgWaterTop.sprite = LevelManager.Instance.waterTopSp[color - 1];
            ImgWaterDown.sprite = LevelManager.Instance.waterSp[color - 1];
        }
    }

    /// <summary>
    /// 检查本次倒水瓶子中是否有局内道具生效
    /// </summary>
    /// <returns></returns>
    public bool BottleHasItem()
    {
        /*// 仅作用魔法阵
        int itemId = 0;
        int itemPlace = 0;

        for (int i = 0; i < waters.Count; i++)
        {
            int waterColor = waters[i];

            if (waterColor == (int)ItemType.MagnetItem)
            {
                if (itemId == 0)
                {
                    itemId = waterColor;
                    itemPlace = i;
                }
                else if (itemId == waterColor && i - itemPlace == 1)
                {
                    return true;
                }
            }
        }

        return false;*/

        //作用所有道具
        int itemId = 0;
        int itemPlace = 0;

        for (int i = 0; i < waters.Count; i++)
        {
            int waterColor = waters[i];

            if (waterColor > 1000)
            {
                if (itemId == 0)
                {
                    itemPlace = i;
                    itemId = waterColor;
                }
                else if (waterColor == itemId && i - itemPlace == 1)
                {
                    // 判断是否是符合条件的特殊道具
                    bool isMatch =
                        waterColor == (int)ItemType.ClearItem ||
                        waterColor == (int)ItemType.MakeColorItem ||
                        waterColor == (int)ItemType.MagnetItem ||
                        (waterColor > 2000 && waterColor < 3000) ||
                        waterColor > 3000;

                    if (isMatch)
                        return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 检查道具
    /// </summary>
    public void CheckItem()
    {
        int itemId = 0;
        int itemPlace = 0;
        List<int> items = new List<int>();


        for (int i = 0; i < waters.Count; i++)
        {
            var waterColor = waters[i];

            if (waterColor > 1000)
            {
                if (itemId == 0)
                {
                    itemPlace = i;
                    itemId = waterColor;
                }
                else
                {
                    if (waters[i] == itemId)
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
                                    waterImg[i].color = new Color(1, 1, 1, 0);
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
                                if (waters[i] > 2000 && waters[i] < 3000)
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

        for (int i = 0; i < waters.Count; i++)
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

        if (items.Count > 0)
        {
            CheckHide();
            SetHideShow(false);
            SetNowSpinePos(topIdx + 1);
        }
    }

    /// <summary>
    /// 换色道具动画
    /// </summary>
    /// <param name="color"></param>
    /// <param name="fromPos"></param>
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

        foreach (var ctrl in list)
        {
            var go = Instantiate(LevelManager.Instance.broomBullet);
            
            var fly = go.GetComponent<FlyCtrl>();
            fly.target = ctrl.transform;
            fly.flyTime = 1.2f;
            go.transform.position = fromPos;
            fly.BeginFly();
        }

        //由BottleWaterCtrl的协程调用(避免中断协程导致Destroy不生效)
        //SetBottleColor();
    }

    /// <summary>
    /// 移除单色
    /// </summary>
    /// <param name="color"></param>
    public void RemoveAllOneColor(int color)
    {
        List<int> list = new List<int>();
        List<WaterItem> items = new List<WaterItem>();
        List<bool> hides = new List<bool>();
        for (int i = 0; i < waters.Count; i++)
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

    /// <summary>
    /// 移除时动画特效
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    IEnumerator PlayShine(int i)
    {
        isPlayAnim = true;
        var imgcmp = waterImg[i].transform.GetComponent<Image>();
        imgcmp.material = LevelManager.Instance.shineMaterial;
        StartCoroutine(waterImg[i].ShowBroomAfter());


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
        isPlayAnim = false;
    }

    /// <summary>
    /// 记录上一步
    /// </summary>
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

    /// <summary>
    /// 返回上一步
    /// </summary>
    /// <returns></returns>
    public bool ReturnLast()
    {
        if (moveRecords.Count <= 0)
        {
            return false;
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
        return true;
    }

    /// <summary>
    /// 设置瓶子最大装水数
    /// </summary>
    public void SetMaxBottle()
    {
        bottleOne.SetActive(maxNum == 1);
        bottleTwo.SetActive(maxNum == 2);
        bottleThree.SetActive(maxNum == 3);
        bottleFour.SetActive(maxNum == 4);
        ImgBottleOne.gameObject.SetActive(maxNum == 1);
        ImgBottleTwo.gameObject.SetActive(maxNum == 2);
        ImgBottleThree.gameObject.SetActive(maxNum == 3);
        ImgBottleFour.gameObject.SetActive(maxNum == 4);
    }

    /// <summary>
    /// 打乱水块顺序
    /// </summary>
    public void RandomWater()
    {
        var numbers = Enumerable.Range(1, waters.Count).ToList();

        int n = numbers.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            int value = numbers[k];
            numbers[k] = numbers[n];
            numbers[n] = value;
        }

        bool checkSame = true;
        if (numbers[0] == 1)
        {
            for (int i = 0; i < waters.Count - 1; i++)
            {
                if (waters[i] != waters[numbers[i] - 1])
                {
                    if (waters[i] + 1 != waters[i + 1])
                    {
                        checkSame = false;
                        break;
                    }
                }
            }
        }
        else
        {
            checkSame = false;
        }


        if (checkSame)
        {
            RandomWater();
        }
        else
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < waters.Count; i++)
            {
                temp.Add(waters[numbers[i] - 1]);
            }


            waters = temp;

            SetBottleColor();
        }
    }

    /// <summary>
    /// 道具显示材质切换
    /// </summary>
    public void ShowItemSelect()
    {
        ImgBottleOne.material = LevelManager.Instance.selectMaterial;
        ImgBottleTwo.material = LevelManager.Instance.selectMaterial;
        ImgBottleFour.material = LevelManager.Instance.selectMaterial;
        ImgBottleFour.material = LevelManager.Instance.selectMaterial;
    }

    /// <summary>
    /// 道具隐藏材质切换
    /// </summary>
    public void HideItemSelect()
    {
        ImgBottleOne.material = null;
        ImgBottleTwo.material = null;
        ImgBottleFour.material = null;
        ImgBottleFour.material = null;
    }
}
