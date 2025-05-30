using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using GameDefine;

public class StageModel : AbstractModel
{
    public BindableDictionary<int, int> ItemDic;

    public int CountinueWinNum => mCountinueWinNum.Value;
    private BindableProperty<int> mCountinueWinNum;

    private const string ITEM_SIGN = "g_WaterSceneItem";
    private const string COUNTINUE_WIN_NUM_SIGN = "g_WaterCountinueWinNum";

    protected override void OnInit()
    {
        var stroge = this.GetUtility<SaveDataUtility>();

        ItemDic = new BindableDictionary<int, int>();
        mCountinueWinNum = new BindableProperty<int>();

        for (int i = 1; i <= GameDefine.GameConst.ITEM_COUNT; i++)
        {
            var key = $"{ITEM_SIGN}{i}";
            ItemDic[i] = stroge.LoadIntValue(key, 0);
        }
        ItemDic.OnReplace.Register((itemID, oldValue, newValue) =>
        {
            stroge.SaveInt($"{ITEM_SIGN}{itemID}", newValue);
            this.SendEvent<RefreshItemEvent>();
            //Debug.Log($"道具ID：{itemID} 数量更新为:{newValue},发送事件通知...");
        });

        mCountinueWinNum.SetValueWithoutEvent(stroge.LoadIntValue(COUNTINUE_WIN_NUM_SIGN));
        mCountinueWinNum.Register(value =>
        {
            stroge.SaveInt(COUNTINUE_WIN_NUM_SIGN, value);

        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="addNum"></param>
    /// 1 回退 2 取消黑色 3 加瓶子 4 加一格瓶子 5 取消所有限制 6 加一格瓶子 7取消两根黑色 8随机颜色</param>
    public void AddItem(int itemID, int addNum)
    {
        if (ItemDic.ContainsKey(itemID))
        {
            ItemDic[itemID] += addNum;
        }
        else
        {
            ItemDic[itemID] = addNum;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemID">Item ID</param>
    /// <param name="reduceNum">Reduce Item Num</param>
    public void ReduceItem(int itemID, int reduceNum)
    {
        if (ItemDic.ContainsKey(itemID))
        {
            ItemDic[itemID] = Mathf.Max(0, ItemDic[itemID] - reduceNum);
        }
    }

    public void AddCountinueWinNum()
    {
        int tempNum = mCountinueWinNum.Value;
        tempNum++;
        //mCountinueWinNum.Value = tempNum < GameConst.MAX_COUNTINUE_WIN_NUM ? tempNum : GameConst.MAX_COUNTINUE_WIN_NUM;
    }

    public void ResetCountinueWinNum()
    {
        mCountinueWinNum.Value = 0;
    }
}
