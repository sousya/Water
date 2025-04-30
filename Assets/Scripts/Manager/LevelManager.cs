using QFramework;
using System.Collections.Generic;
using UnityEngine;
using GameDefine;
using System;
using Google.Play.Review;
using QFramework.Example;
using static LevelCreateCtrl;
using System.Collections;
using Spine.Unity;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoBehaviour, ICanSendEvent, ICanGetUtility, ICanRegisterEvent
{
    private ResLoader mResLoader = ResLoader.Allocate();
    public bool ignoreAnim = false;
    public static LevelManager Instance;
    public ReviewManager _reviewManager;
    public List<GameObject> bottleNodes = new List<GameObject>();
    public List<LevelCreateCtrl> levels = new List<LevelCreateCtrl>();
    public List<int> clearList = new List<int>();
    //带有阻碍的颜色(魔法布，藤曼，冰冻)
    public List<int> cantClearColorList = new List<int>();
    //带有阻碍的颜色(魔法布，藤曼，冰冻)
    public List<int> cantChangeColorList = new List<int>();
    public List<BottleCtrl> nowBottles = new List<BottleCtrl>();

    public List<BottleCtrl> iceBottles = new List<BottleCtrl>();
    public List<BottleCtrl> bottles = new List<BottleCtrl>();
    public List<BottleCtrl> topBottle = new List<BottleCtrl>();
    public List<BottleCtrl> bottomBottle = new List<BottleCtrl>();
    public List<BottleCtrl> hideBottleList = new List<BottleCtrl>();

    public List<int> hideColor = new List<int>();
    //水块颜色
    public List<Color> waterColor = new List<Color>();
    public List<Sprite> waterTopSp;
    public List<Sprite> waterSp;
    public int VictoryBottle;
    public BottleProperty emptyBottle = new BottleProperty();
    public Transform gameCanvas;
    List<ChangePair> changeList;
    public List<GameObject> createFx = new List<GameObject>();
    public LevelCreateCtrl nowLevel;
    public Color ItemColor;

    public Material shineMaterial;// 材质
    public float speed = 1.0f; // 光带移动速度

    [SerializeField]
    SpriteRenderer levelBgSprite;

    public int levelId = 1, moreCakeNum = 0, lastStar, moveNum, moreMoveNum, bombMaxNum, countDownNum, nowItem;
    public float timeCountDown, timeNow, test;
    public GameObject mahoujinGo, broomBullet, fireRuneBullet;
    public SkeletonGraphic mahoujinSpine;
    bool isOpenDefeat = false, isBomb = false, isCountDown = false, isTimeCountDown = false;
    public bool isPlayAnim, isPlayFxAnim, isSelectItem;
    public BottleCtrl nowHalf;
    [SerializeField]
    GameCtrl gameCtrl;

    public List<Sprite> scene1, scene2, scene3, scene4;
    public List<string> scenePartName1, scenePartName2, scenePartName3, scenePartName4;
    public List<int> needScene1, needScene2, needScene3, needScene4;
    public List<int> needScenePart1, needScenePart2, needScenePart3, needScenePart4;
    public Material selectMaterial;
    public GameObject hideBg;
    //携带的道具
    public List<int> takeItem = new List<int>();

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
        //this.GetUtility<SaveDataUtility>().SaveLevel(6);
        //PlayerPrefs.DeleteKey("g_WaterSceneRecord");
        //PlayerPrefs.DeleteKey("g_WaterScenePartRecord");
        //PlayerPrefs.DeleteKey("g_WaterSceneBoxRecord");
        //this.GetUtility<SaveDataUtility>().SetCoinNum(0);
        //this.GetUtility<SaveDataUtility>().SetSceneRecord(1);

        //清空携带道具
        StringEventSystem.Global.Register("ClearTakeItem", () =>
        {
            ClearTakeItem();

        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        this.GetUtility<SaveDataUtility>().SaveLevel(11);

        emptyBottle.numCake = 4;
        levelId = this.GetUtility<SaveDataUtility>().GetLevelClear();

        //var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
        if (levelId <= 5)
        {
            StartGame(levelId);
        }

        UIKit.OpenPanel<UIBegin>();

    }

    /// <summary>
    /// 清空携带道具
    /// </summary>
    void ClearTakeItem()
    {
        takeItem.Clear();
    }

    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="fromTarget"></param>
    public void UseItem(int itemId, Transform fromTarget)
    {
        //记录触发位置
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
                    if (pair.item == ItemType.ChangeGreen)
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
                    if (pair.item == ItemType.ChangeOrange)
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
                    if (pair.item == ItemType.ChangePink)
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
                    if (pair.item == ItemType.ChangeYellow)
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
                    if (pair.item == ItemType.ChangePurple)
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
                    if (pair.item == ItemType.ChangeDarkBlue)
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

    /// <summary>
    /// 添加颜色
    /// </summary>
    /// <param name="fromPos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 合成完毕逻辑
    /// </summary>
    /// <param name="clearColor"></param>
    /// <param name="botterIdx"></param>
    public void FinishClear(int clearColor, int botterIdx)
    {
        //Debug.Log($"颜色编号：{clearColor}");
        //Debug.Log($"idx：{botterIdx}");
        foreach (var item in nowBottles)
        {
            item.CheckUnlockHide(clearColor);
            item.CheckNearHide(botterIdx);
        }

        StopCoroutine(WaitCheckFinish());
        StartCoroutine(WaitCheckFinish(clearColor));
    }

    /// <summary>
    /// 胜利后&胜利动画后逻辑处理
    /// </summary>
    /// <param name="clearColor"></param>
    /// <returns></returns>
    public IEnumerator WaitCheckFinish(int clearColor = 0)
    {
        // 通过逻辑重构
        if (clearColor != 0 && clearList.Count > 0)
            clearList.Remove(clearColor);

        if (clearList.Count == 0)
        {
            //Debug.Log("胜利");
            UIKit.OpenPanel<UIMask>(UILevel.PopUI);
            yield return new WaitForSeconds(4);

            //Debug.Log("开始播放胜利结算");
            UIKit.ClosePanel<UIMask>();
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId + 1);
            CoinManager.Instance.AddCoin(20);

            //前五关(前五关应该不统计连胜)
            if (levelId < 5)
            {
                this.SendEvent<LevelStartEvent>();
                StartGame(levelId + 1);
            }
            else
            {
                UIKit.OpenPanel<UIVictory>();
            }
            CheckWinNum();
        }
        else
            yield return null;

        // obsolete
        /*if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            Debug.Log("胜利");
        }

        if (clearColor != 0 && clearList.Count > 1)
        {
            clearList.Remove(clearColor);
        }
        yield return new WaitForSeconds(4);

        if (clearColor != 0 && clearList.Count == 1)
        {
            clearList.Remove(clearColor);
        }
        if (clearList.Count == 0)
        {
            //VictoryBottle = idx;
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId + 1);
            CoinManager.Instance.AddCoin(20);

            //前五关(前五关应该不统计连胜)
            if (levelId < 5)
            {
                //CheckWinNum();
                this.SendEvent<LevelStartEvent>();
                StartGame(levelId + 1);
            }
            else
            {
                UIKit.OpenPanel<UIVictory>();
            }
            CheckWinNum();
        }*/
    }

    /// <summary>
    /// 连胜判断
    /// </summary>
    public void CheckWinNum()
    {
        var winNum = this.GetUtility<SaveDataUtility>().GetCountinueWinNum();
        winNum++;
        
        if (winNum < 3)
            this.GetUtility<SaveDataUtility>().SetCountinueWinNum(winNum);
        else
            this.GetUtility<SaveDataUtility>().SetCountinueWinNum(3);

    }

    /// <summary>
    /// 判断能加色的瓶子
    /// </summary>
    /// <returns></returns>
    public List<BottleCtrl> GetMakeColorBottle()
    {
        List<BottleCtrl> ret = new List<BottleCtrl>();
        foreach (var bottle in nowBottles)
        {
            if (!bottle.isFreeze && bottle.waters.Count < 4 && !bottle.isClearHide && !bottle.isNearHide)
            {
                ret.Add(bottle);
            }
        }
        return ret;
    }

    /// <summary>
    /// 变色
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="target"></param>
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

    /// <summary>
    /// 判断能清除的颜色
    /// </summary>
    /// <returns></returns>
    public List<int> CheckCanClearList()
    {
        List<int> ret = new List<int>();
        foreach (var color in clearList)
        {
            //if (!cantClearColorList.Contains(color))
            if (!cantChangeColorList.Contains(color))
            {
                ret.Add(color);
            }
        }
        return ret;
    }

    /// <summary>
    /// 判断瓶子完成后的消除逻辑
    /// </summary>
    /// <param name="color"></param>
    public void CheckFinishChange(int color)
    {
        foreach (var bottle in nowBottles)
        {
            bottle.CheckUnlockHide(color);
        }
    }

    /// <summary>
    /// 变色动画以及动画后的逻辑
    /// </summary>
    /// <param name="color"></param>
    /// <param name="fromPos"></param>
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

    /// <summary>
    /// 破冰
    /// </summary>
    /// <returns></returns>
    public BottleWaterCtrl BreakIce()
    {
        int iceIdx = UnityEngine.Random.Range(0, iceBottles.Count);

        var bottle = iceBottles[iceIdx];
        iceBottles.RemoveAt(iceIdx);
        return bottle.FindIceWater();
    }

    /// <summary>
    /// 清除炸弹
    /// </summary>
    public void CancelBomb()
    {
        isBomb = false;
    }

    /// <summary>
    /// 移动步数记录
    /// </summary>
    public void AddMoveNum()
    {
        moveNum++;
        if ((moveNum >= bombMaxNum && isBomb) || (isCountDown && moveNum >= countDownNum))
        {
            OnDefeat();
        }
    }


    /// <summary>
    /// 使用道具添加瓶子(单个或整瓶)
    /// </summary>
    /// <param name="isHalf"></param>
    public void AddBottle(bool isHalf ,Action action)
    {
        if (nowBottles.Count < (topBottle.Count + bottomBottle.Count) || nowHalf != null)
        {
            UseItemAddBottle();
            MoveAndAddBottle(isHalf , action);
        }
    }

    /// <summary>
    /// 增加瓶子
    /// </summary>
    public void UseItemAddBottle()
    {
        if (nowBottles.Count < (topBottle.Count + bottomBottle.Count))
        {
            if (nowHalf != null && nowHalf.maxNum != 4)
            {
                //Debug.Log("nowHalf not null");
                return;
            }

            int topAc = 0;
            int bomAc = 0;

            foreach (var item in topBottle)
            {
                if (item.gameObject.activeSelf)
                {
                    ++topAc;
                }
            }

            foreach (var item in bottomBottle)
            {
                if (item.gameObject.activeSelf)
                {
                    ++bomAc;
                }
            }

            //Debug.Log($"上排激活了{topAc}");
            //Debug.Log($"下排激活了{bomAc}");

            if (topAc > bomAc)
            {
                //索引刚好对应下一个要激活的瓶子
                bottomBottle[bomAc].Show();
                nowBottles.Add(bottomBottle[bomAc]);
            }
            else
            {
                topBottle[topAc].Show();
                nowBottles.Add(topBottle[topAc]);
            }
        }
    }

    #region 关卡重置初始化/进入关卡初始化

    /// <summary>
    /// 开始游戏&初始化
    /// </summary>
    /// <param name="id"></param>
    public void StartGame(int id)
    {
        //moreBottle = 0;
        cantClearColorList.Clear();
        cantChangeColorList.Clear();
        hideBottleList.Clear();
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
        if (countDownNum > 0)
        {
            isCountDown = true;
        }
        if (timeCountDown > 0)
        {
            isTimeCountDown = true;
        }

        clearList = new List<int>(levelInfo.clearList);
        hideColor = new List<int>(levelInfo.hideList);
        nowBottles.Clear();

        nowHalf = null;
        InitLevels(levelInfo);

        this.SendEvent<LevelStartEvent>();
    }

    /// <summary>
    /// 关卡重置初始化/进入关卡初始化
    /// </summary>
    public void InitLevels(LevelCreateCtrl levelInfo)
    {
        //清空操作记录 
        foreach (var bottle in bottles)
        {
            bottle.moveRecords.Clear();
        }

        //Debug.Log("关卡重置初始化/首次进入关卡初始化");
        ShowBottleGo();
        InitBottle(levelInfo);

        //当前有连胜，去黑水瓶生效
        int WinNum = this.GetUtility<SaveDataUtility>().GetCountinueWinNum();
        //Debug.Log("当前连胜次数:" + WinNum);
        if (WinNum > 0)
            StringEventSystem.Global.Send("StreakWinItem", WinNum);
    }

    /// <summary>
    /// 判断显示那些瓶子（现用于初始化关卡的瓶子）
    /// </summary>
    /// <param name="userItemSign"></param>
    /// public void ShowBottleGo(int num)
    public void ShowBottleGo()
    {
        nowBottles.Clear();

        for (int i = 0; i < topBottle.Count; i++)
        {
            var useBottle = topBottle[i];
            var num = nowLevel.topNum;
            useBottle.gameObject.SetActive(i < num);
            if (i < num)
                nowBottles.Add(useBottle);
        }

        for (int i = 0; i < bottomBottle.Count; i++)
        {
            var useBottle = bottomBottle[i];
            var num = nowLevel.bottomNum;
            useBottle.gameObject.SetActive(i < num);
            if (i < num)
                nowBottles.Add(useBottle);
        }
    }

    /// <summary>
    /// 根据数据初始化瓶子
    /// </summary>
    /// <param name="levelInfo"></param>
    public void InitBottle(LevelCreateCtrl levelInfo)
    {

        for (int i = 0; i < levelInfo.bottles.Count; i++)
        {
            var bottle = nowBottles[i];
            bottle.Init(levelInfo.bottles[i], i);
            bottle.maxNum = 4;
        }
    }

    /// <summary>
    /// 重置关卡
    /// </summary>
    public void RefreshLevel()
    {
        //Debug.Log("重置关卡");
        clearList = new List<int>(nowLevel.clearList);
        hideColor = new List<int>(nowLevel.hideList);
        changeList = new List<ChangePair>(nowLevel.changeList);
        hideBottleList.Clear();
        cantClearColorList.Clear();

        nowHalf = null;
        InitLevels(nowLevel);

        //会触发两次重置(事件调用了StartGame，里面调用了InitLevels)，
        //如果后续有问题，直接在这调用StartGame做那些数据处理
        //this.SendEvent<GameStartEvent>();

        GameCtrl.Instance.InitGameCtrl();
    }

    #endregion

    /// <summary>
    /// 添加瓶子并移动瓶子数据
    /// </summary>
    /// <param name="isHalf"></param>
    /// <param name="action"><扣除道具的回调/param>
    public void MoveAndAddBottle(bool isHalf ,Action action)
    {
        var num = nowBottles.Count;
        //测试结果---无用
        /*        for (int i = 0; i < tempBottles.Count; i++)
                {
                    nowBottles[i].MoveBottle(tempBottles[i]);
                }*/

        //初始化瓶子-更新索引
        if (isHalf)
        {
            if (nowHalf == null || nowHalf.maxNum == 4)
            {
                nowBottles[nowBottles.Count - 1].Init(emptyBottle, nowBottles.Count - 1);
                nowHalf = nowBottles[nowBottles.Count - 1];
                nowBottles[nowBottles.Count - 1].maxNum = 1;
            }
            else
            {
                nowBottles[nowBottles.Count - 1].maxNum++;
                if (nowBottles[nowBottles.Count - 1].maxNum == 4)
                    nowHalf = null;
            }
        }
        else    //整瓶
        {
            nowBottles[nowBottles.Count - 1].Init(emptyBottle, nowBottles.Count - 1);
            nowBottles[nowBottles.Count - 1].maxNum = 4;
            nowHalf = null;
        }

        action?.Invoke();
        nowBottles[nowBottles.Count - 1].SetMaxBottle();
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
            BeginSelectItem(1);
        }
    }

    /// <summary>
    /// 失败后逻辑
    /// </summary>
    public void OnDefeat()
    {
        Debug.Log("失败");
    }

    /// <summary>
    /// 倒计时
    /// </summary>
    public void TimeCountDown()
    {
        if (isTimeCountDown)
        {
            timeNow += Time.deltaTime;
            if (timeNow >= timeCountDown)
            {
                OnDefeat();
                isTimeCountDown = false;
            }
        }

        #region obsolete
        /*test += Time.deltaTime;
        if (test >= 1)
        {
            test = 0;
            CheckVitality();
            //Debug.Log("时间 " + this.GetUtility<SaveDataUtility>().GetVitalityNum()  + " " + this.GetUtility<SaveDataUtility>().GetVitalityTime());
        }*/
        #endregion  

    }

    #region obsolete 
    /*/// <summary>
    /// 判断体力
    /// </summary>
    void CheckVitality()
    {
        //获取当前体力
        int lastVitalityNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();

        if (lastVitalityNum < 5)
        {
            // 计算完全恢复体力所需的时间点
            long recoveryTime = this.GetUtility<SaveDataUtility>().GetVitalityTime() + (5 - lastVitalityNum) * GameConst.RecoveryTime;
            //Debug.Log($"上次体力恢复满的时候 ： {this.GetUtility<SaveDataUtility>().GetVitalityTime()}");

            // 计算当前时间与完全恢复时间的时间差
            long timeOffset = recoveryTime - this.GetUtility<SaveDataUtility>().GetNowTime();
            //Debug.Log("体力 " + lastVitalityNum + " " + timeOffset);
            // 如果时间差大于0，说明体力尚未完全恢复
            if (timeOffset > 0)
            {
                // 计算从上次记录的恢复时间到现在的时间差
                long checkTime = this.GetUtility<SaveDataUtility>().GetNowTime() - this.GetUtility<SaveDataUtility>().GetVitalityTime();
                // 根据时间差计算可以恢复的体力数量
                int addNum = Mathf.FloorToInt((float)checkTime / GameConst.RecoveryTime);

                //Debug.Log("体力 " + addNum + " " + timeOffset);
                // 如果恢复的体力数量超过最大体力值，则直接设置为最大体力值
                if (addNum > GameConst.MaxVitality)
                {
                    addNum = GameConst.MaxVitality;
                    this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
                }
                // 如果恢复的体力数量大于等于1，则更新体力值和恢复时间
                else if (addNum >= 1)
                {
                    this.GetUtility<SaveDataUtility>().SetVitality(lastVitalityNum + addNum, (this.GetUtility<SaveDataUtility>().GetVitalityTime() + (addNum) * GameConst.RecoveryTime) + "");
                }
            }
            // 如果时间差小于等于0，说明体力已经完全恢复
            else
            {
                this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
            }
            // 发送体力时间变化事件，通知其他模块体力恢复的时间差
            this.SendEvent<VitalityTimeChangeEvent>(new VitalityTimeChangeEvent() { timeOffset = timeOffset });
        }
    }*/
    #endregion


    /// <summary>
    /// 记录所有瓶子
    /// </summary>
    public void RecordLast()
    {
        foreach (var bottle in nowBottles)
        {
            bottle.RecordLast();
        }
    }

    /// <summary>
    /// 返回上一步
    /// </summary>
    /// <returns>是否能回退</returns>
    public bool ReturnLast()
    {
        bool ret = false;
        foreach (var bottle in nowBottles)
        {
            var needRet = bottle.ReturnLast();
            ret = ret || needRet;
        }
        return ret;
    }

    /// <summary>
    /// 清除所有附加道具
    /// </summary>
    public void RemoveAll()
    {
        foreach (var bottle in nowBottles)
        {
            bottle.SetNormal();
        }
    }

    /// <summary>
    /// 判断是否有阻碍效果
    /// </summary>
    /// <returns></returns>
    public bool CheckAllDebuff()
    {
        //是否有黑水
        if (hideBottleList.Count != 0)
        {
            return true;
        }

        foreach (var bottle in nowBottles)
        {
            //是否有阻碍效果
            if (bottle.isFreeze || bottle.limitColor != 0 || bottle.isNearHide || bottle.isClearHide)
            {
                return true;
            }

            foreach (var item in bottle.waterItems)
            {
                //是否有冰冻，荆棘等
                if (item != WaterItem.None)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 清除所有黑水
    /// </summary>
    /// <param name="num"></param>
    /// <param name="action">使用道具回调</param>
    public void RemoveHide(Action action , int num = 0)
    {
        if (num == 0)
        {
            foreach (var bottle in nowBottles)
            {
                bottle.RemovHide();
            }

        }
        else
        {

            for (int i = 0; i < num; i++)
            {
                nowBottles[i].RemovHide();
            }
        }

        action?.Invoke();
    }
     
    /// <summary>
    /// 魔法阵动画
    /// </summary>
    public void ShowMahoujin()
    {
        //触发遮罩
        UIKit.OpenPanel<UIMask>(UILevel.PopUI);
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
        UIKit.ClosePanel<UIMask>();
        //Debug.Log("去除遮罩");
        RemoveAll();
        mahoujinGo.SetActive(false);
        isPlayFxAnim = false;
    }

    /// <summary>
    /// 判断需要多少星星解锁场景部件(已使用多少星星数)
    /// </summary>
    /// <param name="scene">场景编号</param>
    /// <param name="num">当前场景部件编号</param>
    /// <returns></returns>
    public int GetUnlockNeedStar(int scene, int num)
    {
        List<int> useList = null;
        switch (scene)
        {
            case 1:
                useList = needScene1;
                break;
            case 2:
                useList = needScene2;
                break;
            case 3:
                useList = needScene3;
                break;
            case 4:
                useList = needScene4;
                break;
        }

        if (num == 0)
        {
            if (scene == 1)
            {
                return 0;
            }
            else
            {
                switch (scene)
                {
                    case 2:
                        useList = needScene1;
                        break;
                    case 3:
                        useList = needScene2;
                        break;
                    case 4:
                        useList = needScene3;
                        break;
                }
                return useList[4];

            }
        }
        return useList[num - 1];
    }

    /// <summary>
    /// 判断下一场景需要多少星星解锁场景第一部件(判断需要多少星星解锁下一部件)
    /// </summary>
    /// <param name="scene">当前场景编号</param>
    /// <param name="num">当前部件编号</param>
    /// <returns></returns>
    public int GetPartNeedStar(int scene, int num)
    {
        List<int> useList = null;
        switch (scene)
        {
            case 1:
                useList = needScenePart1;
                break;
            case 2:
                useList = needScenePart2;
                break;
            case 3:
                useList = needScenePart3;
                break;
            case 4:
                useList = needScenePart4;
                break;
        }
        return num == 0 ? useList[0] : useList[num - 1];
        //return useList[num - 1];//有时会传入0
    }

    /// <summary>
    /// 解锁场景部件
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="num"></param>
    public void UnlockScene(int scene, int num)
    {
        UnlockSceneEvent e = new UnlockSceneEvent();
        e.scene = scene;
        e.part = num;

        this.GetUtility<SaveDataUtility>().SetSceneRecord(scene);
        this.GetUtility<SaveDataUtility>().SetScenePartRecord(num);

        //需要先保存数据，然后在发送事件，因为后面的逻辑需要更新UI，重新读取数据，
        this.SendEvent<UnlockSceneEvent>(e);

        //在发送完事件之后，处理完解锁动画更新数据
        if (num == 5)
            this.GetUtility<SaveDataUtility>().SetSceneRecord(scene + 1);
    }

    /// <summary>
    /// 道具选择
    /// </summary>
    public void ShowItemSelect()
    {
        hideBg.SetActive(true);
        for (int i = 0; i < nowBottles.Count; i++)
        {
            nowBottles[i].ShowItemSelect();
        }
    }

    /// <summary>
    /// 隐藏道具选择
    /// </summary>
    public void HideItemSelect()
    {
        hideBg.SetActive(false);
        for (int i = 0; i < nowBottles.Count; i++)
        {
            nowBottles[i].HideItemSelect();
        }
    }

    /// <summary>
    /// 使用选择的道具
    /// </summary>
    /// <param name="bottleCtrl"></param>
    public void UseSelectItem(BottleCtrl bottleCtrl)
    {
        HideItemSelect();
        LevelManager.Instance.isSelectItem = false;

        switch (nowItem)
        {
            case 1:
                bottleCtrl.RandomWater();
                break;
        }
    }

    /// <summary>
    /// 开始使用道具选择瓶子
    /// </summary>
    /// <param name="item"></param>
    public void BeginSelectItem(int item)
    {
        nowItem = item;
        ShowItemSelect();
        LevelManager.Instance.isSelectItem = true;
    }

    public void CancelSelectItem()
    {
        LevelManager.Instance.isSelectItem = false;
    }
}

[Serializable]
public class MovePair
{
    public int from;
    public int to;
}

[Serializable]
public class BottleRecord
{
    public bool isFinish, isFreeze, isClearHide, isNearHide;
    public List<int> waters = new List<int>();
    public List<bool> hideWaters = new List<bool>();
    public List<WaterItem> waterItems = new List<WaterItem>();
}