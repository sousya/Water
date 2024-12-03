using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using LitJson;
using System.Runtime.CompilerServices;
using QFramework.Example;
using QAssetBundle;
using TMPro;
using System.IO;
using Unity.VisualScripting;
using System;
using Google.Play.Review;
using UnityEngine.UI;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoBehaviour, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public static LevelManager Instance;

    public List<Sprite> cakeFourImgs = new List<Sprite>();
    public List<Sprite> cakeSixImgs = new List<Sprite>();
    public List<Sprite> cakeEightImgs = new List<Sprite>();
    public List<Sprite> cakeFloorImgs = new List<Sprite>();
    public List<Sprite> cakeShowFloorImgs = new List<Sprite>();
    public List<Sprite> cakeThreeImgs = new List<Sprite>();
    public List<Sprite> lockTypeImgs = new List<Sprite>();
    public List<Sprite> levelBgs = new List<Sprite>();
    public List<Sprite> unlockCakeSp = new List<Sprite>();
    public List<GameObject> cakeNodes = new List<GameObject>();
    public List<LevelCreateCtrl> levels = new List<LevelCreateCtrl>();
    public List<LevelCreateCtrl> splevels = new List<LevelCreateCtrl>();
    public List<LevelCreateCtrl> orderlevels = new List<LevelCreateCtrl>();
    public List<LevelCreateCtrl> jigsawlevels = new List<LevelCreateCtrl>();
    public Sprite hide;
    public LevelBgFxCtrl levelBgFx;

    public LevelCreateCtrl nowLevel;

    [SerializeField]
    SpriteRenderer levelBgSprite;
    [SerializeField]
    List<CakeCtrl> cakes = new List<CakeCtrl>();
    [SerializeField]
    List<CakeCtrl> sixCakes = new List<CakeCtrl>();
    public List<CakeCtrl> nowCakes = new List<CakeCtrl>();
    List<CakeCtrl> tempCakes = new List<CakeCtrl>();
    [SerializeField]
    List<RectTransform> cakeTrans = new List<RectTransform>();
    [SerializeField]
    List<RectTransform> sixCakeTrans = new List<RectTransform>();
    [SerializeField]
    List<CakeCtrl> moreCakes = new List<CakeCtrl>();
    public GameObject levelNode, sixLevelNode, fxNode;
    public int finishNum = 0;
    public int levelId = 1, moreCakeNum = 0, lastStar, moveNum, moreMoveNum;
    bool isOpenDefeat = false;
    [SerializeField]
    GameCtrl gameCtrl;

    List<int> ADList = new List<int>()
    {
        6, 8, 10, 11
    };

    public List<int> hardLevels = new List<int>()
    {
        5, 15, 29, 45, 60, 66, 74, 81, 90, 97, 107, 116
    };

    public List<int> spLevels = new List<int>()
    {
        9, 14, 43, 28, 33, 38, 42, 53, 58, 63, 68, 73, 78, 83, 88, 93, 98, 103, 108, 113, 118, 123
    };

    public int returnNum = 0;

    public List<KeyValuePair<int, int>> moveHistory = new List<KeyValuePair<int, int>>();
    public bool NoAD = false;
    public bool ChangeEnd = false, musicOn = true;

    public List<List<int>> unlockCake = new List<List<int>>()
    {
        new List<int>{2},new List<int>{3},new List<int>{4,5},new List<int>{6},new List<int>{7},new List<int>{13}
        ,new List<int>{14},new List<int>{8,9},new List<int>{15},new List<int>{16},new List<int>{10,11,12}
        ,new List<int>{17},new List<int>{18},new List<int>{19},new List<int>{20},new List<int>{21}
        ,new List<int>{22},new List<int>{23}
    };
    public List<int> unlockLevel = new List<int>();
    public List<int> unlockNum = new List<int>();
    int nowUnlockNum = 1;
    int unlockStep = 0;
    public bool showUnlock, isSpLevel, isClear;
    public int levelType = 0;
    private ReviewManager _reviewManager;
    public VerticalLayoutGroup layout;

    int hideNum = 0, hideIdx = 0;
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

        levelId = this.GetUtility<SaveDataUtility>().GetLevelClear();
        NoAD = this.GetUtility<SaveDataUtility>().GetNoAD();
    }

    IEnumerator Judge()
    {
        Debug.Log("BegeinJudge");
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            Debug.LogError("BegeinJudge  " + requestFlowOperation.Error.ToString());
            yield break;
        }
       var _playReviewInfo = requestFlowOperation.GetResult();
       var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        UIKit.HidePanel<UIJudge>();
    }

    public void BeginJudge()
    {
        StartCoroutine(Judge());
    }

    public void ReStart()
    {
        fxNode.SetActive(false);

        if (levelType == 1)
        {
            StartSpGame(levelId, true);
        }
        else if(levelType == 0)
        {
            StartGame(levelId, true);
        }
        else if(levelType == 2)
        {
            StartOrderGame(true);
        }
        else if(levelType == 3)
        {
            StartJigsawGame(levelId, true);
        }
    }

    public void StartSpGame(int id, bool isRestart = false, bool isClear = false)
    {
        TopOnADManager.Instance.RemoveBannerAd();
        levelId = id;
        nowLevel = splevels[id - 1];

        if (!isRestart)
        {
            moreCakeNum = 0;
        }
        StartSpGame(nowLevel, isRestart, isClear);

    }

    public void StartOrderGame(bool isRestart = false, bool isClear = false)
    {
        TopOnADManager.Instance.RemoveBannerAd();
        SaveDataUtility save = this.GetUtility<SaveDataUtility>();
        if (save.GetLevelClear() < 9)
        {
            return;
        }

        int id = save.GetOrder();
        levelId = id;

        if (!isRestart)
        {
            moreCakeNum = 0;
            if ((levelId == 2 || levelId == 4) && isClear == true)
            {
                TopOnADManager.Instance.ShowInterstitialAd();
            }
        }
        else
        {
                TopOnADManager.Instance.ShowInterstitialAd();
        }

        if (id > 4 && !this.GetUtility<SaveDataUtility>().GetShowOrderEnd())
        {
            var ui = UIKit.GetPanel<UIBegin>();
            ui.ShowOrderEnd();
            //this.SendEvent<ShowOrderEnd>();
        }
        else
        {
            nowLevel = orderlevels[id - 1];

            if (!isRestart)
            {
                moreCakeNum = 0;
            }
            StartOrderGame(nowLevel, isRestart, isClear);
        }

    }
    public void StartJigsawGame(int id, bool isRestart = false, bool isClear = false)
    {
        TopOnADManager.Instance.RemoveBannerAd();
        levelId = id;
        if (!isRestart)
        {
            moreCakeNum = 0;
        }
        else
        {
            TopOnADManager.Instance.ShowInterstitialAd();
        }

        nowLevel = jigsawlevels[levelId - 1];

        if (!isRestart)
        {
            moreCakeNum = 0;
        }
        StartJigsawGame(nowLevel, isRestart, isClear);

    }

    public void StartGame(int id, bool isRestart = false, bool isClear = false)
    {
        //string trackStr = new System.Diagnostics.StackTrace().ToString();
        Debug.LogError("CheckGameStartId: " + id );
        UIKit.ClosePanel<UIDefeat>();
        //Debug.LogError("CheckGameStartId: " + id + " Stack Info: " + trackStr);
        levelId = id;
        nowLevel = levels[id - 1];
        moveNum = 0;
        moreMoveNum = 0;

        if (!isRestart)
        {
            moreCakeNum = 0;
            if ((levelId >= 11 && ((levelId - 11) % 5 == 2 || (levelId - 11) % 5 == 0 || levelId == 11)) && isClear == true)
            {
                TopOnADManager.Instance.ShowInterstitialAd();
            }
        }
        else
        {
            TopOnADManager.Instance.ShowInterstitialAd();
        }
        TopOnADManager.Instance.RemoveBannerAd();
        TopOnADManager.Instance.ShowBannerAd();

        //if (levelId == 6)
        //{
        //    UIKit.OpenPanel<UIJudge>(UILevel.Common, null, "uijudge_prefab", "UIJudge");
        //}
        //else if(levelId > 11)
        //{
        //    TopOnADManager.Instance.ShowInterstitialAd();
        //}
        StartGame(nowLevel, isRestart, isClear);

    }

    public void CheckBegin()
    {
        StartCoroutine(OnBegin());
    }
    IEnumerator OnBegin()
    {
        yield return new WaitForEndOfFrame();
        UIKit.OpenPanel<UIRank>(UILevel.Common, null, "uibegin_prefab", "UIBegin");
        UIKit.HidePanel<UIRank>();
        yield return new WaitForEndOfFrame();
        UIKit.OpenPanel<UIBegin>(UILevel.Bg, null, "uibegin_prefab", "UIBegin");
        this.SendEvent<LevelStartEvent>();
    }
    void StartSpGame(LevelCreateCtrl level, bool isRestart = false, bool isClear = false)
    {
        fxNode.SetActive(false);
        levelType = 1;
        this.SendEvent<LevelStartEvent>();
        nowCakes.Clear();
        gameCtrl.FirstCake = null;
        gameCtrl.SecondCake = null;
        moveHistory.Clear();
        finishNum = 0;
        CheckMoreCake(isRestart);

        levelBgSprite.sprite = levelBgs[0];
        levelBgFx.ShowFx();

        foreach (var cake in cakes)
        {
            cake.OnCancelSelect();
            cake.ClearCake();
        }

        if (!isRestart)
        {
            SetShowCake();
        }
        else
        {
            TopOnADManager.Instance.ShowInterstitialAd();
            SetShowCake(moreCakeNum);
        }


        StopCoroutine(WaitChange(level, isRestart));
        ChangeEnd = false;
        StartCoroutine(WaitChange(level, isRestart));
    }

    void StartOrderGame(LevelCreateCtrl level, bool isRestart = false, bool isClear = false)
    {
        fxNode.SetActive(false);
        isSpLevel = true;
        levelType = 2;
        this.SendEvent<LevelStartEvent>();
        nowCakes.Clear();
        gameCtrl.FirstCake = null;
        gameCtrl.SecondCake = null;
        moveHistory.Clear();
        finishNum = 0;
        CheckMoreCake(isRestart);

        levelBgSprite.sprite = levelBgs[0];
        levelBgFx.ShowFx();


        foreach (var cake in cakes)
        {
            cake.OnCancelSelect();
            cake.ClearCake();
        }

        if (!isRestart)
        {
            SetShowCake();
        }
        else
        {
            SetShowCake(moreCakeNum);
        }



        StopCoroutine(WaitChange(level, isRestart));
        ChangeEnd = false;
        StartCoroutine(WaitChange(level, isRestart));
    }
    void StartJigsawGame(LevelCreateCtrl level, bool isRestart = false, bool isClear = false)
    {
        fxNode.SetActive(false);
        levelType = 3;
        this.SendEvent<LevelStartEvent>();
        nowCakes.Clear();
        gameCtrl.FirstCake = null;
        gameCtrl.SecondCake = null;
        moveHistory.Clear();
        finishNum = 0;
        CheckMoreCake(isRestart);

        levelBgSprite.sprite = levelBgs[0];
        levelBgFx.ShowFx();


        foreach (var cake in cakes)
        {
            cake.OnCancelSelect();
            cake.ClearCake();
        }

        if (!isRestart)
        {
            SetShowCake();
        }
        else
        {
            SetShowCake(moreCakeNum);
        }



        StopCoroutine(WaitChange(level, isRestart));
        ChangeEnd = false;
        StartCoroutine(WaitChange(level, isRestart));
    }
    void StartGame(LevelCreateCtrl level, bool isRestart = false, bool isClear = false)
    {
        fxNode.SetActive(false);

        //if (unlockLevel.Contains(levelId))
        //{
        //    nowUnlockNum = 1;
        //    unlockStep = unlockLevel.IndexOf(levelId);
        //}
        //else
        //{
        //    nowUnlockNum++;
        //}

        //if(unlockNum.Count > unlockStep)
        //{
        //    if (nowUnlockNum <= unlockNum[unlockStep])
        //    {
        //        level.unlockCake = unlockCake[unlockStep];
        //        level.unlockNow = nowUnlockNum;
        //        level.unlockNeed = unlockNum[unlockStep];
        //        if (nowUnlockNum == unlockNum[unlockStep])
        //        {
        //            unlockStep++;
        //        }
        //    }
        //}


        //level.
        levelType = 0;
        this.SendEvent<LevelStartEvent>();
        nowCakes.Clear();
        gameCtrl.FirstCake = null;
        gameCtrl.SecondCake = null;
        moveHistory.Clear();
        finishNum = 0;
        hideIdx = 0;
        hideNum = 0;
        CheckMoreCake(isRestart);
        levelBgFx.HideFx();

        if (levelId > 120)
        {
            levelBgSprite.sprite = levelBgs[levelBgs.Count - 1];
            if(levelBgs.Count <= 1)
            {
                levelBgFx.ShowFx();
            }
        }
        else
        {
            int levelBg = (levelId - 1) / 8;
            levelBgSprite.sprite = levelBgs[levelBg];

            if (levelBg == 0)
            {
                levelBgFx.ShowFx();
            }
        }

        foreach (var cake in cakes)
        {
            cake.OnCancelSelect();
            cake.ClearCake();
        }

        if(!isRestart)
        {
            SetShowCake();
        }
        else
        {
            SetShowCake(moreCakeNum);
        }



        StopCoroutine(WaitChange(level, isRestart));
        ChangeEnd = false;
        StartCoroutine(WaitChange(level, isRestart));
        
        if(isClear)
        {
            if (levelId > 5 && nowLevel.unlockNow == 1)
            {
                showUnlock = true;
                UIKit.OpenPanel<UICakes>(UILevel.Common, null, "uicakes_prefab", "UICakes");
            }
        }
    }

    void SetShowCake(int moreCake = 0)
    {
        var useCake = cakes;
        var useTrans = cakeTrans;
        if (levelId == 9999 && levelType == 0) ///第6关
        {
            useCake = sixCakes;
            useTrans = sixCakeTrans;
            sixLevelNode.SetActive(true);
            levelNode.SetActive(false);
        }
        else
        {
            sixLevelNode.SetActive(false);
            levelNode.SetActive(true);
        }
        tempCakes.Clear();

        int count = nowLevel.cakes.Count + moreCake;
        if (count > 12)
        {
            layout.spacing = 3.65f;
            for (int i = 0; i < useCake.Count; i++)
            {
                useTrans[i].sizeDelta = new Vector2(5, useTrans[i].rect.height);
                useTrans[i].localScale = new Vector3(0.8f, 0.8f, 0.8f);
                if (i < count)
                {
                    useCake[i].gameObject.SetActive(true);
                    tempCakes.Add(useCake[i]);
                }
                else
                {
                    useCake[i].gameObject.SetActive(false);
                }
            }

            int columns = (count - 1) / 4;

            for (int i = 0; i < 5; i++)
            {
                if (columns >= i)
                {
                    cakeNodes[i].gameObject.SetActive(true);
                }
                else
                {
                    cakeNodes[i].gameObject.SetActive(false);
                }
            }
        }
        else if(count > 9)
        {
            layout.spacing = 3.65f;
            for (int i = 0; i < useCake.Count; i++)
            {
                useTrans[i].sizeDelta = new Vector2(5.5f, useTrans[i].rect.height);
                useTrans[i].localScale = new Vector3(0.9f, 0.9f, 0.9f);
                if (i < count)
                {
                    useCake[i].gameObject.SetActive(true);
                    tempCakes.Add(useCake[i]);
                }
                else
                {
                    useCake[i].gameObject.SetActive(false);
                }
            }

            int columns = (count - 1) / 4;

            for (int i = 0; i < 5; i++)
            {
                if (columns >= i)
                {
                    cakeNodes[i].gameObject.SetActive(true);
                }
                else
                {
                    cakeNodes[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            layout.spacing = 4.75f;
            //if(count > 3 && count < 6)
            //{
            //    layout.spacing = 14;
            //}
            if (levelId == 9999 && levelType == 0)  ///第6关
            {
                for (int i = 0; i < 8; i++)
                {
                    useTrans[i].sizeDelta = new Vector2(7, useTrans[i].rect.height);
                    useTrans[i].localScale = new Vector3(1f, 1f, 1f);
                    if (i < 6 + moreCake)
                    {
                        useCake[i].gameObject.SetActive(true);
                        tempCakes.Add(useCake[i]);
                    }
                    else
                    {
                        useCake[i].gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                int showCake = 0;
                for (int i = 0; i < useCake.Count; i++)
                {
                    Debug.Log("蛋糕 " + i);
                    useTrans[i].sizeDelta = new Vector2(7, useTrans[i].rect.height);
                    useTrans[i].localScale = new Vector3(1f, 1f, 1f);
                    if (showCake < count && (i % 4 != 3))
                    {
                        useCake[i].gameObject.SetActive(true);
                        tempCakes.Add(useCake[i]);
                        showCake++;
                    }
                    else
                    {
                        useCake[i].gameObject.SetActive(false);
                    }
                }


                int columns = (count - 1) / 3;

                for (int i = 0; i < 5; i++)
                {
                    if (columns >= i)
                    {
                        cakeNodes[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        cakeNodes[i].gameObject.SetActive(false);
                    }
                }
            }
            
        }

        if(nowCakes.Count == 0) 
        {
            nowCakes = new List<CakeCtrl>(tempCakes);
        }
        else if(tempCakes.Count != nowCakes.Count)
        {
            for (int i = 0; i < tempCakes.Count; i++)
            {
                if(i < nowCakes.Count)
                {
                    tempCakes[i].hideList = nowCakes[i].hideList;
                    tempCakes[i].containCakeNum = nowCakes[i].containCakeNum;
                    tempCakes[i].cakeTypeList = nowCakes[i].cakeTypeList;
                    tempCakes[i].maxCakeNum = nowCakes[i].maxCakeNum;
                    tempCakes[i].histroyMove = nowCakes[i].histroyMove;
                    tempCakes[i].histroyBomb = nowCakes[i].histroyBomb;
                    tempCakes[i].lockType = nowCakes[i].lockType;
                    tempCakes[i].bombList = nowCakes[i].bombList;
                    tempCakes[i].showConnect = nowCakes[i].showConnect;
                    tempCakes[i].isSpecial = nowCakes[i].isSpecial;
                    List<int> connectMap = new List<int>();

                    for (int j = 0; j < nowCakes[i].connectOtherMap.Count; j++)
                    {
                        if (j >= nowCakes[i].connectOther.Count || nowCakes[i].connectOther[j] == null)
                        {
                            connectMap.Add(0);
                        }
                        else
                        {
                            connectMap.Add(nowCakes[i].connectOtherMap[j]);
                        }
                    }
                    tempCakes[i].connectOtherMap = connectMap;

                    tempCakes[i].RefreshCake();
                }
                else
                {
                    tempCakes[i].InitCake(nowLevel.cakes[0], true);
                }
            }
            nowCakes = new List<CakeCtrl>(tempCakes);
        }

        foreach(CakeCtrl c in nowCakes)
        {
            c.RefreshConnect();
        }
    }

    void InitShowCake(int moreCake = 0)
    {
        int count = nowLevel.cakes.Count + moreCake;
        var useCake = cakes;
        if (levelId == 9999 && levelType == 0)  ///第6关
        {
            useCake = sixCakes;
        }
        else
        {
        }

        if (count > 12)
        {
            for (int i = 0; i < useCake.Count; i++)
            {
                if (i < nowLevel.cakes.Count)
                {
                    useCake[i].InitCake(nowLevel.cakes[i]);
                }
                else
                {
                    useCake[i].InitCake(nowLevel.cakes[1], true);
                }
            }
        }
        else if (count > 9)
        {
            for (int i = 0; i < useCake.Count; i++)
            {
                if (i < nowLevel.cakes.Count)
                {
                    useCake[i].InitCake(nowLevel.cakes[i]);
                }
                else
                {
                    useCake[i].InitCake(nowLevel.cakes[1], true);
                }
            }
        }
        else
        {
            int showCake = 0;
            for (int i = 0; i < useCake.Count; i++)
            {
                if (levelId == 9999 && levelType == 0)  ///第6关
                {
                    if (i < 6 + moreCake)
                    {
                        useCake[i].InitCake(nowLevel.cakes[showCake]);
                        showCake++;
                    }
                    else
                    {
                        useCake[i].InitCake(nowLevel.cakes[1], true);
                    }
                }
                else
                {
                    if (showCake < nowLevel.cakes.Count && (i % 4 != 3))
                    {
                        useCake[i].InitCake(nowLevel.cakes[showCake]);
                        showCake++;
                    }
                    else if (i % 4 != 3)
                    {
                        useCake[i].InitCake(nowLevel.cakes[1], true);
                    }
                }
                    
            }
        }
    }


    IEnumerator WaitChange(LevelCreateCtrl level, bool isRestart = false)
    {
        yield return new WaitUntil(() =>
        {
            return ChangeEnd;
        });
        ChangeEnd = false;

        if(isRestart)
        {
            InitShowCake(moreCakeNum);
        }
        else
        {
            InitShowCake();
        }
        finishNum = 0;
        if (levelId == 1 && levelType == 0)
        {
            var e = new TeachEvent()
            {
                step = 1
            };
            this.SendEvent<TeachEvent>(e);
        }
    }

    public void RecordHistory(CakeCtrl cake1, CakeCtrl cake2)
    {
        int idx1 = nowCakes.IndexOf(cake1);
        int idx2 = nowCakes.IndexOf(cake2);
        moveHistory.Add(new KeyValuePair<int, int>(idx1, idx2));
        if(moveHistory.Count > 5)
        {
            moveHistory.RemoveAt(0);
        }
    }
    
    public void ReturnMove()
    {
        //if(NoAD)
        //{
        //    if (moveHistory.Count > 0)
        //    {
        //        var pair = moveHistory[moveHistory.Count - 1];
        //        moveHistory.RemoveAt(moveHistory.Count - 1);
        //        nowCakes[pair.Key].ReturnMove();
        //        nowCakes[pair.Value].ReturnMove();
        //        returnNum--;

        //        this.SendEvent<UpdateReturnEvent>();
        //    }
        //}
        //else
        //{
            if (moveHistory.Count > 0 && returnNum > 0)
            {
                var pair = moveHistory[moveHistory.Count - 1];
                moveHistory.RemoveAt(moveHistory.Count - 1);
                nowCakes[pair.Key].ReturnMove();
                nowCakes[pair.Value].ReturnMove();

                //Debug.Log("移动回退 " + nowCakes[pair.Value].gameObject.name + "->" + nowCakes[pair.Key].gameObject.name);

            returnNum--;

                this.SendEvent<UpdateReturnEvent>();
            }
            else
            {
                if(moveHistory.Count == 0)
                {
                    SendTip("Text_CantReturn");
                }
            }
        //}
       
    }

    public void SendTip(string showText)
    {
        TipEvent e = new TipEvent();
        e.tipStr = showText;
        this.SendEvent(e);
    }

    public void FinishCake()
    {
        Debug.Log("完成");
        finishNum++;

        if(finishNum == nowLevel.winCakeNum)
        {
            ClearLevel();
        }
    }

    public void UnlockMoreCake()
    {
        CancelAll();
        moreCakeNum++;
        CheckMoreCake();
    }

    public void ReduceMoveNum()
    {
        moreMoveNum += 7;
        this.SendEvent<MoveCakeEvent>();
    }

    public void SkipClear()
    {

        moreCakeNum = 0;
        levelId++;
        if (levelType == 0)
        {
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId);
        }
    }

    public void ClearLevel()
    {
        moreCakeNum = 0;
        levelId++;
        if (levelType == 0)
        {
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId);
        }
        //UIKit.OpenPanel<UIClear>(UILevel.Common, null, "uiclear_prefab", "UIClear");
        LevelManager.Instance.CheckStar();

        if ((levelId >= 8 && levelId <= 31) && !(levelId == 20 || levelId == 29 || levelId == 15))
        {
            StartCoroutine(OpenRank());
        }
        else
        {
            StartCoroutine(OpenUI());
        }
    }  

    public void ClearChallengeLevel()
    {
        UIKit.HidePanel<UIClear>();
        this.GetUtility<SaveDataUtility>().SaveChallenge(levelId - 1);
        this.SendEvent<ShowChallengeEvent>();
        this.SendEvent<LevelClearEvent>();
    }
    public void ClearJigsawLevel()
    {

        if ((levelId == 2 || levelId == 4))
        {
            TopOnADManager.Instance.ShowInterstitialAd();
        }
        UIKit.HidePanel<UIClear>();
        this.GetUtility<SaveDataUtility>().SaveJigsaw(levelId - 1);
        this.SendEvent<ShowJigsawEvent>();
        this.SendEvent<LevelClearEvent>();
    }

    public void ClearOrderLevel()
    {
        UIKit.HidePanel<UIClear>();
        this.GetUtility<SaveDataUtility>().SaveOrder(levelId);
        StartOrderGame(false, true);
        this.SendEvent<LevelClearEvent>();
    }

    IEnumerator OpenRank()
    {
        fxNode.SetActive(true);
        yield return new WaitForSeconds(2);
        UIKit.OpenPanel<UIRankClear>(UILevel.Common, null, "uirankclear_prefab", "UIRankClear");
    }

    IEnumerator OpenUI()
    {
        fxNode.SetActive(true);
        yield return new WaitForSeconds(2);
        UIKit.OpenPanel<UIClear>(UILevel.Common, null, "uiclear_prefab", "UIClear");

    }

    IEnumerator CheckChange()
    {
        while (levelId < levels.Count)
        {
            ClearLevel();
            yield return new WaitForSeconds(2.1f);
            WaitStart();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            ReStart();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShareManager.Instance.ShareScreen();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            ReturnMove();
        }

        //if(Input.GetKeyDown(KeyCode.F2))
        //{
        //    UnlockMoreCake();
        //}

        if (Input.GetKeyDown(KeyCode.F4))
        {
            //StartCoroutine(CheckChange());
            //this.SendEvent<ShowJigsawEvent>();
            //UIKit.OpenPanel<UIRank>(UILevel.Common);
            //UIKit.OpenPanel<UIRankClear>(UILevel.Common);
            ClearLevel();
        }
        //if(Input.GetMouseButton(0))
        //{
        //    Debug.Log("点击 ");
        //}
    }

    public void BackMove()
    {

    }

    public void CheckMoreCake(bool isRestart = false)
    {
        int checkNum = moreCakeNum - 1;
        SetShowCake(moreCakeNum);
        // cakes[nowLevel.cakes.Count + checkNum].gameObject.SetActive(true);
        //cakes[nowLevel.cakes.Count + checkNum].InitCake(nowLevel.cakes[0], true);
        StartCoroutine(WaitPosition());

    }

    IEnumerator WaitPosition()
    {
        yield return new WaitForEndOfFrame();
        foreach (var item in nowCakes)
        {
            item.ReSetPosition();
        }
    }

    public void WaitStart()
    {
        //yield return new WaitForSeconds(1);
        //yield return new WaitForEndOfFrame();

        //moreCakeNum = 0;
        //levelId++;
        if (levelId == 6)
        {
            UIKit.OpenPanel<UIJudge>(UILevel.Common, null, "uijudge_prefab", "UIJudge");
        }

        if(levelId == 7)
        {
            this.GetUtility<SaveDataUtility>().SetRankTime();
        }
        StartGame(levelId, false, true);
        UIKit.HidePanel<UIClear>();
    }

    public void CheckStar()
    {
        if(nowLevel.Star3 != 0)
        {
            if(moveNum < nowLevel.Star3 + moreMoveNum)
            {
                lastStar = 3;
            }
            else if(moveNum < nowLevel.Star2 + moreMoveNum)
            {
                lastStar = 2;
            }
            else
            {
                lastStar = 1;
            }
            this.GetUtility<SaveDataUtility>().SaveStarLevel(levelId - 1, lastStar);
        }
    }

    public void CancelAll()
    {
        if(gameCtrl.FirstCake != null)
        {
            gameCtrl.FirstCake.OnCancelSelect();
        }
    }

    public int GetStarNum()
    {
        int star = 0;
        
        for(int i = 7; i < 31; i++)
        {
            star += this.GetUtility<SaveDataUtility>().GetStarLevel(i); 
        }

        star += this.GetUtility<SaveDataUtility>().GetOrAddMoreStar();
        return star;
    }

    public int GetRankIdx(int star)
    {
        int rank = 0;
        if (star > 34)
        {
            rank = 1;
        }
        else if (star > 33)
        {
            rank = 8;
        }
        else if (star > 32)
        {
            rank = 16;
        }
        else if (star > 31)
        {
            rank = 40;
        }
        else if (star > 30)
        {
            rank = 66;
        }
        else if (star > 29)
        {
            rank = 88;
        }
        else if (star > 28)
        {
            rank = 112;
        }
        else if (star > 27)
        {
            rank = 134;
        }
        else if (star > 26)
        {
            rank = 157;
        }
        else if (star > 25)
        {
            rank = 176;
        }
        else if (star > 24)
        {
            rank = 193;
        }
        else if (star > 23)
        {
            rank = 210;
        }
        else if (star > 22)
        {
            rank = 234;
        }
        else if (star > 21)
        {
            rank = 256;
        }
        else if (star > 20)
        {
            rank = 288;
        }
        else if (star > 19)
        {
            rank = 314;
        }
        else if (star > 18)
        {
            rank = 334;
        }
        else if (star > 17)
        {
            rank = 356;
        }
        else if (star > 16)
        {
            rank = 378;
        }
        else if (star > 15)
        {
            rank = 399;
        }
        else if (star > 14)
        {
            rank = 422;
        }
        else if (star > 13)
        {
            rank = 440;
        }
        else if (star > 12)
        {
            rank = 457;
        }
        else if (star > 11)
        {
            rank = 473;
        }
        else if (star > 10)
        {
            rank = 489;
        }
        else if (star > 9)
        {
            rank = 504;
        }
        else if (star > 8)
        {
            rank = 520;
        }
        else if (star > 7)
        {
            rank = 538;
        }
        else if (star > 6)
        {
            rank = 552;
        }
        else if (star > 5)
        {
            rank = 568;
        }
        else if (star > 4)
        {
            rank = 585;
        }
        else if (star > 3)
        {
            rank = 600;
        }
        else if (star > 2)
        {
            rank = 617;
        }
        else if (star > 1)
        {
            rank = 633;
        }
        else
        {
            rank = 668;
        }

        return rank;
    }

    public void Defeat()
    {
        Debug.Log("失败");
        if(!isOpenDefeat)
        {
            isOpenDefeat = true;
            StartCoroutine(WaitDefeat());
        }
    }

    IEnumerator WaitDefeat()
    {

        yield return new WaitForSeconds(0.5f);
        UIKit.OpenPanel<UIClear>(UILevel.Common, null, "uidefeat_prefab", "UIDefeat");
        isOpenDefeat = false;
    }

    public void CheckHide()
    {
        if(nowLevel.stepHide)
        {
            //if(hideNum == nowLevel.)
            var hideCake = nowLevel.hideList[hideIdx];
            if(hideNum == nowLevel.hideNum[hideIdx] - 1)
            {
                hideIdx++;
            }

            foreach (var item in nowCakes)
            {
                for(int i = 0; i < item.cakeTypeList.Count; i++)
                {
                    if (!item.hideList[i] && item.cakeTypeList[i] == hideCake)
                    {
                        item.hideList[i] = true;
                        hideNum++;
                        item.UpdateTopColorValues();

                        return;
                    }
                }
            }
        }
    }
}
