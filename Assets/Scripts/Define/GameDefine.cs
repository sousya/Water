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
        ClearItem = 1001,
        MagnetItem = 1002, 
        MakeColorItem = 1003,
        ChangeGreen = 2001,
        ChangeOrange = 2002,
        ChangePink = 2003,
        ChangePurple = 2004,
        ChangeYellow = 2005,
        ChangeDarkBlue = 2006,
        ClearPink = 3001,
        ClearOrange = 3002,
        ClearBlue = 3003,
        ClearYellow = 3004,
        ClearDarkGreen = 3005,
        ClearRed = 3006,
        ClearGreen = 3007,
    }
    public enum LanguageType
    {
        zh = 0,
        ja = 1,
        en = 2,
        ko = 3,
    }
}
