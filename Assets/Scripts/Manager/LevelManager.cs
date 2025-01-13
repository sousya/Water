using QFramework;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using LitJson;
using QAssetBundle;
using TMPro;
using System.IO;
using System;
using Google.Play.Review;
using UnityEngine.UI;
using Unity.VisualScripting;
using QFramework.Example;
using static LevelCreateCtrl;
using System.Collections;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoBehaviour, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public static LevelManager Instance;
    public ReviewManager _reviewManager;
    public List<GameObject> bottleNodes = new List<GameObject>();
    public List<LevelCreateCtrl> levels = new List<LevelCreateCtrl>();
    public List<int> clearList = new List<int>();
    public List<int> cantClearColorList = new List<int>();
    public List<int> cantChangeColorList = new List<int>();
    public List<BottleCtrl> nowBottles = new List<BottleCtrl>();
    public List<BottleCtrl> tempBottles = new List<BottleCtrl>();
    public List<BottleCtrl> iceBottles = new List<BottleCtrl>();
    public List<BottleCtrl> bottles = new List<BottleCtrl>();
    public List<BottleCtrl> topBottle = new List<BottleCtrl>();
    public List<BottleCtrl> bottomBottle = new List<BottleCtrl>();
    public List<int> hideColor = new List<int>();
    public List<Color> waterColor = new List<Color>();
    public List<Sprite> waterTopSp;
    public List<Sprite> waterSp;
    public int VictoryBottle, moreBottle;
    public BottleProperty emptyBottle =  new BottleProperty();
    public Transform gameCanvas;

    public LevelCreateCtrl nowLevel;

    [SerializeField]
    SpriteRenderer levelBgSprite;

    public int levelId = 1, moreCakeNum = 0, lastStar, moveNum, moreMoveNum, bombMaxNum, countDownNum;
    public float timeCountDown, timeNow;
    bool isOpenDefeat = false, isBomb = false, isCountDown = false, isTimeCountDown = false;
    [SerializeField]
    GameCtrl gameCtrl;

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }   

    private void Awake()
    {
        Instance = this;
        _reviewManager = new ReviewManager();
    }

    private void Start()
    {
        //StartGame(levelId);
        //UIKit.OpenPanel("UILevelMain", UILevel.Common, "uilevelmain_prefab"); 
        //UIKit.OpenPanel<UIBegin>(UILevel.Common);
        //UIKit.OpenPanel<UIJudge>(UILevel.Common);
        if(GameCtrl.Instance.FirstBottle != null)
        {
            GameCtrl.Instance.FirstBottle.OnCancelSelect();
            GameCtrl.Instance.FirstBottle = null;
        }
        emptyBottle.numCake = 4;
        levelId = this.GetUtility<SaveDataUtility>().GetLevelClear();

        UIKit.OpenPanel<UIBegin>();
    }

    public void UseItem(int itemId)
    {
        switch((ItemType)itemId)
        {
            case ItemType.ClearItem:
                ////////随机色块
                var clearlist = CheckCanClearList();
                int clearColorIdx = UnityEngine.Random.Range(0, clearlist.Count);
                int clearColor = clearlist[clearColorIdx];
                ////////清除色块
                ClearColor(clearColor);

                break;
            case ItemType.MagnetItem:
                foreach(var bottle in nowBottles)
                {
                    bottle.SetNormal();
                }
                break;
            case ItemType.MakeColorItem:
                var bottleList = GetMakeColorBottle();
                int addIdx = 0;
                while(hideColor.Count != 0)
                {
                    int addColorIdx = UnityEngine.Random.Range(0, hideColor.Count);
                    int addColor = hideColor[addColorIdx];
                    var useBottle = bottleList[addIdx];
                    if (useBottle.waters.Count < useBottle.maxNum)
                    {
                        useBottle.AddColor(addColor);
                        hideColor.RemoveAt(addColorIdx);
                    }
                    else
                    {
                        addIdx++;
                    }
                }

                foreach(var bottle in bottleList)
                {
                    bottle.SetBottleColor();
                    bottle.CheckItem();
                    bottle.CheckFinish();
                    foreach(var item  in bottle.waterImg)
                    {
                        item.waterImg.fillAmount = 1;
                    }
                }
                break;

            case ItemType.ChangeGreen:
                int changeGreenColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangeGreen)
                    {
                        changeGreenColorFrom = pair.NeedChangeColor;
                    }
                }

                int changeGreenColorTo = 1;
                ////////更换色块
                ChangeColor(changeGreenColorFrom, changeGreenColorTo);
                break;

            case ItemType.ChangeOrange:
                int changeOrangeColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangeOrange)
                    {
                        changeOrangeColorFrom = pair.NeedChangeColor;
                    }
                }

                int changeOrangeColorTo = 7;
                ////////更换色块
                ChangeColor(changeOrangeColorFrom, changeOrangeColorTo);
                break;
            case ItemType.ChangePink:
                int changePinkColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangePink)
                    {
                        changePinkColorFrom = pair.NeedChangeColor;
                    }
                }

                int changePinkColorTo = 3;
                ////////更换色块
                ChangeColor(changePinkColorFrom, changePinkColorTo);
                break;
            case ItemType.ChangeYellow:
                int changeYellowColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangeYellow)
                    {
                        changeYellowColorFrom = pair.NeedChangeColor;
                    }
                }

                int changeYellowColorTo = 6;
                ////////更换色块
                ChangeColor(changeYellowColorFrom, changeYellowColorTo);
                break;
            case ItemType.ChangePurple:
                int changePurpleColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangePurple)
                    {
                        changePurpleColorFrom = pair.NeedChangeColor;
                    }
                }

                int changePurpleColorTo = 3;
                ////////更换色块
                ChangeColor(changePurpleColorFrom, changePurpleColorTo);
                break;
            case ItemType.ChangeDarkBlue:
                int changeDarkBlueColorFrom = 0;
                foreach (var pair in nowLevel.changeList)
                {
                    if(pair.item == ItemType.ChangeDarkBlue)
                    {
                        changePurpleColorFrom = pair.NeedChangeColor;
                    }
                }

                int changeDarkBlueColorTo = 3;
                ////////更换色块
                ChangeColor(changeDarkBlueColorFrom, changeDarkBlueColorTo);
                break;
        }
    }
    public void FinishClear(int clearColor, int idx)
    {
        clearList.Remove(clearColor);

        foreach(var item in nowBottles)
        {
            item.CheckUnlockHide(clearColor);
            item.CheckNearHide(idx);
        }

        StopCoroutine(WaitCheckFinish());
        StartCoroutine(WaitCheckFinish());
    }

    public IEnumerator WaitCheckFinish()
    {
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            Debug.Log("胜利");
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId); 
        }

        yield return new WaitForSeconds(4);
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            StartGame(levelId + 1);
        }
    }

    public void CheckVictory(int idx)
    {
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            Debug.Log("胜利");
        }
    }

    public void VictoryEnter(int idx)
    {
        if(VictoryBottle == idx)
        {
            //StartGame(levelId + 1);
        }
    }

    public List<BottleCtrl> GetMakeColorBottle()
    {
        List<BottleCtrl> ret = new List<BottleCtrl>();
        foreach(var bottle in nowBottles)
        {
            if(!bottle.isFreeze && bottle.waters.Count < 4)
            {
                ret.Add(bottle);
            }
        }
        return ret;
    }

    public void ChangeColor(int from, int to)
    {
        if (clearList.Contains(from))
        {
            clearList.Remove(from);
            clearList.Add(to);
        }
        foreach (var bottle in nowBottles)
        {
            bottle.ChangeColor(from, to);
            bottle.CheckHide();
            bottle.CheckFinish();
        }
    }

    public List<int> CheckCanClearList()
    {
        List<int> ret = new List<int>();
        foreach(var color in clearList)
        {
            //if (!cantClearColorList.Contains(color))
            if (!cantChangeColorList.Contains(color))
            {
                ret.Add(color);
            }
        }
        return ret;
    }


    public void ClearColor(int color)
    {
        if(clearList.Contains(color))
        {
            clearList.Remove(color);
        }



        foreach (var bottle in nowBottles)
        {
            bottle.RemoveAllOneColor(color);
        }
    }

    public void BreakIce()
    {
        int iceIdx = UnityEngine.Random.Range(0, iceBottles.Count);

        var bottle = iceBottles[iceIdx];

        bottle.UnlockIceWater();
    }

    public void CancelBomb()
    {
        isBomb = false;
    }

    public void AddMoveNum()
    {
        moveNum++;
        if((moveNum >= bombMaxNum && isBomb) || (isCountDown && moveNum >= countDownNum))
        {
            OnDefeat();
        }
    }

    public void StartGame(int id)
    {
        cantClearColorList.Clear();
        cantChangeColorList.Clear();
        levelId = id;
        VictoryBottle = -1;
        var levelInfo = levels[levelId - 1];

        nowLevel = levelInfo;
        bombMaxNum = levelInfo.bombNum;
        countDownNum = levelInfo.countDownNum;
        timeCountDown = levelInfo.timeCountDown;
        timeNow = 0;

        if (bombMaxNum > 0)
        {
            isBomb = true;
        }
        if(countDownNum > 0)
        {
            isCountDown = true;
        }
        if(timeCountDown > 0)
        {
            isTimeCountDown = true;
        }

        clearList = new List<int>(levelInfo.clearList);
        hideColor = new List<int>(levelInfo.hideList);
        nowBottles.Clear();

        SetBottle(levelInfo);

        this.SendEvent<LevelStartEvent>();
    }

    public void AddBottle()
    {
        moreBottle++;
        ShowBottleGo();
        //ShowBottleGo(nowBottles.Count + 1);
        MoveAndAddBottle();
    }

    public void SetBottle(LevelCreateCtrl levelInfo)
    {
        //ShowBottleGo(levelInfo.bottles.Count);
        ShowBottleGo();
        InitBottle(levelInfo);
    }

    //public void ShowBottleGo(int num)
    public void ShowBottleGo()
    {
        tempBottles = new List<BottleCtrl>(nowBottles);
        nowBottles.Clear();

        int topAdd = 0;
        int bottomAdd = 0;
        
        for(int i = 0; i < moreBottle; i++)
        {
            if (nowLevel.topNum < nowLevel.bottomNum)
            {
                topAdd += 1;
            }
            else if (nowLevel.topNum > nowLevel.bottomNum)
            {
                bottomAdd += 1;
            }
        }

        for(int i = 0; i < topBottle.Count; i++)
        {
            var useBottle = topBottle[i];
            var num = (topAdd + nowLevel.topNum);
            useBottle.gameObject.SetActive(i < num);
            if(i < num)
            {
                nowBottles.Add(useBottle);
            }
        }

        for(int i = 0; i < bottomBottle.Count; i++)
        {
            var useBottle = bottomBottle[i];
            var num = (bottomAdd + nowLevel.bottomNum);
            useBottle.gameObject.SetActive(i < num);
            if (i < num)
            {
                nowBottles.Add(useBottle);
            }
        }
        //var num = levelInfo.bottles.Count;
        //if (num <= 6)
        //{
        //    for (int i = 0; i < bottles.Count; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < num);
        //        if(i < num)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}
        //else if (num == 7)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 4);
        //        if(i < 4)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 11);
        //        if (i < 11)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}
        //else if (num == 8)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 4);

        //        if (i < 4)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 12);
        //        if (i < 12)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}
        //else if (num == 9)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 5);
        //        if (i < 5)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 12);
        //        if (i < 12)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}
        //else if (num == 10)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 5);
        //        if (i < 5)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 13);
        //        if (i < 13)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}
        //else if (num == 11)
        //{
        //    for (int i = 0; i < 8; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 6);
        //        if (i < 6)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //    for (int i = 8; i < 16; i++)
        //    {
        //        var useBottle = bottles[i];
        //        useBottle.gameObject.SetActive(i < 13);
        //        if (i < 13)
        //        {
        //            nowBottles.Add(useBottle);
        //        }
        //    }
        //}

    }

    public void InitBottle(LevelCreateCtrl levelInfo)
    {
        for (int i = 0; i < levelInfo.bottles.Count; i++)
        {
            var bottle = nowBottles[i];
            bottle.Init(levelInfo.bottles[i], i);
        }
    }

    public void MoveAndAddBottle()
    {
        var num = nowBottles.Count;
        for (int i = 0; i < tempBottles.Count; i++)
        {
            nowBottles[i].MoveBottle(tempBottles[i]);            
        }

        nowBottles[nowBottles.Count - 1].Init(emptyBottle, nowBottles.Count);
    }

    public void RefreshLevel()
    {
        clearList = new List<int>(nowLevel.clearList);
        hideColor = new List<int>(nowLevel.hideList);
        //ShowBottleGo(nowBottles.Count);
        ShowBottleGo();
        InitBottle(nowLevel);
    }

    public void Update()
    {
        TimeCountDown();


        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShareManager.Instance.ShareScreen();
        }
    }

    public void OnDefeat()
    {
        Debug.Log("失败");
    }

    public void TimeCountDown()
    {
        if(isTimeCountDown)
        {
            timeNow += Time.deltaTime;
            if(timeNow >= timeCountDown)
            {
                OnDefeat();
                isTimeCountDown = false;
            }
        }
    }
}
