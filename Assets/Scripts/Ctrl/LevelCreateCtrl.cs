using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels")]
public class LevelCreateCtrl : ScriptableObject
{
    public List<BottleProperty> bottles;
    public GameType gameType;

    [System.Serializable]
    public class BottleProperty
    {
        public List<int> waterSet = new List<int>();
        public List<bool> isHide = new List<bool>();
        public List<WaterItem> waterItem = new List<WaterItem>();
        public int numCake;
        public int lockType;
        public bool isClearHide, isNearHide;
    }
    public List<int> clearList;

    public List<int> hideList;

    public int bombNum;

    public int countDownNum;

    public float timeCountDown;

}
