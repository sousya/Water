using System.Collections.Generic;
using UnityEngine;

namespace WaterGame.Models
{
    public class Water
    {
        public int Color { get; set; }
        public WaterItem Item { get; set; }
        public bool IsHidden { get; set; }
    }

    public class BottleState
    {
        public List<Water> Waters { get; set; } = new List<Water>();
        public bool IsSelected { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsHidden { get; set; }
        public bool IsClearHidden { get; set; }
        public bool IsNearHidden { get; set; }
        public bool IsFinished { get; set; }
        public int MaxCapacity { get; set; } = 4;
        public int LimitColor { get; set; }
        public int UnlockClear { get; set; }
        public int BottleIndex { get; set; }
        
        public int TopIndex => Waters.Count - 1;
        public bool IsEmpty => Waters.Count == 0;
        public bool IsFull => Waters.Count >= MaxCapacity;
        public Water TopWater => IsEmpty ? null : Waters[TopIndex];
    }
} 