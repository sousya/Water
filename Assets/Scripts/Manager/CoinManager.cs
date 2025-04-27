using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameDefine;

public class CoinManager : MonoSingleton<CoinManager>, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    private int coin;
    public int Coin => coin;

    private SaveDataUtility saveData;

    public override void OnSingletonInit()
    {

    }

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    #region 金币数据存储
    private void SetCoinNum(int num)
    {
        PlayerPrefs.SetInt("g_WaterCoinNum", num);
        this.SendEvent<CoinChangeEvent>();
    }

    private int GetCoinNum()
    {
        return PlayerPrefs.GetInt("g_WaterCoinNum", 0);
    }
    #endregion

    private void Awake()
    {
        saveData = this.GetUtility<SaveDataUtility>();
        coin = saveData.GetCoinNum();
    }

    /// <summary>
    /// 使用金币
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    public void CostCoin(int cost)
    {
        coin -= cost;
        saveData.SetCoinNum(coin);
    }

    /// <summary>
    /// 购买体力
    /// </summary>
    /// 900金币补满体力
    public void BuyVitality(int cost)
    {
        if(cost <= coin && HealthManager.Instance.UsedHp > 0)
        {
            //this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
            CostCoin(cost);
            HealthManager.Instance.SetNowHpToMax();
        }
    }
}

