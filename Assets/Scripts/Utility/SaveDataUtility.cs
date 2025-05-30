using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;

public class SaveDataUtility : IUtility, ICanSendEvent
{
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    public void SaveInt(string key,int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int LoadIntValue(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
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


    /// <summary>
    /// 标志所有建筑解锁完成
    /// </summary>
    /// <param name="sign"></param>
    public void SetOverUnLock(bool sign)
    {
        PlayerPrefs.SetString("g_OverUnLock", sign.ToString());
    }

    public string GetOverUnLock()
    {
       return PlayerPrefs.GetString("g_OverUnLock", "false");
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
        int scene = PlayerPrefs.GetInt("g_WaterSceneRecord", 1);
        return scene > 4 ? 4 : scene;
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
}
