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
using System.Linq;

[MonoSingletonPath("[Level]/LevelManager")]
public class LevelManager : MonoBehaviour,IController, ICanSendEvent
{
    public static LevelManager Instance;
    public ReviewManager _reviewManager;
    //public List<GameObject> bottleNodes = new List<GameObject>();
    public List<LevelCreateCtrl> levels = new List<LevelCreateCtrl>();
    public List<int> clearList = new List<int>();
    //带有阻碍的颜色(魔法布，藤曼，冰冻)
    //public List<int> cantClearColorList = new List<int>();
    public List<int> cantChangeColorList = new List<int>();
    public List<BottleCtrl> nowBottles = new List<BottleCtrl>();

    public List<BottleCtrl> iceBottles = new List<BottleCtrl>();
    [SerializeField] private Transform BottomBottleNode;
    [SerializeField] private List<BottleCtrl> bottles = new List<BottleCtrl>();
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
    public List<ChangePair> changeList;
    public List<GameObject> createFx = new List<GameObject>();
    public LevelCreateCtrl nowLevel;
    public Color ItemColor;

    public Material shineMaterial;// 材质
    //public float speed = 3.0f; // 光带移动速度

    public int levelId = 1, bombMaxNum, countDownNum, playingHideAnimCount;
    //public int moveNum;//步数统计,暂无用
    public float timeCountDown, timeNow;
    public GameObject mahoujinGo, broomBullet;
    public SkeletonGraphic mahoujinSpine;
    bool isFinish = false, isBomb = false, isCountDown = false, isTimeCountDown = false;
    public bool isPlayAnim, isPlayFxAnim;
    public bool ISPlayingHideAnim => playingHideAnimCount == 0;//playingHideAnimCount 播放计数
    public BottleCtrl nowHalf;

    public List<SceneUnLockSO> SceneUnLockSOs;

    public Material selectMaterial;
    public GameObject hideBg;
    //携带的道具
    public List<int> takeItem = new List<int>();
    public List<LevelManagerRecord> LevelManagerRecords = new List<LevelManagerRecord>();

    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }

    private StageModel stageModel;

    private void Awake()
    {
        Instance = this;
        _reviewManager = new ReviewManager();
        stageModel = this.GetModel<StageModel>();

        InitBottle();
    }

    private void Start()
    {
        //UIKit.OpenPanel<UIBegin>(UILevel.Common);
        AudioKit.PlayMusic("resources://Audio/BG_BGM");

        //清空携带道具
        StringEventSystem.Global.Register("ClearTakeItem", () =>
        {
            takeItem.Clear();

        }).UnRegisterWhenGameObjectDestroyed(gameObject);

        emptyBottle.numCake = 4;
        levelId = this.GetUtility<SaveDataUtility>().GetLevelClear();

        if (levelId <= 5)
        {
            StartGame(levelId);
            UIKit.OpenPanel<UIGameNode>();
        }
        else
            UIKit.OpenPanel<UIBegin>();

    }

    /// <summary>
    /// 将瓶子数据初始化
    /// 初始化时调用/游戏结束时调用/退出关卡调用
    /// </summary>
    public void InitBottle()
    {
        foreach (var item in bottles)
        {
            item.Init(emptyBottle, 0);
        }
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

            ////////改变色块
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

            ////////清除色块
            case ItemType.ClearPink:
                ClearColor(3, fromPos);
                break;
            case ItemType.ClearOrange:
                ClearColor(7, fromPos);
                break;
            case ItemType.ClearBlue:
                ClearColor(4, fromPos);
                break;
            case ItemType.ClearYellow:
                ClearColor(6, fromPos);
                break;
            case ItemType.ClearDarkGreen:
                ClearColor(9, fromPos);
                break;
            case ItemType.ClearRed:
                ClearColor(2, fromPos);
                break;
            case ItemType.ClearGreen:
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
        AudioKit.PlaySound("resources://Audio/AddColor");

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
                //Debug.Log("添加颜色 " + addColor);
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

        UIKit.ClosePanel<UIMask>();
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
    /// 测试用方法
    /// </summary>
    public IEnumerator TestFinish()
    {
        isFinish = true;
        //Debug.Log("胜利");
        UIKit.OpenPanel<UIMask>(UILevel.PopUI);
        yield return new WaitForSeconds(1);

        //Debug.Log("开始播放胜利结算");
        AudioKit.PlaySound("resources://Audio/Victory");
        UIKit.ClosePanel<UIMask>();
        this.GetUtility<SaveDataUtility>().SaveLevel(levelId + 1);

        //前五关(前五关应该不统计连胜)
        if (levelId < 5)
            StartGame(levelId + 1);
        else
            UIKit.OpenPanel<UIVictory>();

        // 连胜计数
        stageModel.AddCountinueWinNum();
    }

    /// <summary>
    /// 胜利后&胜利动画后逻辑处理
    /// </summary>
    /// <param name="clearColor"></param>
    /// <returns></returns>
    public IEnumerator WaitCheckFinish(int clearColor = 0)
    {
        if (clearColor != 0 && clearList.Count > 0)
            clearList.Remove(clearColor);

        if (clearList.Count == 0 && !isFinish)
        {
            isFinish = true;
            //Debug.Log("胜利");
            UIKit.OpenPanel<UIMask>(UILevel.PopUI);
            yield return new WaitForSeconds(4);

            //Debug.Log("开始播放胜利结算");
            AudioKit.PlaySound("resources://Audio/Victory");
            UIKit.ClosePanel<UIMask>();
            this.GetUtility<SaveDataUtility>().SaveLevel(levelId + 1);

            //前五关(前五关应该不统计连胜)
            if (levelId < 5)
                StartGame(levelId + 1);
            else
                UIKit.OpenPanel<UIVictory>();

            // 连胜计数
            stageModel.AddCountinueWinNum();
        }
        else
            yield return null;
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
        AudioKit.PlaySound("resources://Audio/ChangeColor");
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
        UIKit.ClosePanel<UIMask>();
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
    /// 变色动画以及动画后的逻辑
    /// </summary>
    /// <param name="color"></param>
    /// <param name="fromPos"></param>
    public void ClearColor(int color, Vector3 fromPos)
    {
        StartCoroutine(ClearColorCoroutine(color, fromPos));
    }

    IEnumerator ClearColorCoroutine(int color, Vector3 fromPos)
    {
        isPlayFxAnim = true;
        yield return new WaitForSeconds(1f);
        AudioKit.PlaySound("resources://Audio/BroomBullet");

        foreach (var bottle in nowBottles)
        {
            bottle.PlayBroomBullet(color, fromPos);
        }

        yield return new WaitForSeconds(0.2f);

        if (clearList.Contains(color))
        {
            clearList.Remove(color);
        }

        //先获取要移除单色的瓶子列表(用于判断是否都在一个瓶子)
        List<BottleCtrl> removeColorBottles = new List<BottleCtrl>();

        foreach (var bottle in nowBottles)
        {
            var bottleCtrl = bottle.CheckRemoveOneColor(color);
            if (bottleCtrl != null && !removeColorBottles.Contains(bottleCtrl))
                removeColorBottles.Add(bottleCtrl);

            //bottle.RemoveAllOneColor(color);
            bottle.CheckUnlockHide(color);
        }

        foreach (var bottle in removeColorBottles)
        {
            bottle.RemoveAllOneColor(color, removeColorBottles.Count == 1);
        }

        StartCoroutine(WaitCheckFinish());
        UIKit.ClosePanel<UIMask>();
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

    /*/// <summary>
    /// 移动步数记录
    /// </summary>
    public void AddMoveNum()
    {
        moveNum++;
        if ((moveNum >= bombMaxNum && isBomb) || (isCountDown && moveNum >= countDownNum))
        {
            OnDefeat();
        }
    }*/

    #region Add Bottle

    /// <summary>
    /// 使用道具添加瓶子(单个或整瓶)
    /// </summary>
    /// <param name="isHalf"></param>
    public void AddBottle(bool isHalf, Action action)
    {
        //增加瓶子时一定会激活底部瓶子节点
        if (!BottomBottleNode.gameObject.activeSelf)
            BottomBottleNode.Show();

        // 半瓶道具逻辑
        if (isHalf)
        {
            if (nowHalf == null || nowHalf.maxNum == 4)
            {
                // 新开一个半瓶
                if (nowBottles.Count < (topBottle.Count + bottomBottle.Count))
                {
                    UseItemAddBottle();
                    MoveAndAddBottle(true, action);
                }
            }
            else
            {
                // 直接增加容量
                nowHalf.maxNum++;
                nowHalf.SetMaxBottle();
                if (nowHalf.maxNum == 4)
                    nowHalf = null;
                action?.Invoke();
            }
            return;
        }

        // 整瓶道具逻辑
        if (!isHalf)
        {
            if (nowHalf != null && nowHalf.maxNum < 4)
            {
                // 补满半瓶
                nowHalf.maxNum = 4;
                nowHalf.SetMaxBottle();
                nowHalf = null;
                action?.Invoke();
                return;
            }
            if (nowBottles.Count < (topBottle.Count + bottomBottle.Count))
            {
                UseItemAddBottle();
                MoveAndAddBottle(false, action);
            }
        }

        /*if (nowBottles.Count < (topBottle.Count + bottomBottle.Count) || nowHalf != null)
        {
            UseItemAddBottle();
            MoveAndAddBottle(isHalf, action);
        }*/
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

            int temp = 0;
            foreach (var item in topBottle)
            {
                if (item.gameObject.activeSelf)
                {
                    item.bottleIdx = temp;
                    ++temp;
                }
            }
            foreach (var item in bottomBottle)
            {
                if (item.gameObject.activeSelf)
                {
                    item.bottleIdx = temp;
                    ++temp;
                }
            }
        }
    }

    /// <summary>
    /// 添加瓶子并初始化瓶子数据
    /// </summary>
    /// <param name="isHalf"></param>
    /// <param name="action"><扣除道具的回调/param>
    public void MoveAndAddBottle(bool isHalf, Action action)
    {
        var num = nowBottles.Count;
        
        //初始化瓶子-更新索引
        if (isHalf)
        {
            if (nowHalf == null || nowHalf.maxNum == 4)
            {
                nowBottles[num - 1].Init(emptyBottle, nowBottles[num - 1].bottleIdx);
                nowHalf = nowBottles[num - 1];
                nowBottles[num - 1].maxNum = 1;
            }
            else
            {
                nowBottles[num - 1].maxNum++;
                if (nowBottles[num - 1].maxNum == 4)
                    nowHalf = null;
            }
        }
        //整瓶
        else
        {
            nowBottles[num - 1].Init(emptyBottle, nowBottles[num - 1].bottleIdx);
            nowBottles[num - 1].maxNum = 4;
            nowHalf = null;
        }
        action?.Invoke();
        nowBottles[num - 1].SetMaxBottle();

        //对瓶子列表重新排序(整个流程应该都是通过索引找瓶子，排序不改数据也不对)
        nowBottles = nowBottles.OrderBy(bottle => bottle.bottleIdx).ToList();
    }

    #endregion

    #region 关卡重置初始化/进入关卡初始化

    /// <summary>
    /// 开始游戏&初始化
    /// </summary>
    /// <param name="id"></param>
    public void StartGame(int id)
    {
        //cantClearColorList.Clear();
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
    }

    /// <summary>
    /// 关卡重置初始化/进入关卡初始化
    /// </summary>
    public void InitLevels(LevelCreateCtrl levelInfo)
    {
        this.SendEvent<LevelStartEvent>();

        //清空操作记录 
        foreach (var bottle in bottles)
        {
            bottle.moveRecords.Clear();
        }
        LevelManagerRecords.Clear();

        GameCtrl.Instance.InitPouringCount();
        //重置魔法布统计
        playingHideAnimCount = 0;
        isFinish = false;

        //Debug.Log("关卡重置初始化/首次进入关卡初始化");
        ShowBottleGo();
        InitBottle(levelInfo);
        BottleLayoutRefresh();

        //连胜去黑水(暂去)
        ////当前有连胜，去黑水瓶生效
        //int WinNum = stageModel.CountinueWinNum;
        ////Debug.Log("当前连胜次数:" + WinNum);
        //if (WinNum > 0)
        //    StringEventSystem.Global.Send("StreakWinItem", WinNum);

        //关卡引导判断
        if (GameDefine.GameConst.GuideLevelInfo.TryGetValue(levelId ,out (string guideText,string guideAnimName) value))
        {
            UIKit.OpenPanel<UIGuideAnimPop>(UILevel.PopUI, new UIGuideAnimPopData
            {
                GuideText = value.guideText,
                GuideAnimName = value.guideAnimName
            });

            ActionKit.Delay(5f, () =>
            {
                UIKit.ClosePanel<UIGuideAnimPop>();
            }).Start(this);
        }
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
            //瓶子还没做初始配置，直接全部设置4（要改）
            bottle.maxNum = 4;
            bottle.Init(levelInfo.bottles[i], i);
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
        //cantClearColorList.Clear();

        nowHalf = null;
        InitLevels(nowLevel);

        //会触发两次重置(事件调用了StartGame，里面调用了InitLevels)，
        //如果后续有问题，直接在这调用StartGame做那些数据处理
        //this.SendEvent<GameStartEvent>();

        GameCtrl.Instance.InitGameCtrl();
    }

    /// <summary>
    /// 刷新瓶子布局
    /// </summary>
    /// 取决于第二列是否有瓶子
    private void BottleLayoutRefresh()
    {
        var active = false;
        foreach (RectTransform child in BottomBottleNode)
        {
            if (child.gameObject.activeSelf)
            {
                active = true;
                break;
            }
        }
        BottomBottleNode.gameObject.SetActive(active);
    }

    #endregion

    public void Update()
    {
        TimeCountDown();


        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShareManager.Instance.ShareScreen();
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
    }

    /// <summary>
    /// 记录所有瓶子
    /// </summary>
    public void RecordLast()
    {
        LevelManagerRecord record = new LevelManagerRecord();
        record.clearList = new List<int>(clearList);
        record.hideColor = new List<int>(hideColor);
        record.changeList = new List<ChangePair>(changeList);
        LevelManagerRecords.Add(record);

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
        if (ret)
        {
            var record = LevelManagerRecords.LastOrDefault();
            clearList = record.clearList;
            hideColor = record.hideColor;
            changeList = record.changeList;
            LevelManagerRecords.Remove(record);
        }
        return ret;
    }

    /// <summary>
    /// 清除所有附加道具
    /// </summary>
    /// <param name="action">主动道具调用可传入委托</param>
    public void RemoveAll(Action action = null)
    {
        foreach (var bottle in nowBottles)
        {
            bottle.SetNormal();
        }
        cantChangeColorList.Clear();

        action?.Invoke();
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
    public void RemoveHide(Action action, int num = 0)
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
        StartCoroutine(ShowMahoujinCoroutine());
    }

    IEnumerator ShowMahoujinCoroutine()
    {
        AudioKit.PlaySound("resources://Audio/MagicCircle");

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
        IReadOnlyList<int> _useList = null;
        int _index = scene - 1;
        if (_index >= 0 && _index < SceneUnLockSOs.Count)
        {
            _useList = SceneUnLockSOs[_index].SceneNeedStarCount;
        }

        // num==0时，首场景返回0，其它场景返回上一个场景的最后一个部件总需星数
        if (num == 0)
        {
            if (scene == 1)
                return 0;

            int prevIndex = _index - 1;
            if (prevIndex >= 0 && prevIndex < SceneUnLockSOs.Count)
            {
                var prevList = SceneUnLockSOs[prevIndex].SceneNeedStarCount;
                return prevList[prevList.Count - 1];
            }
        }

        // 正常返回当前部件所需星数(可能越界)
        return _useList[num - 1];
    }

    /// <summary>
    /// 判断下一场景需要多少星星解锁场景第一部件(判断需要多少星星解锁下一部件)
    /// </summary>
    /// <param name="scene">当前场景编号</param>
    /// <param name="num">当前部件编号</param>
    /// <returns></returns>
    public int GetPartNeedStar(int scene, int num)
    {
        IReadOnlyList<int> _useList = null;
        int _index = scene - 1;
        if (_index >= 0 && _index < SceneUnLockSOs.Count)
            _useList = SceneUnLockSOs[_index].ScenePartNeedStar;
        else
        {
            //Debug.Log("越界，设为最后一个场景");
            _useList = SceneUnLockSOs[SceneUnLockSOs.Count - 1].ScenePartNeedStar;
        }

        return num == 0 ? _useList[0] : _useList[num - 1];
    }
   
    /// <summary>
    /// 道具选择(替换背景，更换瓶子材质)
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
    /// 取消道具选择
    /// </summary>
    public void HideItemSelect()
    {
        hideBg.SetActive(false);
        for (int i = 0; i < nowBottles.Count; i++)
        {
            nowBottles[i].HideItemSelect();
        }
    }
}

[Serializable]
public class BottleRecord
{
    public bool isFinish, isFreeze, isClearHide, isNearHide;
    public int limitColor;
    public List<int> waters = new List<int>();
    public List<bool> hideWaters = new List<bool>();
    public List<WaterItem> waterItems = new List<WaterItem>();
}

[Serializable]
public class LevelManagerRecord
{
    public List<int> clearList;
    public List<int> hideColor;
    public List<ChangePair> changeList;
}