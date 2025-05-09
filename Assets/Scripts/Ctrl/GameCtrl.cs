using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCtrl : MonoBehaviour, ICanSendEvent
{
    public static GameCtrl Instance;
    public BottleCtrl FirstBottle, SecondBottle;

    public bool control = false;

    //选中道具标志及道具方法
    [SerializeField] private bool isSelectedItem = false;    
    private Action<BottleCtrl> RandomItemAction;

    //倒水计数，处于0表示当前不处于倒水过程
    private int pouringCount = 0;
    public bool IsPouring => pouringCount == 0;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    /// <summary>
    /// 选中瓶子
    /// </summary>
    /// <param name="bottle"></param>
    public void OnSelect(BottleCtrl bottle)
    {
        if (isSelectedItem)
        {
            //能被选中、水块大于1且不同色
            if (bottle.OnSelect(false)
            && bottle.waters.Count > 1 && !bottle.waters.All(x => x == bottle.waters[0]))
            {
                //Debug.Log("可选中");
                RandomItemAction?.Invoke(bottle);
            }
            else
            {
                //Debug.Log("不可选中");
                LevelManager.Instance.HideItemSelect();
            }
            isSelectedItem = false;
            RandomItemAction = null;
        }
        else
        {
            if (!control)
            {

                if (FirstBottle == null)
                {
                    if (bottle.OnSelect(true))
                    {
                        FirstBottle = bottle;
                    }

                }
                else if (SecondBottle == null)
                {

                    if (bottle != FirstBottle && bottle.OnSelect(false))
                    {
                        SecondBottle = bottle;
                    }
                    else
                    {
                        FirstBottle.OnCancelSelect();
                        FirstBottle = null;
                    }
                }

                if (FirstBottle != null && SecondBottle != null)
                {
                    control = true;
                    if (FirstBottle.CheckMoveOut() && SecondBottle.CheckMoveIn(FirstBottle.GetMoveOutTop())
                        && !FirstBottle.isPlayAnim && !SecondBottle.isPlayAnim)
                    {
                        //Debug.Log("移动 " + FirstCake.gameObject.name + "->" + SecondCake.gameObject.name);
                        LevelManager.Instance.RecordLast();
                        ++pouringCount;
                        FirstBottle.MoveTo(SecondBottle);
                        FirstBottle = null;
                        SecondBottle = null;
                        //LevelManager.Instance.AddMoveNum();//步数统计.暂时无用
                        //this.SendEvent<MoveCakeEvent>();//未使用
                    }
                    else
                    {
                        control = false;
                        FirstBottle.OnCancelSelect();
                        FirstBottle = null;
                        SecondBottle = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 选中道具状态
    /// </summary>
    /// <param name="action"></param>
    public void SeletedItem(Action<BottleCtrl> action)
    {
        isSelectedItem = true;
        RandomItemAction = action;  
    }

    /// <summary>
    /// 重置状态
    /// </summary>
    public void InitGameCtrl()
    {
        FirstBottle = null;
        SecondBottle = null;
        control = false;
    }

    /// <summary>
    /// 倒水状态完成
    /// </summary>
    public void ReducePouringCount()
    {
        --pouringCount;
        if (pouringCount < 0)
            pouringCount = 0;
    }

    /// <summary>
    /// 重置倒水状态
    /// </summary>
    public void InitPouringCount()
    {
        pouringCount = 0;
    }
}
