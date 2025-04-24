using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;
using Unity.Services.Core;
using GameDefine;

public class CoinManager : MonoSingleton<CoinManager>, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    public int coin;

    public override void OnSingletonInit()
    {

    }


    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    void Start()
    {
        coin = this.GetUtility<SaveDataUtility>().GetCoinNum();
    }

    public bool CostCoin(int cost)
    {
        if(cost > coin)
        {
            return false;
        }

        coin -= cost;
        //缺少保存金币的逻辑
        return true;
    }

    /// <summary>
    /// 购买体力
    /// </summary>
    /// 900金币补满体力
    public void BuyVitality()
    {
        if(CostCoin(900))
        {
            this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
        }
    }
}

