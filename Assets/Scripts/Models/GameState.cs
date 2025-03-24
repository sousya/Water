using System.Collections.Generic;
using UnityEngine;

namespace WaterGame.Models
{
    public class GameState
    {
        public bool IsPlaying { get; set; }
        public bool IsPaused { get; set; }
        public int CurrentLevel { get; set; }
        public int Moves { get; set; }
        public float TimeRemaining { get; set; }
        public IBottleController SelectedBottle { get; set; }
        public List<IBottleController> Bottles { get; set; } = new List<IBottleController>();
        public List<int> CantClearColors { get; set; } = new List<int>();
        public List<int> CantChangeColors { get; set; } = new List<int>();
        public bool IsPlayingAnimation { get; set; }
        public bool IsPlayingFxAnimation { get; set; }
    }
} 