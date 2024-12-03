using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Levels")]
public class LevelCreateCtrl : ScriptableObject
{
    public List<LevelProperty> bottles;
   

    [System.Serializable]
    public class LevelProperty
    {
        public List<int> waterSet;
        public List<bool> isHide;
        public List<int> cakeLock;
        public List<int> cakeKey;
        public List<int> cakeBomb;
        public int numCake;
        public bool needUnlock = false;
        public bool isSpecial = false;
        public int cantNum;
        public int lockType;
        public List<int> connectOther;
        public bool showConnect;
    }
    public int winCakeNum;

    public List<int> unlockCake;

    public int unlockNow, unlockNeed;

    public int Star3;

    public int Star2;

    public int Total;

    public int test;

    public bool hasBomb;

    public int bombStep;

    public bool stepHide;

    public List<int> hideList;
    public List<int> hideNum;

    public bool allHide;

    public bool stillHide;
}
