using UnityEngine;
using UnityEngine.UI;
using QFramework;
using GameDefine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using System.Linq;
using System;
using TMPro;

namespace QFramework.Example
{
    public class UIBeginData : UIPanelData
    {
    }
    public partial class UIBegin : UIPanel, ICanRegisterEvent, ICanGetUtility, ICanGetModel
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public GameObject LevelNode;
        public GameObject[] SceneNodes;
        public ScenePartCtrl[] scenePartCtrls;

        public ParticleTargetMoveCtrl coinFx, starFx;

        #region BottomMenuSetting
        [SerializeField] private List<Button> bottomMenuBtns;
        [SerializeField] private List<RectTransform> bottomMenuRect;
        [SerializeField] private List<GameObject> Panels;
        [SerializeField] private RectTransform selectedImg;
        private GameObject HomeNode => Panels[2];
        private int nowButton = 2;
        private readonly Vector2 SELECTED = new Vector2(256, 200);  // 选中放大的大小
        private readonly Vector2 NSELECTED = new Vector2(206, 200); // 未选中的大小
        private readonly float minScaleValue = 0.5f;                // 按钮的缩小值(先缩小后放大)
        private readonly float maxScaleValue = 1.2f;                // 按钮的放大值
        private readonly float targetPosY = 80f;                    // 按钮往上抬起的高度
        private readonly float initPosY = 15f;                      // 按钮的初始位置
        #endregion

        private TextMeshProUGUI mTxtCoinAdd;
        private StageModel stageModel;
        private SaveDataUtility saveData;

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIBeginData ?? new UIBeginData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            //真机模式下，AssetBundle 加载资源后需要关联材质
            TxtLevel.font.material.shader = Shader.Find(TxtLevel.font.material.shader.name);
            //TxtImgprogress.font.material.shader = Shader.Find(TxtImgprogress.font.material.shader.name);
        }

        protected override void OnShow()
        {
            stageModel = this.GetModel<StageModel>();
            saveData = this.GetUtility<SaveDataUtility>();

            mTxtCoinAdd = TxtCoinAdd.GetComponent<TextMeshProUGUI>();

            BindBtn();
            RegisterEvent();
            SetVitality();
            SetCoin();
            SetStar();
            //InitBeginMenuButton();//有需要初始化可以使用

            //启动游戏时，处于前五关会直接开始游戏，需要更新UI
            var levelNow = saveData.GetLevelClear();
            if (levelNow <= 5)
            {
                StartOrOverChangePanel(true, false);
                TxtLevel.text = LevelManager.Instance.levelId.ToString();
                BottomMenuBtns.Hide();
            }
            SetScene();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        private void Update()
        {
            if (HealthManager.Instance.UnLimitHp || !HealthManager.Instance.IsMaxHp)
            {
                TxtTime.text = HealthManager.Instance.UnLimitHp ?
                    HealthManager.Instance.UnLimitHpTimeStr :
                    HealthManager.Instance.RecoverTimerStr;
            }
        }

        //按钮监听
        void BindBtn()
        {
            BtnStepBack.onClick.RemoveAllListeners();
            BtnRemoveHide.onClick.RemoveAllListeners();
            BtnAddBottle.onClick.RemoveAllListeners();
            BtnHalfBottle.onClick.RemoveAllListeners();
            BtnRemoveAll.onClick.RemoveAllListeners();

            BtnStepBack.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[1] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 1 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    if (LevelManager.Instance.ReturnLast())
                        stageModel.ReduceItem(1, 1);
                }
            });

            BtnRemoveHide.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[2] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 2 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    //判断是否有黑水瓶
                    if (LevelManager.Instance.hideBottleList.Count != 0)
                    {
                        LevelManager.Instance.RemoveHide(() =>
                        {
                            stageModel.ReduceItem(2, 1);
                        });
                    }
                }
            });

            BtnAddBottle.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[3] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 3 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(false, () =>
                    {
                        stageModel.ReduceItem(3, 1);
                    });
                }
            });

            BtnHalfBottle.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[4] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 4 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        stageModel.ReduceItem(4, 1);
                    });
                }
            });

            BtnRemoveAll.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[5] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 5 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    if (LevelManager.Instance.CheckAllDebuff())
                    {
                        LevelManager.Instance.RemoveAll(() =>
                        {
                            //清空操作记录的障碍(避免回退恢复)
                            foreach (var bottle in LevelManager.Instance.nowBottles)
                            {
                                foreach (var record in bottle.moveRecords)
                                {
                                    record.isFreeze = false;
                                    record.isClearHide = false;
                                    record.isNearHide = false;
                                    record.limitColor = 0;

                                    for (int i = 0; i < record.hideWaters.Count; i++)
                                    {
                                        record.hideWaters[i] = false;
                                    }

                                    for (int i = 0; i < record.waterItems.Count; i++)
                                    {
                                        record.waterItems[i] = WaterItem.None;
                                    }
                                }
                            }
                        });
                        stageModel.ReduceItem(5, 1);
                    }
                }
            });

            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                UIKit.OpenPanel<UIBeginSelect>();
            });

            BtnReturn.onClick.RemoveAllListeners();
            BtnReturn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                UIKit.OpenPanel<UIRetry>();
            });

            BtnHeart.onClick.RemoveAllListeners();
            BtnHeart.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                UIKit.OpenPanel<UIMoreLife>();
            });

            BtnArea.onClick.RemoveAllListeners();
            BtnArea.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");

                UIKit.OpenPanel<UIUnlockScene>();
            });

            BtnGetReward.onClick.RemoveAllListeners();
            //获取完奖励回调 
            BtnGetReward.onClick.AddListener(() =>
            {
                //更新场景，
                SetScene();
                StartCoroutine(FlyReward());
            });

            BtnItem1.onClick.RemoveAllListeners();
            BtnItem2.onClick.RemoveAllListeners();
            BtnItem3.onClick.RemoveAllListeners();

            BtnItem1.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");
                UseItem(6, BtnItem1);
            });
            BtnItem2.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");
                UseItem(7, BtnItem2);
            });
            BtnItem3.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");
                LevelManager.Instance.ShowItemSelect();
                GameCtrl.Instance.SeletedItem(bottele => { UseItem(8, BtnItem3, bottele); });
            });

            BtnHead.onClick.RemoveAllListeners();
            BtnHead.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/BtnSound");
                UIKit.OpenPanel("UIPersonal");
            });

            BtnCoin.onClick.AddListener(() =>
            {
                //跳转商店
                InitBeginMenuButton(0);
            });

            //底部区域按钮监听
            foreach (var btn in bottomMenuBtns)
            {
                btn.onClick.AddListener(() =>
                {
                    AudioKit.PlaySound("resources://Audio/BtnSound");
                    int index = bottomMenuBtns.IndexOf(btn);
                    //切换界面
                    ChangePanel(index);
                    if (nowButton != index)
                    {
                        for (int i = 0; i < bottomMenuRect.Count; i++)
                        {
                            var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                            if (i == index)
                            {
                                //设置选中效果
                                rt.localScale = new Vector3(minScaleValue, minScaleValue, minScaleValue);
                                rt.DOScale(new Vector3(maxScaleValue, maxScaleValue, 1), 0.1f);
                                rt.DOLocalMoveY(targetPosY, 0.1f);
                                bottomMenuRect[index].sizeDelta = SELECTED;
                            }
                            else
                            {
                                //设置未选中效果
                                rt.DOScale(Vector3.one, 0.2f);
                                rt.DOLocalMoveY(initPosY, 0.2f);
                                bottomMenuRect[i].sizeDelta = NSELECTED;
                            }

                        }
                        //等待一帧
                        ActionKit.DelayFrame(1, () =>
                        {
                            //同步按钮中心位置(可以设置按钮下的字体显示)
                            for (int i = 0; i < bottomMenuBtns.Count; i++)
                            {
                                var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                                rt.DOLocalMoveX(bottomMenuRect[i].localPosition.x, 0.2f);
                            }
                            //更新滑动块
                            selectedImg.DOMove(bottomMenuRect[index].position, 0.1f);
                            nowButton = index;
                        }).Start(this);
                    }
                });
            }
        }

        //事件注册
        void RegisterEvent()
        {
            //开始游戏事件
            this.RegisterEvent<LevelStartEvent>(e =>
            {
                BottomMenuBtns.Hide();
                TxtLevel.text = LevelManager.Instance.levelId.ToString();
                SetTakeItem();
                SetItem();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //胜利结算=》返回主页事件
            this.RegisterEvent<LevelClearEvent>(e =>
            {
                LevelManager.Instance.InitBottle();
                InitBeginMenuButton();
                StartOrOverChangePanel(false, true);
                StartCoroutine(ShowFx());

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<CoinChangeEvent>(e =>
            {
                SetCoin(e.coin);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<VitalityChangeEvent>(e =>
            {
                SetVitality();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<UnlockSceneEvent>(e =>
            {
                SetScene();
                ShowFx(e.scene, e.part);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<RewardSceneEvent>(e =>
            {
                ShowReward();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<ReturnMainEvent>(e =>
            {
                string _del = $"用户退出关卡:{saveData.GetLevelClear()}," +
                $"当前关卡进度:{saveData.GetLevelClear()}";
                AnalyticsManager.Instance.SendLevelEvent(_del);

                StartOrOverChangePanel(false, true);
                //SetScene();
                InitBeginMenuButton();
                HealthManager.Instance.UseHp();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<RefreshItemEvent>(e =>
            {
                SetItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<GameStartEvent>(e =>
            {
                LevelManager.Instance.StartGame(saveData.GetLevelClear());
                StartOrOverChangePanel(true, false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            StringEventSystem.Global.Register("StreakWinItem", (int count) =>
            {
                ClearBottleBlackWater(count);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            StringEventSystem.Global.Register("OpenShopPanel", () =>
            {
                InitBeginMenuButton(0);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #region 底部菜单栏按钮切换

        /// <summary>
        /// 菜单按钮点击切换界面
        /// </summary>
        /// <param name="index"></param>
        void ChangePanel(int index)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                if (i == index)
                    Panels[i].Show();
                else
                    Panels[i].Hide();
            }
        }

        /// <summary>
        /// 切换开始游戏和结束游戏面板
        /// </summary>
        /// <param name="levelNode">游戏界面状态</param>
        /// <param name="homeNode">主页开始启用状态</param>
        void StartOrOverChangePanel(bool levelNode, bool homeNode)
        {
            LevelNode.gameObject.SetActive(levelNode);
            HomeNode.gameObject.SetActive(homeNode);
        }

        /// <summary>
        /// 显示并初始化底部菜单按钮
        /// </summary>
        void InitBeginMenuButton(int index = -1)
        {
            BottomMenuBtns.Show();
            //有参传入，初始按钮点击(切换对应界面)
            if (index > -1)
                bottomMenuBtns[index].onClick.Invoke();
        }

        #endregion

        #region 道具相关

        /// <summary>
        /// 使用携带道具按钮事件
        /// </summary>
        /// 进入游戏/重置关卡调用
        void SetTakeItem()
        {
            var takeItems = LevelManager.Instance.takeItem;

            var buttons = new[] { BtnItem1, BtnItem2, BtnItem3 };
            var texts = new[] { TxtItem1, TxtItem2, TxtItem3 };
            var itemIds = new[] { 6, 7, 8 };

            for (int i = 0; i < itemIds.Length; i++)
            {
                int itemId = itemIds[i];

                bool active = (takeItems.Contains(itemId) && CheckHaveItem(itemId)) || HealthManager.Instance.UnLimitHp;
                buttons[i].interactable = active;
                texts[i].text = active ? "1" : "0";
            }
        }

        /// <summary>
        /// 检查是否拥有道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool CheckHaveItem(int itemID)
        {
            if (stageModel.ItemDic[itemID] > 0)
                return true;
            else return false;
        }

        /// <summary>
        /// 使用携带道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemObj"></param>
        /// <param name="botter">作用与哪个瓶子(打乱水块道具传入)</param>
        void UseItem(int itemID, Button itemObj, BottleCtrl botter = null)
        {
            switch (itemID)
            {
                case 6:
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        if (!HealthManager.Instance.UnLimitHp)
                            stageModel.ReduceItem(6, 1);
                        TxtItem1.text = "0";
                    });
                    break;

                case 7:
                    if (!(LevelManager.Instance.hideBottleList.Count > 0))
                        return;
                    ClearBottleBlackWater(2, true, () =>
                    {
                        if (!HealthManager.Instance.UnLimitHp)
                            stageModel.ReduceItem(7, 1);
                        TxtItem2.text = "0";
                    });
                    break;

                case 8:
                    // 索引列表用于随机洗牌
                    List<int> _indices = Enumerable.Range(0, botter.waters.Count).ToList();
                    do
                    {
                        for (int i = 0; i < _indices.Count; i++)
                        {
                            int randIndex = UnityEngine.Random.Range(i, _indices.Count);
                            (_indices[i], _indices[randIndex]) = (_indices[randIndex], _indices[i]);
                        }
                    }
                    while (Enumerable.SequenceEqual(_indices.Select(i => botter.waters[i]), botter.waters));

                    List<int> _newWaters = new List<int>();
                    List<bool> _newHideWater = new List<bool>();
                    List<WaterItem> _newWaterItems = new List<WaterItem>();

                    foreach (int idx in _indices)
                    {
                        _newWaters.Add(botter.waters[idx]);
                        _newHideWater.Add(botter.hideWaters[idx]);
                        _newWaterItems.Add(botter.waterItems[idx]);
                    }
                    // 替换原列表
                    botter.waters = _newWaters;
                    botter.hideWaters = _newHideWater;
                    botter.waterItems = _newWaterItems;

                    //修改水块颜色和切换道具位置
                    for (int i = 0; i < botter.waters.Count; i++)
                    {
                        var useColor = botter.waters[i] - 1;
                        if (useColor < 1000)
                            botter.waterImg[i].SetColorState(ItemType.UseColor, LevelManager.Instance.waterColor[useColor]);
                        else
                            botter.waterImg[i].SetColorState((ItemType)botter.waters[i], LevelManager.Instance.ItemColor);
                    }

                    //修改水面位置，修改水面颜色并播放水面动画
                    botter.SetNowSpinePos(botter.waters.Count);
                    botter.PlaySpineWaitAnim();
                    botter.CheckWaterItem();

                    botter.SetHideShow(true);
                    LevelManager.Instance.HideItemSelect();

                    if (!HealthManager.Instance.UnLimitHp)
                        stageModel.ReduceItem(8, 1);
                    TxtItem3.text = "0";
                    //Debug.Log("打乱顺序成功");
                    break;
            }

            //if (!CheckHaveItem(itemID))//调整为仅使用一次
            itemObj.interactable = false;
        }

        /// <summary>
        /// 祛除瓶中所有黑水
        /// </summary>
        /// <param name="count">祛除的瓶子数量</param>
        /// <param name="effctNow">是否立即生效</param>
        /// <param name="action">回调(道具使用时传入)</param>
        private void ClearBottleBlackWater(int count, bool effctNow = false, Action action = null)
        {
            if (LevelManager.Instance.hideBottleList.Count > 0)
            {
                var tempList = new List<BottleCtrl>(LevelManager.Instance.hideBottleList);

                while (tempList.Count > count)
                {
                    int randIndex = UnityEngine.Random.Range(0, tempList.Count);
                    tempList.RemoveAt(randIndex);
                }
                foreach (var item in tempList)
                {
                    for (int i = 0; i < item.hideWaters.Count; i++)
                    {
                        item.hideWaters[i] = false;
                    }
                    item.SetHideShow(effctNow);
                    LevelManager.Instance.hideBottleList.Remove(item);
                }

                action?.Invoke();
            }
        }

        /// <summary>
        /// 下方道具栏道具更新
        /// </summary>
        private void SetItem()
        {
            BtnAddStepBack.gameObject.SetActive(stageModel.ItemDic[1] <= 0);
            TxtRefreshNum.text = stageModel.ItemDic[1].ToString();

            BtnAddRemove.gameObject.SetActive(stageModel.ItemDic[2] <= 0);
            TxtRemoveHideNum.text = stageModel.ItemDic[2].ToString();

            BtnAddAddBottle.gameObject.SetActive(stageModel.ItemDic[3] <= 0);
            TxtAddBottleNum.text = stageModel.ItemDic[3].ToString();

            BtnAddHalfBottle.gameObject.SetActive(stageModel.ItemDic[4] <= 0);
            TxtAddHalfBottleNum.text = stageModel.ItemDic[4].ToString();

            BtnAddRemoveBottle.gameObject.SetActive(stageModel.ItemDic[5] <= 0);
            TxtRemoveAllNum.text = stageModel.ItemDic[5].ToString();
        }

        #endregion

        /// <summary>
        /// 解锁建筑宝箱奖励
        /// </summary>
        void ShowReward()
        {
            RewardNode.gameObject.SetActive(true);
            ImgItem1.gameObject.SetActive(true);
            ImgItem2.gameObject.SetActive(true);
            ImgItem3.gameObject.SetActive(true);
            ImgItem4.gameObject.SetActive(true);
            ImgItem5.gameObject.SetActive(true);
            ImgItem6.gameObject.SetActive(true);
            ImgItem7.gameObject.SetActive(true);
            ImgItem8.gameObject.SetActive(true);
        }

        /// <summary>
        /// 更新金币数量
        /// </summary>
        /// <param name="num"></param>
        void SetCoin(int num = 0)
        {
            if (num == 0)
                num = CoinManager.Instance.Coin;

            TxtCoin.text = num.ToString();
        }

        /// <summary>
        /// 更新星星数量
        /// </summary>
        void SetStar()
        {
            var nowStar = saveData.GetLevelClear() - 1;
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            //可以记录一个已使用星星数量
            var useStar = LevelManager.Instance.GetUnlockNeedStar(sceneNow, partNow);
            TxtStar.text = (nowStar - useStar).ToString();

            //Debug.Log("更新星星数量");
        }

        /// <summary>
        /// 更新体力
        /// </summary>
        void SetVitality()
        {
            TxtHeart.text = HealthManager.Instance.UnLimitHp ? "∞" : HealthManager.Instance.NowHp.ToString();

            if (!HealthManager.Instance.UnLimitHp && HealthManager.Instance.IsMaxHp)
                TxtTime.text = "FULL";
        }

        /// <summary>
        /// 金币和星星的飞行粒子效果
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowFx()
        {
            starFx.Play(10);

            yield return new WaitForSeconds(1.5f);
            SetStar();
            mTxtCoinAdd.text = $"+{GameConst.WIN_COINS * stageModel.GoldCoinsMultiple}";
            TxtCoinAdd.Play("TxtUp");
            coinFx.Play(10);

            yield return new WaitForSeconds(1.5f);
            CoinManager.Instance.AddCoin((int)(GameConst.WIN_COINS * stageModel.GoldCoinsMultiple));
        }

        /// <summary>
        /// 更新主页场景建筑和部分UI
        /// </summary>
        void SetScene()
        {
            SetStar();
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            
            TxtArea.text = "Area " + sceneNow;

            //解锁完成(锁定最后一个场景)
            if (saveData.GetOverUnLock())
            {
                ImgProgress.fillAmount = 0;
                TxtImgprogress.text = 0 + " / 5";

                var _scene = LevelManager.Instance.SceneUnLockSOs.Count() - 1;
                SceneNodes[_scene].Show();
                SetScenePart(_scene, partNow);
                return;
            }

            //未全部解锁
            for (int i = 0; i < SceneNodes.Length; i++)
            {
                //场景从1开始计算
                if ((i + 1) == sceneNow)
                    SceneNodes[i].Show();
                else
                    SceneNodes[i].Hide();
            }

            ImgProgress.fillAmount = partNow / 5f;
            TxtImgprogress.text = partNow + " / 5";

            SetScenePart(sceneNow, partNow);
        }

        /// <summary>
        /// 解锁场景建筑
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="partNow"></param>
        void SetScenePart(int scene, int partNow)
        {
            //场景从1开始计算
            int _index = scene - 1;
            if (_index >= 0 && _index < scenePartCtrls.Length)
            {
                for (int j = 0; j < scenePartCtrls[_index].SceneParts.Length; j++)
                {
                    scenePartCtrls[_index].SceneParts[j].SetActive(partNow > j);
                }
            }
        }

        /// <summary>
        /// 解锁场景建筑触发特效
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="num"></param>
        void ShowFx(int scene, int num)
        {
            //场景从1开始计算
            int _index = scene - 1;
            if (_index >= 0 && _index < scenePartCtrls.Length)
            {
                StartCoroutine(scenePartCtrls[_index].ShowUnlock(num));
            }
        }

        /// <summary>
        /// 道具飞行效果(完成回调)
        /// </summary>
        /// <returns></returns>
        IEnumerator FlyReward()
        {
            RewardNode.gameObject.SetActive(false);

            ImgItem1.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem1.gameObject.SetActive(false);
                    ImgItem1.transform.position = Begin1.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem2.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem2.gameObject.SetActive(false);
                    ImgItem2.transform.position = Begin2.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem3.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem3.gameObject.SetActive(false);
                    ImgItem3.transform.position = Begin3.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem4.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem4.gameObject.SetActive(false);
                    ImgItem4.transform.position = Begin4.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem5.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem5.gameObject.SetActive(false);
                    ImgItem5.transform.position = Begin5.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem6.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem6.gameObject.SetActive(false);
                    ImgItem6.transform.position = Begin6.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem7.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem7.gameObject.SetActive(false);
                    ImgItem7.transform.position = Begin7.transform.position;
                });
            yield return new WaitForSeconds(0.2f);
            ImgItem8.transform.DOMove(Target.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem8.gameObject.SetActive(false);
                    ImgItem8.transform.position = Begin8.transform.position;
                });

            RewardCoinFx.Play(10);
            yield return new WaitForSeconds(1.5f);
            CoinManager.Instance.AddCoin(200);
        }
    }
}
