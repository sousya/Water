using System.Collections.Generic;
using UnityEngine;

namespace WaterGame.Models
{
    [System.Serializable]
    public class LevelData
    {
        public int LevelIndex;
        public string LevelName;
        public List<BottleData> Bottles = new List<BottleData>();
        public int MaxMoves;
        public bool IsTutorial;
    }

    [System.Serializable]
    public class BottleData
    {
        public int MaxCapacity = 4;
        public List<Water> Waters = new List<Water>();
        public bool IsFrozen;
        public bool IsHidden;
        public bool IsClearHidden;
        public bool IsNearHidden;
        public int LimitColor;
        public int UnlockClear;
    }

    [System.Serializable]
    public class Water
    {
        public int Color;
        public WaterItem Item;
        public bool IsHidden;

        public Water(int color, WaterItem item = WaterItem.None, bool isHidden = false)
        {
            Color = color;
            Item = item;
            IsHidden = isHidden;
        }
    }

    public enum WaterItem
    {
        None,
        Ice,
        Bomb,
        BreakIce
    }
} 