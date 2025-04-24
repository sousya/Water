using UnityEngine;
using UnityEngine.UI;
using QFramework;
using GameDefine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine.SocialPlatforms;
using System.Linq;
using UnityEngine.Rendering;

namespace QFramework.Example
{
    public class UIBeginData : UIPanelData
    {
    }
    public partial class UIBegin : UIPanel, ICanRegisterEvent, ICanGetUtility
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public List<Animator> ButtonAnim;

        public GameObject BeginNode, LevelNode, SceneNode1, SceneNode2, SceneNode3, SceneNode4;
        public ScenePartCtrl ScenePart1, ScenePart2, ScenePart3, ScenePart4;
        public ParticleTargetMoveCtrl coinFx, starFx;
        int nowButton = 2;

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
            BindBtn();
            RegisterEvent();
            SetText();
            SetCoin();
            SetStar();
            SetVitality();
            SetItem();
            InitBeginMenuButton();

            var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
            if (levelNow <= 5)
            {
                LevelNode.SetActive(true);
                BeginNode.SetActive(false);
            }
            SetScene();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        //按钮监听
        void BindBtn()
        {
            BtnRefresh.onClick.RemoveAllListeners();
            BtnRemoveHide.onClick.RemoveAllListeners();
            BtnAddBottle.onClick.RemoveAllListeners();
            BtnHalfBottle.onClick.RemoveAllListeners();
            BtnRemoveAll.onClick.RemoveAllListeners();

            BtnRefresh.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim)
                {
                    var num = this.GetUtility<SaveDataUtility>().GetItemNum(1);
                    if (num <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 1 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    if (LevelManager.Instance.ReturnLast())
                    {
                        UseItemUpdateNum(1);
                    }
                }
            });

            BtnRemoveHide.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim)
                {
                    var num = this.GetUtility<SaveDataUtility>().GetItemNum(2);
                    if (num <= 0)
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
                            UseItemUpdateNum(2);
                        });
                    }
                }
            });

            BtnAddBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim)
                {
                    var num = this.GetUtility<SaveDataUtility>().GetItemNum(3);
                    if (num <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 3 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(false, () =>
                    {
                        UseItemUpdateNum(3);
                    });
                }
            });

            BtnHalfBottle.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim)
                {
                    var num = this.GetUtility<SaveDataUtility>().GetItemNum(4);
                    if (num <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 4 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        UseItemUpdateNum(4);
                    });
                }
            });

            BtnRemoveAll.onClick.AddListener(() =>
            {
                if (!LevelManager.Instance.isPlayFxAnim)
                {
                    var num = this.GetUtility<SaveDataUtility>().GetItemNum(5);
                    if (num <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 5 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    //判定是否有任何阻碍效果
                    if (LevelManager.Instance.CheckAllDebuff())
                    {
                        LevelManager.Instance.RemoveAll();
                        UseItemUpdateNum(5);
                    }
                }
            });

            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBeginSelect>();
            });

            BeginMenuButton1.onClick.RemoveAllListeners();
            BeginMenuButton1.onClick.AddListener(() =>
            {
                nowButton = 1;
                CheckBeginMenuButton();
            });

            BeginMenuButton2.onClick.RemoveAllListeners();
            BeginMenuButton2.onClick.AddListener(() =>
            {
                nowButton = 2;
                CheckBeginMenuButton();
            });

            BeginMenuButton3.onClick.RemoveAllListeners();
            BeginMenuButton3.onClick.AddListener(() =>
            {
                nowButton = 3;
                CheckBeginMenuButton();
            });

            BtnReturn.onClick.RemoveAllListeners();
            BtnReturn.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIRetry>();
            });

            BtnHeart.onClick.RemoveAllListeners();
            BtnHeart.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIMoreLife>();
            });

            BtnArea.onClick.RemoveAllListeners();
            BtnArea.onClick.AddListener(() =>
            {
                //已获得星星数
                var nowStar = this.GetUtility<SaveDataUtility>().GetLevelClear() - 1;// -1
                //当前场景编号
                var scene = this.GetUtility<SaveDataUtility>().GetSceneRecord();
                //当前场景建筑编号
                var num = this.GetUtility<SaveDataUtility>().GetScenePartRecord();
                //剩余多少星星
                var offset = nowStar - LevelManager.Instance.GetUnlockNeedStar(scene, num);
                //Debug.Log("已有星星 nowStar:" + nowStar);
                //Debug.Log("场景编号 sceneNow :" + scene);
                //Debug.Log("场景部件编号 partNow :" + num);
                //Debug.Log("使用星星" + LevelManager.Instance.GetUnlockNeedStar(scene, num));
                //Debug.Log("剩余星星 offset" + offset);
                //剩余星星大于等于下一部件所需星星 或 宝箱满了未开
                //Debug.Log("需要星星 ：" + LevelManager.Instance.GetPartNeedStar(scene, num));
                //Debug.Log(this.GetUtility<SaveDataUtility>().GetSceneBox());
                if (offset >= LevelManager.Instance.GetPartNeedStar(scene, num) ||
                    (this.GetUtility<SaveDataUtility>().GetSceneBox() != scene) && scene != 1)
                {
                    UIKit.OpenPanel<UIUnlockScene>();
                }
                else
                {
                    UIKit.OpenPanel<UILessStar>();
                }
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
            //BtnItem2.onClick.RemoveAllListeners();
            BtnItem3.onClick.RemoveAllListeners();

            //判断是否还有道具
            BtnItem1.onClick.AddListener(() =>
            {
                if (CheckHaveItem(6))
                    UseItem(6, BtnItem1);
            });
            //由点击使用改为自动使用
            /*BtnItem2.onClick.AddListener(() =>
            {
                if (CheckHaveItem(7))
                    UseItem(7, BtnItem2);
            });*/
            BtnItem3.onClick.AddListener(() =>
            {
                if (CheckHaveItem(8))
                    UseItem(8, BtnItem3);
            });


            BtnHead.onClick.RemoveAllListeners();
            BtnHead.onClick.AddListener(() =>
            {
                UIKit.OpenPanel("UIPersonal");
            });
        }

        //事件注册
        void RegisterEvent()
        {
            this.RegisterEvent<LevelStartEvent>(e =>
            {
                TxtLevel.text = LevelManager.Instance.levelId.ToString();//"Level " + 
                SetTakeItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<LevelClearEvent>(e =>
            {
                //SetScene();
                ReturnBegin(e.coin);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<CoinChangeEvent>(e =>
            {
                SetCoin();
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
                LevelNode.SetActive(false);
                BeginNode.SetActive(true);
                SetScene();
                CheckBeginMenuButton();
                this.GetUtility<SaveDataUtility>().CostVitality();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<RefreshItemEvent>(e =>
            {
                SetTakeItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<GameStartEvent>(e =>
            {
                LevelManager.Instance.StartGame(this.GetUtility<SaveDataUtility>().GetLevelClear());
                LevelNode.SetActive(true);
                BeginNode.SetActive(false);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //驱动按钮初始选择
            StringEventSystem.Global.Register("InitBeginMenuButton", () =>
            {
                InitBeginMenuButton();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            //尝试使用道具7
            StringEventSystem.Global.Register("TryUserItem7", () =>
            {
                if (CheckHaveItem(7))
                    UseItem(7, BtnItem2);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>
        /// 检查主页菜单当前状态
        /// </summary>
		void CheckBeginMenuButton()
        {
            switch (nowButton)
            {
                case 1:
                    ButtonAnim[0].Play("BeginMenuButtonSelect");
                    ButtonAnim[1].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[2].Play("BeginMenuButtonUnSelect");
                    break;
                case 2:
                    ButtonAnim[0].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[1].Play("BeginMenuButtonSelect");
                    ButtonAnim[2].Play("BeginMenuButtonUnSelect");
                    break;
                case 3:
                    ButtonAnim[0].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[1].Play("BeginMenuButtonUnSelect");
                    ButtonAnim[2].Play("BeginMenuButtonSelect");
                    break;
            }
        }

        /// <summary>
        /// 初始化主页菜单当前状态
        /// </summary>
        void InitBeginMenuButton()
        {
            ButtonAnim[0].Play("BeginMenuButtonUnSelect");
            ButtonAnim[1].Play("BeginMenuButtonSelect");
            ButtonAnim[2].Play("BeginMenuButtonUnSelect");
        }

        /// <summary>
        /// 使用携带道具按钮事件
        /// </summary>
        void SetTakeItem()
        {
            BtnItemBg.gameObject.SetActive(LevelManager.Instance.takeItem.Count > 0);
            BtnItem1.gameObject.SetActive(LevelManager.Instance.takeItem.Contains(6));
            BtnItem2.gameObject.SetActive(LevelManager.Instance.takeItem.Contains(7));
            BtnItem3.gameObject.SetActive(LevelManager.Instance.takeItem.Contains(8));

            TxtItem1.text = this.GetUtility<SaveDataUtility>().GetItemNum(6).ToString();
            TxtItem2.text = this.GetUtility<SaveDataUtility>().GetItemNum(7).ToString();
            TxtItem3.text = this.GetUtility<SaveDataUtility>().GetItemNum(8).ToString();

            SetItem();
        }

        /// <summary>
        /// 检查是否拥有道具
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        bool CheckHaveItem(int itemID)
        {
            if (this.GetUtility<SaveDataUtility>().GetItemNum(itemID) > 0)
                return true;
            else return false;
        }

        /// <summary>
        /// 使用携带道具 --道具效果还未实现
        /// </summary>
        /// <param name="itemID"></param>
        void UseItem(int itemID, Button itemObj)
        {
            switch (itemID)
            {
                case 6:
                    LevelManager.Instance.AddBottle(true, () =>
                    {
                        this.GetUtility<SaveDataUtility>().ReduceItemNum(6);
                        TxtItem1.text = this.GetUtility<SaveDataUtility>().GetItemNum(6).ToString();
                    });
                    break;

                case 7:
                    //判定使用有黑水瓶子，有则开始生效，然后道具减一,然后隐藏
                    if (LevelManager.Instance.hideBottleList.Count > 0)
                    {
                        var tempList = new List<BottleCtrl>(LevelManager.Instance.hideBottleList);

                        while (tempList.Count > 2)
                        {
                            int randIndex = Random.Range(0, tempList.Count);
                            tempList.RemoveAt(randIndex);
                        }
                        foreach (var item in tempList)
                        {
                            for (int i = 0; i < item.hideWaters.Count; i++)
                            {
                                item.hideWaters[i] = false;
                            }
                            item.SetHideShow(false);
                        }
                        ActionKit.Delay(0.5f, () =>
                        {
                            //现在的问题就是回调执行的时机不对，需要动画播放完成之后，进行隐藏
                            //没有隐藏的问题，
                            this.GetUtility<SaveDataUtility>().ReduceItemNum(7);
                            TxtItem2.text = this.GetUtility<SaveDataUtility>().GetItemNum(7).ToString();
                            itemObj.Hide();
                        }).Start(this);
                    }
                    break;

                case 8:
                    //可能还需要别的条件
                    if (GameCtrl.Instance.FirstBottle != null)
                    {
                        var botter = GameCtrl.Instance.FirstBottle;

                        //非空瓶和只有一格水,还有水的颜色不统一
                        if (botter.waters.Count > 1 && !botter.waters.All(x => x == botter.waters[0]))
                        {
                            //Debug.Log("可以使用道具");
                            // 索引列表用于随机洗牌
                            List<int> indices = Enumerable.Range(0, botter.waters.Count).ToList();
                            do
                            {
                                for (int i = 0; i < indices.Count; i++)
                                {
                                    int randIndex = Random.Range(i, indices.Count);
                                    (indices[i], indices[randIndex]) = (indices[randIndex], indices[i]);
                                }
                            }
                            while (Enumerable.SequenceEqual(indices, Enumerable.Range(0, botter.waters.Count)));

                            List<int> newWaters = new List<int>();
                            List<bool> newHideWater = new List<bool>();
                            //List<BottleWaterCtrl> newWaterImgs = new List<BottleWaterCtrl>();
                            foreach (int idx in indices)
                            {
                                newWaters.Add(botter.waters[idx]);
                                newHideWater.Add(botter.hideWaters[idx]);
                                //newWaterImgs.Add(botter.waterImg[idx]);
                            }
                            // 替换原列表
                            botter.waters = newWaters;
                            botter.hideWaters = newHideWater;

                            //修改水块颜色和切换道具位置
                            for (int i = 0; i < botter.waters.Count; i++)
                            {
                                var useColor = botter.waters[i] - 1;
                                if (useColor < 1000)
                                {
                                    botter.waterImg[i].color = LevelManager.Instance.waterColor[useColor];
                                    botter.waterImg[i].broomItemGo.SetActive(false);
                                    botter.waterImg[i].createItemGo.SetActive(false);
                                    botter.waterImg[i].changeItemGo.SetActive(false);
                                    botter.waterImg[i].magnetItemGo.SetActive(false);
                                }
                                else
                                {
                                    // 根据道具类型设置对应的显示和动画
                                    switch (botter.waters[i])
                                    {
                                        case (int)ItemType.ClearItem:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                                            break;
                                        case (int)ItemType.MagnetItem:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(true);
                                            botter.waterImg[i].magnetSpine.AnimationState.SetAnimation(0, "idle", false);
                                            break;
                                        case (int)ItemType.MakeColorItem:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(true);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].createSpine.AnimationState.SetAnimation(0, "idle", false);

                                            break;
                                        case (int)ItemType.ChangeGreen:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                                            //waterImg[i].color = new Color(1, 1, 1, 0);
                                            break;
                                        case (int)ItemType.ChangeOrange:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_jh", false);

                                            //waterImg[i].color = new Color(1, 1, 1, 0);
                                            break;
                                        case (int)ItemType.ChangePink:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_fs", false);

                                            //waterImg[i].color = new Color(1, 1, 1, 0);
                                            break;
                                        case (int)ItemType.ChangePurple:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_zs", false);

                                            break;
                                        case (int)ItemType.ChangeYellow:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_hs", false);

                                            break;
                                        case (int)ItemType.ChangeDarkBlue:
                                            botter.waterImg[i].broomItemGo.SetActive(false);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(true);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].changeSpine.AnimationState.SetAnimation(0, "idle_sl", false);

                                            break;
                                        case (int)ItemType.ClearPink:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_fh", false);

                                            break;
                                        case (int)ItemType.ClearOrange:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_jh", false);

                                            break;
                                        case (int)ItemType.ClearBlue:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_gl", false);

                                            break;
                                        case (int)ItemType.ClearYellow:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_hs", false);

                                            break;
                                        case (int)ItemType.ClearDarkGreen:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_sl", false);

                                            break;
                                        case (int)ItemType.ClearRed:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_dh", false);

                                            break;
                                        case (int)ItemType.ClearGreen:
                                            botter.waterImg[i].broomItemGo.SetActive(true);
                                            botter.waterImg[i].createItemGo.SetActive(false);
                                            botter.waterImg[i].changeItemGo.SetActive(false);
                                            botter.waterImg[i].magnetItemGo.SetActive(false);
                                            botter.waterImg[i].broomSpine.AnimationState.SetAnimation(0, "idle_cl", false);

                                            break;
                                    }
                                    //var checkColor = LevelManager.Instance.waterColor[useColor - 1000];
                                    // 设置道具的颜色为统一的道具颜色
                                    botter.waterImg[i].color = LevelManager.Instance.ItemColor;
                                }
                            }

                            //修改水面颜色
                            botter.PlaySpineWaitAnim();
                            //存在小问题，和黑水块交换位置时(不是交换到第一块)，也会触发动画
                            botter.SetHideShow(true);

                            this.GetUtility<SaveDataUtility>().ReduceItemNum(8);
                            TxtItem3.text = this.GetUtility<SaveDataUtility>().GetItemNum(8).ToString();
                            //Debug.Log("打乱顺序成功");
                        }
                    }
                    break;
            }

            if (!CheckHaveItem(itemID))
                itemObj.Hide();
        }

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
            {
                num = this.GetUtility<SaveDataUtility>().GetCoinNum();
            }

            TxtCoin.text = num.ToString();
        }

        /// <summary>
        /// 更新星星数量
        /// </summary>
        void SetStar()
        {
            var nowStar = this.GetUtility<SaveDataUtility>().GetLevelClear() - 1;
            var sceneNow = this.GetUtility<SaveDataUtility>().GetSceneRecord();
            var partNow = this.GetUtility<SaveDataUtility>().GetScenePartRecord();
            var useStar = LevelManager.Instance.GetUnlockNeedStar(sceneNow, partNow);
            TxtStar.text = (nowStar - useStar).ToString();

            //Debug.Log("更新星星数量");
        }

        /// <summary>
        /// 更新体力
        /// </summary>
        void SetVitality()
        {
            TxtHeart.text = this.GetUtility<SaveDataUtility>().GetVitalityNum().ToString();
        }

        /// <summary>
        /// 更新关卡文本
        /// </summary>
        void SetText()
        {
            TxtLevel.text = LevelManager.Instance.levelId.ToString();//"Level " + 
        }

        /// <summary>
        /// 返回主页
        /// </summary>
        /// <param name="coin"></param>
        void ReturnBegin(int coin = 0)
        {
            SetCoin(this.GetUtility<SaveDataUtility>().GetCoinNum() - coin);
            BeginNode.SetActive(true);
            LevelNode.SetActive(false);
            StartCoroutine(ShowFx());
        }

        /// <summary>
        /// 判断是否有道具
        /// </summary>
        public void SetItem()
        {
            var saveU = this.GetUtility<SaveDataUtility>();
            BtnAddRefresh.gameObject.SetActive(saveU.GetItemNum(1) <= 0);
            TxtRefreshNum.text = saveU.GetItemNum(1).ToString();

            BtnAddRemove.gameObject.SetActive(saveU.GetItemNum(2) <= 0);
            TxtRemoveHideNum.text = saveU.GetItemNum(2).ToString();

            BtnAddAddBottle.gameObject.SetActive(saveU.GetItemNum(3) <= 0);
            TxtAddBottleNum.text = saveU.GetItemNum(3).ToString();

            BtnAddHalfBottle.gameObject.SetActive(saveU.GetItemNum(4) <= 0);
            TxtAddHalfBottleNum.text = saveU.GetItemNum(4).ToString();

            BtnAddRemoveBottle.gameObject.SetActive(saveU.GetItemNum(5) <= 0);
            TxtRemoveAllNum.text = saveU.GetItemNum(5).ToString();
        }

        /// <summary>
        /// 使用道具后的回调，更新对应道具剩余数量
        /// </summary>
        void UseItemUpdateNum(int itemId)
        {
            var saveU = this.GetUtility<SaveDataUtility>();
            switch (itemId)
            {
                case 1:
                    saveU.ReduceItemNum(1);
                    BtnAddRefresh.gameObject.SetActive(saveU.GetItemNum(1) <= 0);
                    TxtRefreshNum.text = saveU.GetItemNum(1).ToString();
                    break;
                case 2:
                    saveU.ReduceItemNum(2);
                    BtnAddRemove.gameObject.SetActive(saveU.GetItemNum(2) <= 0);
                    TxtRemoveHideNum.text = saveU.GetItemNum(2).ToString();
                    break;
                case 3:
                    this.GetUtility<SaveDataUtility>().ReduceItemNum(3);
                    BtnAddAddBottle.gameObject.SetActive(saveU.GetItemNum(3) <= 0);
                    TxtAddBottleNum.text = saveU.GetItemNum(3).ToString();
                    break;
                case 4:
                    this.GetUtility<SaveDataUtility>().ReduceItemNum(4);
                    BtnAddHalfBottle.gameObject.SetActive(saveU.GetItemNum(4) <= 0);
                    TxtAddHalfBottleNum.text = saveU.GetItemNum(4).ToString();
                    break;
                case 5:
                    this.GetUtility<SaveDataUtility>().ReduceItemNum(5);
                    BtnAddRemoveBottle.gameObject.SetActive(saveU.GetItemNum(5) <= 0);
                    TxtRemoveAllNum.text = saveU.GetItemNum(5).ToString();
                    break;
            }
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
            TxtCoinAdd.Play("TxtUp");
            coinFx.Play(10);

            yield return new WaitForSeconds(1.5f);
            SetCoin();
        }

        /// <summary>
        /// 更新主页场景建筑和部分UI
        /// </summary>
        void SetScene()
        {
            //Debug.Log("更新场景");
            SetStar();
            var levelNow = this.GetUtility<SaveDataUtility>().GetLevelClear();
            var sceneNow = this.GetUtility<SaveDataUtility>().GetSceneRecord();
            var partNow = this.GetUtility<SaveDataUtility>().GetScenePartRecord();
            TxtArea.text = "Area " + sceneNow;

            //启用场景
            switch (sceneNow)
            {
                case 1:
                    SceneNode1.SetActive(true);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(false);
                    break;
                case 2:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(true);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(false);
                    break;
                case 3:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(true);
                    SceneNode4.SetActive(false);
                    break;
                case 4:
                    SceneNode1.SetActive(false);
                    SceneNode2.SetActive(false);
                    SceneNode3.SetActive(false);
                    SceneNode4.SetActive(true);
                    break;
            }
            ImgProgress.fillAmount = partNow / 5f;
            TxtImgprogress.text = partNow + " / 5";
            //并不需要判断宝箱编号等，只需要负责更新UI即可
            //if (this.GetUtility<SaveDataUtility>().GetSceneBox() == sceneNow)
            //{
            //    TxtArea.text = "Area " + (sceneNow);// + 1
            //    ImgProgress.fillAmount = 0;
            //    TxtImgprogress.text = 0 + " / 5";
            //}
            SetScenePart(sceneNow, partNow);

        }

        /// <summary>
        /// 解锁场景建筑
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="partNow"></param>
        void SetScenePart(int scene, int partNow)
        {
            switch (scene)
            {
                case 1:
                    ScenePart1.ScenePart1.SetActive(partNow >= 1);
                    ScenePart1.ScenePart2.SetActive(partNow >= 2);
                    ScenePart1.ScenePart3.SetActive(partNow >= 3);
                    ScenePart1.ScenePart4.SetActive(partNow >= 4);
                    ScenePart1.ScenePart5.SetActive(partNow >= 5);
                    break;
                case 2:
                    ScenePart2.ScenePart1.SetActive(partNow >= 1);
                    ScenePart2.ScenePart2.SetActive(partNow >= 2);
                    ScenePart2.ScenePart3.SetActive(partNow >= 3);
                    ScenePart2.ScenePart4.SetActive(partNow >= 4);
                    ScenePart2.ScenePart5.SetActive(partNow >= 5);
                    break;
                case 3:
                    ScenePart3.ScenePart1.SetActive(partNow >= 1);
                    ScenePart3.ScenePart2.SetActive(partNow >= 2);
                    ScenePart3.ScenePart3.SetActive(partNow >= 3);
                    ScenePart3.ScenePart4.SetActive(partNow >= 4);
                    ScenePart3.ScenePart5.SetActive(partNow >= 5);
                    break;
                case 4:
                    ScenePart4.ScenePart1.SetActive(partNow >= 1);
                    ScenePart4.ScenePart2.SetActive(partNow >= 2);
                    ScenePart4.ScenePart3.SetActive(partNow >= 3);
                    ScenePart4.ScenePart4.SetActive(partNow >= 4);
                    ScenePart4.ScenePart5.SetActive(partNow >= 5);
                    break;

            }
        }

        /// <summary>
        /// 解锁场景建筑触发特效
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="num"></param>
        void ShowFx(int scene, int num)
        {
            switch (scene)
            {
                case 1:
                    StartCoroutine(ScenePart1.ShowUnlock(num));
                    break;
                case 2:
                    StartCoroutine(ScenePart2.ShowUnlock(num));
                    break;
                case 3:
                    StartCoroutine(ScenePart3.ShowUnlock(num));
                    break;
                case 4:
                    StartCoroutine(ScenePart4.ShowUnlock(num));
                    break;
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

        }
    }
}
