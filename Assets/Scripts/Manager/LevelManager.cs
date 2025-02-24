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
using System.Linq;
using Spine.Unity;

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
    List<ChangePair> changeList;
    public List<GameObject> createFx = new List<GameObject>();
    public LevelCreateCtrl nowLevel;
    public Color ItemColor;

    public Material shineMaterial;// 材质
    public float speed = 1.0f; // 光带移动速度

    //public List<>

    [SerializeField]
    SpriteRenderer levelBgSprite;

    public int levelId = 1, moreCakeNum = 0, lastStar, moveNum, moreMoveNum, bombMaxNum, countDownNum;
    public float timeCountDown, timeNow, test;
    public GameObject mahoujinGo, broomBullet;
    public SkeletonGraphic mahoujinSpine;
    bool isOpenDefeat = false, isBomb = false, isCountDown = false, isTimeCountDown = false;
    public bool isPlayAnim, isPlayFxAnim;
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
        this.GetUtility<SaveDataUtility>().SaveLevel(11);
        //this.GetUtility<SaveDataUtility>().SaveLevel(1);

        if (GameCtrl.Instance.FirstBottle != null)
        {
            GameCtrl.Instance.FirstBottle.OnCancelSelect();
            GameCtrl.Instance.FirstBottle = null;
        }
        emptyBottle.numCake = 4;
        levelId = this.GetUtility<SaveDataUtility>().GetLevelClear();

        //var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
        if (levelId <= 5)
        {
            StartGame(levelId);
        }

        UIKit.OpenPanel<UIBegin>();

    }

    public void UseItem(int itemId, Transform fromTarget)
    {
        Vector3 fromPos = fromTarget.position;
        switch ((ItemType)itemId)
        {
            case ItemType.ClearItem:
                ////////随机色块
                var clearlist = CheckCanClearList();
                int clearColorIdx = UnityEngine.Random.Range(0, clearlist.Count);
                int clearColor = clearlist[clearColorIdx];
                ////////清除色块
                ClearColor(clearColor, fromPos);

                break;
            case ItemType.MagnetItem:
                ShowMahoujin();
                break;
            case ItemType.MakeColorItem:
                StartCoroutine(AddColor(fromPos));

                break;

            case ItemType.ChangeGreen:
                int changeGreenColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangeGreen)
                    {
                        changeGreenColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changeGreenColorTo = 1;
                ////////更换色块
                ChangeColor(changeGreenColorFrom, changeGreenColorTo, fromTarget);
                break;

            case ItemType.ChangeOrange:
                int changeOrangeColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangeOrange)
                    {
                        changeOrangeColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changeOrangeColorTo = 7;
                ////////更换色块
                ChangeColor(changeOrangeColorFrom, changeOrangeColorTo, fromTarget);
                break;
            case ItemType.ChangePink:
                int changePinkColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangePink)
                    {
                        changePinkColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changePinkColorTo = 3;
                ////////更换色块
                ChangeColor(changePinkColorFrom, changePinkColorTo, fromTarget);
                break;
            case ItemType.ChangeYellow:
                int changeYellowColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangeYellow)
                    {
                        changeYellowColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changeYellowColorTo = 6;
                ////////更换色块
                ChangeColor(changeYellowColorFrom, changeYellowColorTo, fromTarget);
                break;
            case ItemType.ChangePurple:
                int changePurpleColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangePurple)
                    {
                        changePurpleColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changePurpleColorTo = 10;
                ////////更换色块
                ChangeColor(changePurpleColorFrom, changePurpleColorTo, fromTarget);
                break;
            case ItemType.ChangeDarkBlue:
                int changeDarkBlueColorFrom = 0;
                foreach (var pair in changeList)
                {
                    if(pair.item == ItemType.ChangeDarkBlue)
                    {
                        changeDarkBlueColorFrom = pair.NeedChangeColor;
                        changeList.Remove(pair);
                        break;
                    }
                }

                int changeDarkBlueColorTo = 4;
                ////////更换色块
                ChangeColor(changeDarkBlueColorFrom, changeDarkBlueColorTo, fromTarget);
                break;
            case ItemType.ClearPink:
                ////////清除色块
                ClearColor(3, fromPos);
                break;
            case ItemType.ClearOrange:
                ////////清除色块
                ClearColor(7, fromPos);
                break;
            case ItemType.ClearBlue:
                ////////清除色块
                ClearColor(4, fromPos);
                break;
            case ItemType.ClearYellow:
                ////////清除色块
                ClearColor(6, fromPos);
                break;
            case ItemType.ClearDarkGreen:
                ////////清除色块
                ClearColor(9, fromPos);
                break;
            case ItemType.ClearRed:
                ////////清除色块
                ClearColor(2, fromPos);
                break;
            case ItemType.ClearGreen:
                ////////清除色块
                ClearColor(1, fromPos);
                break;
        }
    }

    IEnumerator AddColor(Vector3 fromPos)
    {
        yield return new WaitForSeconds(1f);
        var bottleList = GetMakeColorBottle();
        List<BottleCtrl> useBottles = new List<BottleCtrl>();
        int addIdx = 0;
        while (hideColor.Count != 0)
        {
            int addColorIdx = UnityEngine.Random.Range(0, hideColor.Count);
            int addColor = hideColor[addColorIdx];
            var useBottle = bottleList[addIdx];
            if (useBottle.waters.Count < useBottle.maxNum)
            {
                useBottles.Add(useBottle);
                useBottle.AddColor(addColor, fromPos);
                hideColor.RemoveAt(addColorIdx);
                Debug.Log("添加颜色 " + addColor);
            }
            else
            {
                addIdx++;
            }
        }

        foreach (var bottle in useBottles)
        {
            StartCoroutine(bottle.FinishHide());
        }
    }

    public void FinishClear(int clearColor, int idx)
    {

        foreach(var item in nowBottles)
        {
            item.CheckUnlockHide(clearColor);
            item.CheckNearHide(idx);
        }

        StopCoroutine(WaitCheckFinish());
        StartCoroutine(WaitCheckFinish(clearColor));
    }

    public IEnumerator WaitCheckFinish(int clearColor = 0)
    {
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            Debug.Log("胜利");
        }

        yield return new WaitForSeconds(4);
        
        if(clearColor != 0)
        {
            clearList.Remove(clearColor);
        }
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId + 1);
            if(levelId  <= 5)
            {
                this.SendEvent<LevelStartEvent>();
                StartGame(levelId + 1);
            }
            else
            {
                this.SendEvent<LevelClearEvent>();
            }
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
            if(!bottle.isFreeze && bottle.waters.Count < 4 && !bottle.isClearHide && !bottle.isNearHide)
            {
                ret.Add(bottle);
            }
        }
        return ret;
    }

    public void ChangeColor(int from, int to, Transform target)
    {
        if (clearList.Contains(from))
        {
            clearList.Remove(from);
            clearList.Add(to);
        }
        foreach (var bottle in nowBottles)
        {
            bottle.ChangeColor(from, to, target);
            bottle.CheckHide();
            bottle.CheckFinish(true);
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


    public void CheckFinishChange(int color)
    {
        foreach (var bottle in nowBottles)
        {
            bottle.CheckUnlockHide(color);
        }
    }

    public void ClearColor(int color, Vector3 fromPos)
    {
        StartCoroutine(ClearColorCoroutine(color, fromPos));
        //StartCoroutine(WaterShine());

    }

    IEnumerator ClearColorCoroutine(int color, Vector3 fromPos)
    {
        isPlayFxAnim = true;
        yield return new WaitForSeconds(1f);

        foreach (var bottle in nowBottles)
        {
            bottle.PlayBroomBullet(color, fromPos);
        }
        
        yield return new WaitForSeconds(0.2f);

        if (clearList.Contains(color))
        {
            clearList.Remove(color);
        }




        foreach (var bottle in nowBottles)
        {
            bottle.RemoveAllOneColor(color);
            bottle.CheckUnlockHide(color);
        }
    }

    public void BreakIce()
    {
        int iceIdx = UnityEngine.Random.Range(0, iceBottles.Count);

        var bottle = iceBottles[iceIdx];
        iceBottles.RemoveAt(iceIdx);
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
        moreBottle = 0;
        cantClearColorList.Clear();
        cantChangeColorList.Clear();
        levelId = id;
        VictoryBottle = -1;
        var levelInfo = levels[levelId - 1];
        iceBottles.Clear();
        nowLevel = levelInfo;
        bombMaxNum = levelInfo.bombNum;
        countDownNum = levelInfo.countDownNum;
        timeCountDown = levelInfo.timeCountDown;
        timeNow = 0;
        changeList = new List<ChangePair>(nowLevel.changeList);

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

    public void AddBottle(bool isHalf)
    {
        moreBottle++;
        ShowBottleGo();
        //ShowBottleGo(nowBottles.Count + 1);
        MoveAndAddBottle(isHalf);
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
            else if (nowLevel.topNum >= nowLevel.bottomNum)
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
            bottle.maxNum = 4;
        }

        for(int i = 0; i < moreBottle; i++)
        {
            int useIdx = levelInfo.bottles.Count + i;
            var bottle = nowBottles[useIdx];
            bottle.Init(emptyBottle, useIdx);
        }
    }

    public void MoveAndAddBottle(bool isHalf)
    {
        var num = nowBottles.Count;
        for (int i = 0; i < tempBottles.Count; i++)
        {
            nowBottles[i].MoveBottle(tempBottles[i]);            
        }

        nowBottles[nowBottles.Count - 1].Init(emptyBottle, nowBottles.Count);

        if(isHalf)
        {
            nowBottles[nowBottles.Count - 1].maxNum = 1;
        }
        else
        {
            nowBottles[nowBottles.Count - 1].maxNum = 4;
        }
    }

    public void RefreshLevel()
    {
        clearList = new List<int>(nowLevel.clearList); 
        hideColor = new List<int>(nowLevel.hideList);
        changeList = new List<ChangePair>(nowLevel.changeList);
        cantClearColorList.Clear();

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
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(WaterShine());
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

        test += Time.deltaTime;
        if(test >= 1)
        {
            test = 0;
            CheckVitality();
            //Debug.Log("时间 " + this.GetUtility<SaveDataUtility>().GetVitalityNum()  + " " + this.GetUtility<SaveDataUtility>().GetVitalityTime());
        }
    }

    void CheckVitality()
    {
        int lastVitalityNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();

        if (lastVitalityNum < 5)
        {
            long recoveryTime = this.GetUtility<SaveDataUtility>().GetVitalityTime() + (5 - lastVitalityNum) * GameConst.RecoveryTime;
            long timeOffset = recoveryTime - this.GetUtility<SaveDataUtility>().GetNowTime();
            //Debug.Log("体力 " + lastVitalityNum + " " + timeOffset);
            if (timeOffset > 0)
            {
                long checkTime = this.GetUtility<SaveDataUtility>().GetNowTime() - this.GetUtility<SaveDataUtility>().GetVitalityTime();
                int addNum = Mathf.FloorToInt((float)checkTime / GameConst.RecoveryTime);

                //Debug.Log("体力 " + addNum + " " + timeOffset);

                if (addNum > GameConst.MaxVitality)
                {
                    addNum = GameConst.MaxVitality;
                    this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
                }
                else if (addNum >= 1)
                {
                    this.GetUtility<SaveDataUtility>().SetVitality(lastVitalityNum + addNum, (this.GetUtility<SaveDataUtility>().GetVitalityTime() + (addNum) * GameConst.RecoveryTime) + "");
                }
            }
            else
            {
                this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
            }

            this.SendEvent<VitalityTimeChangeEvent>(new VitalityTimeChangeEvent() { timeOffset = timeOffset });
        }
    }

    public void RecordLast()
    {
        foreach(var bottle in nowBottles)
        {
            bottle.RecordLast();
        }
    }

    public void ReturnLast()
    {
        foreach (var bottle in nowBottles)
        {
            bottle.ReturnLast();
        }
    }

    public void RemoveAll()
    {
        foreach (var bottle in nowBottles)
        {
            bottle.SetNormal();
        }
    }

    public void RemoveHide()
    {
        foreach (var bottle in nowBottles)
        {
            bottle.RemovHide();
        }
    }

    public IEnumerator WaterShine()
    {
        //float shineTime = -0.2f;

        //while (shineTime <= 2.5f)
        //{
        //    shineTime += 0.02f * speed;
            //shineMaterial.SetFloat("_BandPosition", shineTime);
        yield return new WaitForSeconds(0.02f);
        //}
    }

    public void ShowMahoujin()
    {
        StartCoroutine(ShowMahoujinCoroutine());
    }

    IEnumerator ShowMahoujinCoroutine()
    {
        isPlayFxAnim = true;
        mahoujinGo.SetActive(true);
        mahoujinSpine.AnimationState.SetEmptyAnimation(0, 0f);
        yield return new WaitForSeconds(2f);
        mahoujinSpine.AnimationState.SetAnimation(0, "attack", false);

        yield return new WaitForSeconds(3.34f);
        RemoveAll();
        mahoujinGo.SetActive(false);
        isPlayFxAnim = false;
    }
}

[Serializable]
public class MovePair
{
    public int from;
    public int to;
}

public class BottleRecord
{
    public bool isFinish, isFreeze, isClearHide, isNearHide;
    public List<int> waters = new List<int>();
    public List<bool> hideWaters = new List<bool>();
    public List<WaterItem> waterItems = new List<WaterItem>();
}