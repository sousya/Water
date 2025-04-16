using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

namespace GameDefine
{
    public static class GameConst
    {
        public const int MaxVitality = 5;
        public const int RecoveryTime = 1800;
    }
    public enum GameType
    {
        Normal = 0,
        Bomb = 1,
        Count = 2,
        Step = 3,
        Hide = 4,
    }

    public enum WaterItem
    {
        None = 0,
        Ice = 1,
        BreakIce = 2,
        Bomb = 3,
    }

    public enum BottleType
    {
        None = 0,
        ClearShow = 1,
        NearShow = 2
    }

    public enum ItemType
    {
        ClearItem = 1001,       // 测试不出效果
        MagnetItem = 1002,      // 魔法书，清楚所有Debuff(障碍，冰冻效果)
        MakeColorItem = 1003,   // 添加隐藏的颜色到瓶子中
        ChangeGreen = 2001,     // 将某种颜色变为绿色-编号1
        ChangeOrange = 2002,    // 将某种颜色变为橙色-编号7
        ChangePink = 2003,      // 将某种颜色变为粉色-编号3
        ChangePurple = 2004,    // 将某种颜色变为紫色-编号10
        ChangeYellow = 2005,    // 将某种颜色变为黄色-编号6
        ChangeDarkBlue = 2006,  // 将某种颜色变为深蓝色-编号4
        ClearPink = 3001,       // 清除所有粉色水块-编号3
        ClearOrange = 3002,     // 清除所有橙色水块-编号7
        ClearBlue = 3003,       // 清除所有蓝色水块-编号4
        ClearYellow = 3004,     // 清除所有黄色水块-编号6
        ClearDarkGreen = 3005,  // 清除所有深绿色水-块编号9
        ClearRed = 3006,        // 清除所有红色水块-编号2
        ClearGreen = 3007,      // 清除所有绿色水块-编号1
    }
    public enum LanguageType
    {
        zh = 0,
        ja = 1,
        en = 2,
        ko = 3,
    }
}
