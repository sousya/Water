using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using GameDefine;

public class CoinManager : MonoBehaviour, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    static public CoinManager Instance;

    public int coin;
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
        return true;
    }

    public void BuyVitality()
    {
        if(CostCoin(900))
        {
            this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
        }
    }
}

