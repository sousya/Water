using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;
using static UnityEngine.UI.Image;
using System.Reflection;

public class SaveDataUtility : IUtility
{
    public void SaveLevel(int level)
    {
        //Debug.Log("LevelBefore " + Convert.ToString(clearLevel, 2) + " clearNowLevel " + clearNowLevel + " LevelNow " + Convert.ToString((clearLevel | clearNowLevel), 2));
        PlayerPrefs.SetInt("g_ClearCakeLevel", level);

    }
    public int GetLevelClear()
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeLevel", 1);
       return clearLevel;
    }
    public void SaveChallenge(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeChallenge", 0);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearCakeChallenge", clearLevel);

    }

    public bool GetChallengeClear(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeChallenge", 0);
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
        PlayerPrefs.SetInt("g_ClearCakeOrder", level);

    }

    public int GetOrder()
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeOrder", 1);

        return clearLevel;
    }
    public void SaveJigsaw(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeJigsaw", 0);
        int clearNowLevel = (int)Mathf.Pow(2, level - 1);
        clearLevel = clearLevel | clearNowLevel;
        PlayerPrefs.SetInt("g_ClearCakeJigsaw", clearLevel);
    }

    public bool GetJigsaw(int level)
    {
        int clearLevel = PlayerPrefs.GetInt("g_ClearCakeJigsaw", 0);
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
        string language =  PlayerPrefs.GetString("g_CakeLanguage", "-1");

        return language;
    }

    public void SaveShowOrderBegin()
    {
        PlayerPrefs.SetInt("g_CakeOrderBegin", 1);
    }

    public bool GetShowOrderBegin()
    {
        return PlayerPrefs.GetInt("g_CakeOrderBegin", 0) == 1;
    }
    public void SaveShowOrderEnd()
    {
        PlayerPrefs.SetInt("g_CakeOrderEnd", 1);
    }

    public bool GetShowOrderEnd()
    {
        return PlayerPrefs.GetInt("g_CakeOrderEnd", 0) == 1;
    }

    public void SaveSelectLanguage(LanguageType languageType)
    {
        string language = languageType.ToString();
     
        PlayerPrefs.SetString("g_CakeLanguage", language);
    }

    public void SaveSelectLanguage(string language)
    {
        PlayerPrefs.SetString("g_CakeLanguage", language);
    }

    public void SaveNoAD(bool NoAD)
    {
        if(NoAD)
        {
            PlayerPrefs.SetInt("g_CakeNoAD", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_CakeNoAD", 0);
        }
    }

    public bool GetNoAD()
    {
        if(PlayerPrefs.GetInt("g_CakeNoAD", 0) == 1)
        {
            return true;
        }

        return false;
    }

    public void SaveClickChallenge(bool click)
    {
        if(click)
        {
            PlayerPrefs.SetInt("g_CakeClickChallenge", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_CakeClickChallenge", 0);
        }
    }

    public bool GetClickChallenge()
    {
        if(PlayerPrefs.GetInt("g_CakeClickChallenge", 0) == 1)
        {
            return true;
        }

        return false;
    }
    public void SaveSeven(bool click)
    {
        if(click)
        {
            PlayerPrefs.SetInt("g_CakeSeven", 1);
        }
        else
        {
            PlayerPrefs.SetInt("g_CakeSeven", 0);
        }
    }

    public bool GetSeven()
    {
        if(PlayerPrefs.GetInt("g_CakeSeven", 0) == 1)
        {
            return true;
        }

        return false;
    }

    public void SaveStarLevel(int level, int star)
    {
        if (level < 7 || level > 30 || level == 19 || level == 28 || level == 14)
        {
            return;
        }
        else if(level > 14 && level < 19)
        {
            level--;
        }
        else if(level > 19 && level < 28)
        {
            level -= 2;
        }
        else if (level > 28 && level <= 30)
        {
            level -= 3;
        }
        string lastSave = PlayerPrefs.GetString("g_CakeStarLevel", "000000000000000000000");
        char[] chars = lastSave.ToCharArray();
        char[] num = (star + "").ToCharArray();
        chars[level - 7] = num[0];
        PlayerPrefs.SetString("g_CakeStarLevel", new string(chars));
        Debug.Log("通关星级 第" + level + "关 : " + star + "星  " + num[0]);
    }

    public int GetStarLevel(int level)
    {
        if (level < 7 || level > 30 || level == 19 || level == 28 || level == 14)
        {
            return 0;
        }
        else if (level > 14 && level < 19)
        {
            level--;
        }
        else if (level > 19 && level < 28)
        {
            level -= 2;
        }
        else if (level > 28 && level <= 30)
        {
            level -= 3;
        }
        string lastSave = PlayerPrefs.GetString("g_CakeStarLevel", "000000000000000000000");
        char[] chars = lastSave.ToCharArray();
        return int.Parse(chars[level - 7] + "");
    }

    public void SetRankTime()
    {
        DateTime now = DateTime.UtcNow + TimeSpan.FromSeconds(259201);
        PlayerPrefs.SetString("g_CakeRankTimeEnd", now.ToString());
    }

    public string GetLeftTime()
    {
        DateTime now = DateTime.Now;
        //long unixTimestamp = (now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        string endTime = PlayerPrefs.GetString("g_CakeRankTimeEnd", (DateTime.UtcNow + TimeSpan.FromSeconds(259201)).ToString());
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
        var total = PlayerPrefs.GetInt("g_CakeMoreStar", 0);
        total += star;
        PlayerPrefs.SetInt("g_CakeMoreStar", total);
        return total;
    }
}
