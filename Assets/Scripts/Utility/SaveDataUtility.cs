using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;
using static UnityEngine.UI.Image;
using System.Reflection;
using System.ComponentModel;

public class SaveDataUtility : IUtility, ICanSendEvent
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    public void SaveLevel(int level)
    {
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        PlayerPrefs.SetInt("g_ClearWaterLevel", level);
    }

    /// <summary>
    /// 当前关卡/已获得的总星星数
    /// </summary>
    /// <returns></returns>
    public int GetLevelClear()
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterLevel", 1);
       return clearLevel;
    }

    public void SaveChallenge(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterChallenge", 0);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearWaterChallenge", clearLevel);

    }

    public bool GetChallengeClear(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterChallenge", 0);
        int checkLevel = (int)Mathf.Pow(2, level - 1);
        int isClear = clearLevel & checkLevel;
        if (isClear == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void SaveOrder(int level)
    {
        PlayerPrefs.SetInt("g_ClearWaterOrder", level);

    }

    public int GetOrder()
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterOrder", 1);

        return clearLevel;
    }
    public void SaveJigsaw(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterJigsaw", 0);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearWaterJigsaw", clearLevel);
    }

    public bool GetJigsaw(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearWaterJigsaw", 0);
        int checkLevel = (int)Mathf.Pow(2, level - 1);
        int isClear = clearLevel & checkLevel;
        if (isClear == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GetSelectLanguage()
    {
        string language =  PlayerPrefs.GetString("g_WaterLanguage", "-1");

        return language;
    }

    public void SaveShowOrderBegin()
    {
        PlayerPrefs.SetInt("g_WaterOrderBegin", 1);
    }

    public bool GetShowOrderBegin()
    {
        return PlayerPrefs.GetInt("g_WaterOrderBegin", 0) == 1;
    }
    public void SaveShowOrderEnd()
    {
        PlayerPrefs.SetInt("g_WaterOrderEnd", 1);
    }

    public bool GetShowOrderEnd()
    {
        return PlayerPrefs.GetInt("g_WaterOrderEnd", 0) == 1;
    }

    public void SaveSelectLanguage(LanguageType languageType)
    {
        string language = languageType.ToString();
     
        PlayerPrefs.SetString("g_WaterLanguage", language);
    }

    public void SaveSelectLanguage(string language)
    {
        PlayerPrefs.SetString("g_WaterLanguage", language);
    }

    public void SaveNoAD(bool NoAD)
    {
        if(NoAD)
        {
            PlayerPrefs.SetInt("g_WaterNoAD", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_WaterNoAD", 0);
        }
    }

    public bool GetNoAD()
    {
        if(PlayerPrefs.GetInt("g_WaterNoAD", 0) == 1)
        {
            return true;
        }

        return false;
    }

    public void SaveClickChallenge(bool click)
    {
        if(click)
        {
            PlayerPrefs.SetInt("g_WaterClickChallenge", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_WaterClickChallenge", 0);
        }
    }

    public bool GetClickChallenge()
    {
        if(PlayerPrefs.GetInt("g_WaterClickChallenge", 0) == 1)
        {
            return true;
        }

        return false;
    }
    public void SaveSeven(bool click)
    {
        if(click)
        {
            PlayerPrefs.SetInt("g_WaterSeven", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_WaterSeven", 0);
        }
    }

    public bool GetSeven()
    {
        if(PlayerPrefs.GetInt("g_WaterSeven", 0) == 1)
        {
            return true;
        }

        return false;
    }

    //public void SaveStarLevel(int level, int star)
    //{
    //    if (level < 7 || level > 30 || level == 19 || level == 28 || level == 14)
    //    {
    //        return;
    //    }
    //    else if(level > 14 && level < 19)
    //    {
    //        level--;
    //    }
    //    else if(level > 19 && level < 28)
    //    {
    //        level -= 2;
    //    }
    //    else if (level > 28 && level <= 30)
    //    {
    //        level -= 3;
    //    }
    //    string lastSave = PlayerPrefs.GetString("g_WaterStarLevel", "000000000000000000000");
    //    char[] chars = lastSave.ToCharArray();
    //    char[] num = (star + "").ToCharArray();
    //    chars[level - 7] = num[0];
    //    PlayerPrefs.SetString("g_WaterStarLevel", new string(chars));
    //    Debug.Log("通关星级 第" + level + "关 : " + star + "星  " + num[0]);
    //}

    //public int GetStarLevel(int level)
    //{
    //    if (level < 7 || level > 30 || level == 19 || level == 28 || level == 14)
    //    {
    //        return 0;
    //    }
    //    else if (level > 14 && level < 19)
    //    {
    //        level--;
    //    }
    //    else if (level > 19 && level < 28)
    //    {
    //        level -= 2;
    //    }
    //    else if (level > 28 && level <= 30)
    //    {
    //        level -= 3;
    //    }
    //    string lastSave = PlayerPrefs.GetString("g_WaterStarLevel", "000000000000000000000");
    //    char[] chars = lastSave.ToCharArray();
    //    return int.Parse(chars[level - 7] + "");
    //}

    public void SetRankTime()
    {
        DateTime now = DateTime.UtcNow + TimeSpan.FromSeconds(259201);
        PlayerPrefs.SetString("g_WaterRankTimeEnd", now.ToString());
    }

    public string GetLeftTime()
    {
        DateTime now = DateTime.Now;
        //long unixTimestamp = (now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        string endTime = PlayerPrefs.GetString("g_WaterRankTimeEnd", (DateTime.UtcNow + TimeSpan.FromSeconds(259201)).ToString());
        DateTime end = DateTime.Parse(endTime);
        var leftSt = end - DateTime.UtcNow;
        DateTime left = new DateTime();
        if (leftSt.TotalSeconds > 0)
        {
            left += leftSt;
        }
        //left.AddSeconds(endTimeSecond);
        return (left.Day - 1) + "D " + left.ToLongTimeString();
    }

    public int GetOrAddMoreStar(int star = 0)
    {
        var total = PlayerPrefs.GetInt("g_WaterMoreStar", 0);
        total += star;
        PlayerPrefs.SetInt("g_WaterMoreStar", total);
        return total;
    }

    public bool SetOrGetIsTipMain(int isTip = -1)
    {
        if(isTip != -1)
        {
            PlayerPrefs.SetInt("g_WaterIsTipMain", isTip);
        }
        return PlayerPrefs.GetInt("g_WaterIsTipMain", 0) == 1;
    }

    //public int GetNowStar()
    //{
    //    return PlayerPrefs.GetInt("g_WaterIsTipMain", 0);

    //}

    ////////////////////////////体力相关///////////////////////////////////////

    public void SetVitality(int num)
    {
        SetVitalityTime();
        SetVitalityNum(num);
    }

    public void CostVitality()
    {
        int lastVitalityNum = GetVitalityNum();
        SetVitality(lastVitalityNum - 1);
    }

    public void SetVitality(int num, string time)
    {
        SetVitalityTime(time);
        SetVitalityNum(num);
    }

    public long GetNowTime()
    {
        DateTime now = DateTime.Now;
        long unixTimestamp = now.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        long time = (unixTimestamp / 10000000);

        return time;
    }

    public void UseVitality()
    {
        SetVitality(GetVitalityNum() - 1);
    }

    /// <summary>
    /// 设置上一次体力恢复的时间
    /// </summary>
    /// <param name="time"></param>
    public void SetVitalityTime(string time)
    {
        PlayerPrefs.SetString("g_WaterVitalityTime", time);
    }

   
    public void SetVitalityTime()
    {
        PlayerPrefs.SetString("g_WaterVitalityTime", GetNowTime() + "");
    }

    /// <summary>
    /// 获取上一次体力恢复的时间
    /// </summary>
    public long GetVitalityTime()
    {
        string timeStr = PlayerPrefs.GetString("g_WaterVitalityTime", "0");
        long time = long.Parse(timeStr);
        return time;
    }

    public void SetVitalityNum(int num)
    {
        PlayerPrefs.SetInt("g_WaterVitalityNum", num);
        this.SendEvent<VitalityChangeEvent>();
    }

    public void AddVitalityNum(int num)
    {
        var v = PlayerPrefs.GetInt("g_WaterVitalityNum", GameConst.MaxVitality);
        v += num;
        if (v > GameConst.MaxVitality)
        {
            v = GameConst.MaxVitality;
        }
        PlayerPrefs.SetInt("g_WaterVitalityNum", v);
    }


    /// <summary>
    /// 获取体力
    /// </summary>
    /// <returns></returns>
    public int GetVitalityNum()
    {
        return PlayerPrefs.GetInt("g_WaterVitalityNum", GameConst.MaxVitality);
    }


    ////////////////////////////金币相关//////////////////////////////////////
    ///
    public void SetCoinNum(int num)
    {
        PlayerPrefs.SetInt("g_WaterCoinNum", num);
        this.SendEvent<CoinChangeEvent>();
    }

    public int GetCoinNum()
    {
        return PlayerPrefs.GetInt("g_WaterCoinNum", 0);
    }

    public void SetSceneRecord(int scene)
    {
       PlayerPrefs.SetInt("g_WaterSceneRecord", scene);
    }

    /// <summary>
    /// 当前玩家所在的场景编号
    /// </summary>
    /// <returns></returns>
    public int GetSceneRecord()
    {
        return PlayerPrefs.GetInt("g_WaterSceneRecord", 1);
    }

    public void SetScenePartRecord(int scene)
    {
       PlayerPrefs.SetInt("g_WaterScenePartRecord", scene);
    }

    /// <summary>
    /// 当前场景中所解锁的建筑编号
    /// </summary>
    /// <returns></returns>
    public int GetScenePartRecord()
    {
        return PlayerPrefs.GetInt("g_WaterScenePartRecord", 0); 
    }

    /// <summary>
    /// 设置宝箱编号(用于确定哪个场景的宝箱)
    /// </summary>
    /// <param name="scene"></param>
    public void SetSceneBox(int scene)
    {
        PlayerPrefs.SetInt("g_WaterSceneBoxRecord", scene);
    }
    /// <summary>
    /// 获取宝箱编号
    /// </summary>
    /// <returns></returns>
    public int GetSceneBox()
    {
        return PlayerPrefs.GetInt("g_WaterSceneBoxRecord", 0);  
    }

    public void AddItemNum(int item, int num = 1)
    {
        var itemNum = GetItemNum(item);
        SetItemNum(item, itemNum + num);
    }

    public void ReduceItemNum(int item, int num = 1)
    {
        var itemNum = GetItemNum(item);
        SetItemNum(item, itemNum - num);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item">1 回退 2 取消黑色 3 加瓶子 4 加一格瓶子 5 取消所有限制 6 加一格瓶子 7取消两根黑色 8随机颜色</param>
    /// <returns></returns>
    public void SetItemNum(int item, int num)
    {
        PlayerPrefs.SetInt("g_WaterSceneItem" + item, num);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"> 1 回退 2 取消黑色 3 加瓶子 4 加一格瓶子 5 取消所有限制 6 加一格瓶子 7取消两根黑色 8随机颜色</param>
    /// <returns></returns>
    public int GetItemNum(int item)
    {
        return PlayerPrefs.GetInt("g_WaterSceneItem" + item, 0);
    }

    /// <summary>
    /// 连胜次数
    /// </summary>
    /// <returns></returns>
    public void SetCountinueWinNum(int num)
    {
        PlayerPrefs.SetInt("g_WaterCountinueWinNum", num);
    }

    public int GetCountinueWinNum()
    {
        return PlayerPrefs.GetInt("g_WaterCountinueWinNum", 0);
    }
}
