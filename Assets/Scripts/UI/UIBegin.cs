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
        private readonly Vector2 SELECTED = new Vector2(256, 200);  // ѡ�зŴ�Ĵ�С
        private readonly Vector2 NSELECTED = new Vector2(206, 200); // δѡ�еĴ�С
        private readonly float minScaleValue = 0.5f;                // ��ť����Сֵ(����С��Ŵ�)
        private readonly float maxScaleValue = 1.2f;                // ��ť�ķŴ�ֵ
        private readonly float targetPosY = 80f;                    // ��ť����̧��ĸ߶�
        private readonly float initPosY = 15f;                      // ��ť�ĳ�ʼλ��
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
            //���ģʽ�£�AssetBundle ������Դ����Ҫ��������
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
            SetAvatar();
            SetVitality();
            SetCoin();
            SetStar();
            //InitBeginMenuButton();//����Ҫ��ʼ������ʹ��

            //������Ϸʱ������ǰ��ػ�ֱ�ӿ�ʼ��Ϸ����Ҫ����UI
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

        //��ť����
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
                if (!LevelManager.Instance.isPlayFxAnim && GameCtrl.Instance.IsPouring)
                {
                    if (stageModel.ItemDic[2] <= 0)
                    {
                        UIBuyItemData data = new UIBuyItemData() { item = 2 };
                        UIKit.OpenPanel<UIBuyItem>(data);
                        return;
                    }
                    //�ж��Ƿ��к�ˮƿ
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
                            //��ղ�����¼���ϰ�(������˻ָ�)
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
                UIKit.OpenPanel<UIBeginSelect>();
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
                UIKit.OpenPanel<UIUnlockScene>();
            });

            BtnGetReward.onClick.RemoveAllListeners();
            //��ȡ�꽱���ص� 
            BtnGetReward.onClick.AddListener(() =>
            {
                //���³�����
                SetScene();
                StartCoroutine(FlyReward());
            });

            BtnItem1.onClick.RemoveAllListeners();
            BtnItem2.onClick.RemoveAllListeners();
            BtnItem3.onClick.RemoveAllListeners();

            BtnItem1.onClick.AddListener(() =>
            {
                UseItem(6, BtnItem1);
            });
            BtnItem2.onClick.AddListener(() =>
            {
                UseItem(7, BtnItem2);
            });
            BtnItem3.onClick.AddListener(() =>
            {
                LevelManager.Instance.ShowItemSelect();
                GameCtrl.Instance.SeletedItem(bottele => { UseItem(8, BtnItem3, bottele); });
            });

            BtnHead.onClick.RemoveAllListeners();
            BtnHead.onClick.AddListener(() =>
            {
                UIKit.OpenPanel("UIPersonal");
            });

            BtnCoin.onClick.AddListener(() =>
            {
                //��ת�̵�
                InitBeginMenuButton(0);
            });

            //�ײ�����ť����
            foreach (var btn in bottomMenuBtns)
            {
                btn.onClick.AddListener(() =>
                {
                    int index = bottomMenuBtns.IndexOf(btn);
                    //�л�����
                    ChangePanel(index);
                    if (nowButton != index)
                    {
                        for (int i = 0; i < bottomMenuRect.Count; i++)
                        {
                            var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                            if (i == index)
                            {
                                //����ѡ��Ч��
                                rt.localScale = new Vector3(minScaleValue, minScaleValue, minScaleValue);
                                rt.DOScale(new Vector3(maxScaleValue, maxScaleValue, 1), 0.1f);
                                rt.DOLocalMoveY(targetPosY, 0.1f);
                                bottomMenuRect[index].sizeDelta = SELECTED;
                            }
                            else
                            {
                                //����δѡ��Ч��
                                rt.DOScale(Vector3.one, 0.2f);
                                rt.DOLocalMoveY(initPosY, 0.2f);
                                bottomMenuRect[i].sizeDelta = NSELECTED;
                            }

                        }
                        //�ȴ�һ֡
                        ActionKit.DelayFrame(1, () =>
                        {
                            //ͬ����ť����λ��(�������ð�ť�µ�������ʾ)
                            for (int i = 0; i < bottomMenuBtns.Count; i++)
                            {
                                var rt = bottomMenuBtns[i].GetComponent<RectTransform>();
                                rt.DOLocalMoveX(bottomMenuRect[i].localPosition.x, 0.2f);
                            }
                            //���»�����
                            selectedImg.DOMove(bottomMenuRect[index].position, 0.1f);
                            nowButton = index;
                        }).Start(this);
                    }
                });
            }
        }

        //�¼�ע��
        void RegisterEvent()
        {
            //��ʼ��Ϸ�¼�
            this.RegisterEvent<LevelStartEvent>(e =>
            {
                BottomMenuBtns.Hide();
                TxtLevel.text = LevelManager.Instance.levelId.ToString();
                SetTakeItem();
                SetItem();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //ʤ������=��������ҳ�¼�
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
                string _del = $"�û��˳��ؿ�:{saveData.GetLevelClear()}," +
                $"��ǰ�ؿ�����:{saveData.GetLevelClear()}";
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

            this.RegisterEvent<AvatarEvent>(e =>
            {
                BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true, e.AvatarId);
                ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false, e.AvatarFrameId);
            }).UnRegisterWhenGameObjectDestroyed(this);

            StringEventSystem.Global.Register("StreakWinItem", (int count) =>
            {
                ClearBottleBlackWater(count);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            StringEventSystem.Global.Register("OpenShopPanel", () =>
            {
                InitBeginMenuButton(0);

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #region �ײ��˵�����ť�л�

        /// <summary>
        /// �˵���ť����л�����
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
        /// �л���ʼ��Ϸ�ͽ�����Ϸ���
        /// </summary>
        /// <param name="levelNode">��Ϸ����״̬</param>
        /// <param name="homeNode">��ҳ��ʼ����״̬</param>
        void StartOrOverChangePanel(bool levelNode, bool homeNode)
        {
            LevelNode.gameObject.SetActive(levelNode);
            HomeNode.gameObject.SetActive(homeNode);
        }

        /// <summary>
        /// ��ʾ����ʼ���ײ��˵���ť
        /// </summary>
        void InitBeginMenuButton(int index = -1)
        {
            BottomMenuBtns.Show();
            //�вδ��룬��ʼ��ť���(�л���Ӧ����)
            if (index > -1)
                bottomMenuBtns[index].onClick.Invoke();
        }

        #endregion

        #region �������

        /// <summary>
        /// ʹ��Я�����߰�ť�¼�
        /// </summary>
        /// ������Ϸ/���ùؿ�����
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
        /// ����Ƿ�ӵ�е���
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
        /// ʹ��Я������
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="itemObj"></param>
        /// <param name="botter">�������ĸ�ƿ��(����ˮ����ߴ���)</param>
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
                    // �����б��������ϴ��
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
                    // �滻ԭ�б�
                    botter.waters = _newWaters;
                    botter.hideWaters = _newHideWater;
                    botter.waterItems = _newWaterItems;

                    //�޸�ˮ����ɫ���л�����λ��
                    for (int i = 0; i < botter.waters.Count; i++)
                    {
                        var useColor = botter.waters[i] - 1;
                        if (useColor < 1000)
                            botter.waterImg[i].SetColorState(ItemType.UseColor, LevelManager.Instance.waterColor[useColor], i == botter.topIdx);
                        else
                            botter.waterImg[i].SetColorState((ItemType)botter.waters[i], LevelManager.Instance.ItemColor, i == botter.topIdx);
                    }

                    //�޸�ˮ��λ�ã��޸�ˮ����ɫ������ˮ�涯��
                    botter.SetNowSpinePos(botter.waters.Count);
                    botter.PlaySpineWaitAnim();
                    botter.CheckWaterItem();

                    botter.SetHideShow(true);
                    LevelManager.Instance.HideItemSelect();

                    if (!HealthManager.Instance.UnLimitHp)
                        stageModel.ReduceItem(8, 1);
                    TxtItem3.text = "0";
                    //Debug.Log("����˳��ɹ�");
                    break;
            }

            //if (!CheckHaveItem(itemID))//����Ϊ��ʹ��һ��
            itemObj.interactable = false;
        }

        /// <summary>
        /// ���ƿ�����к�ˮ
        /// </summary>
        /// <param name="count">�����ƿ������</param>
        /// <param name="effctNow">�Ƿ�������Ч</param>
        /// <param name="action">�ص�(����ʹ��ʱ����)</param>
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
        /// �·����������߸���
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
        /// �����������佱��
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

        private void SetAvatar()
        {
            BtnHead.GetComponent<Image>().sprite = AvatarManager.Instance.GetAvatarSprite(true);
            ImgHeadFrame.sprite = AvatarManager.Instance.GetAvatarSprite(false);
        }

        /// <summary>
        /// ���½������
        /// </summary>
        /// <param name="num"></param>
        void SetCoin(int num = 0)
        {
            if (num == 0)
                num = CoinManager.Instance.Coin;

            TxtCoin.text = num.ToString();
        }

        /// <summary>
        /// ������������
        /// </summary>
        void SetStar()
        {
            var nowStar = saveData.GetLevelClear() - 1;
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            //���Լ�¼һ����ʹ����������
            var useStar = LevelManager.Instance.GetUnlockNeedStar(sceneNow, partNow);
            TxtStar.text = (nowStar - useStar).ToString();

            //Debug.Log("������������");
        }

        /// <summary>
        /// ��������
        /// </summary>
        void SetVitality()
        {
            TxtHeart.text = HealthManager.Instance.UnLimitHp ? "��" : HealthManager.Instance.NowHp.ToString();

            if (!HealthManager.Instance.UnLimitHp && HealthManager.Instance.IsMaxHp)
                TxtTime.text = "FULL";
        }

        /// <summary>
        /// ��Һ����ǵķ�������Ч��
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowFx()
        {
            mTxtCoinAdd.text = $"+{GameConst.WIN_COINS * stageModel.GoldCoinsMultiple}";
            TxtCoinAdd.Play("TxtUp");
            coinFx.Play(10);
            yield return new WaitForSeconds(0.5f);
            CoinManager.Instance.AddCoin((int)(GameConst.WIN_COINS * stageModel.GoldCoinsMultiple));

            starFx.Play(10);
            yield return new WaitForSeconds(0.5f);
            SetStar();
        }

        /// <summary>
        /// ������ҳ���������Ͳ���UI
        /// </summary>
        void SetScene()
        {
            SetStar();
            var sceneNow = saveData.GetSceneRecord();
            var partNow = saveData.GetScenePartRecord();
            
            TxtArea.text = "Area " + sceneNow;

            //�������(�������һ������)
            if (saveData.GetOverUnLock())
            {
                ImgProgress.fillAmount = 0;
                TxtImgprogress.text = 0 + " / 5";

                var _scene = LevelManager.Instance.SceneUnLockSOs.Count() - 1;
                SceneNodes[_scene].Show();
                SetScenePart(_scene, partNow);
                return;
            }

            //δȫ������
            for (int i = 0; i < SceneNodes.Length; i++)
            {
                //������1��ʼ����
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
        /// ������������
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="partNow"></param>
        void SetScenePart(int scene, int partNow)
        {
            //������1��ʼ����
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
        /// ������������������Ч
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="num"></param>
        void ShowFx(int scene, int num)
        {
            //������1��ʼ����
            int _index = scene - 1;
            if (_index >= 0 && _index < scenePartCtrls.Length)
            {
                StartCoroutine(scenePartCtrls[_index].ShowUnlock(num));
            }
        }

        /// <summary>
        /// ���߷���Ч��(��ɻص�)
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
