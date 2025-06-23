using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

namespace QFramework.Example
{
    public class UIBeginSelectData : UIPanelData
    {
    }
    public partial class UIBeginSelect : UIPanel, ICanGetUtility, ICanSendEvent, ICanRegisterEvent, ICanGetModel
    {
        [SerializeField] private Sprite[] giftSprites;
        [SerializeField] private Button[] addItemBtns;

        [SerializeField] private Button[] selectBtns;
        [SerializeField] private GameObject[] selectImgs;
        [SerializeField] private TextMeshProUGUI[] itemNumTxts;

        [Header("consecutive_coin")]
        [SerializeField] private Image ImgCoinWinProcess;
        [SerializeField] private TextMeshProUGUI TxtCoinWinProgress;

        private StageModel stageModel;

        private const int CONTINUE_WIN_NUM_ItemGift = 3;

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIBeginSelectData ?? new UIBeginSelectData();
            // please add init code here
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
            stageModel = this.GetModel<StageModel>();
            StringEventSystem.Global.Send("ClearTakeItem");

            RigesterEvent();

            UpdateWinNum();
            UpdateItem();

            BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

            BtnStart.onClick.AddListener(() =>
            {
                if (!HealthManager.Instance.HasHp && !HealthManager.Instance.UnLimitHp)
                {
                    UIKit.OpenPanel<UIMoreLife>();
                    return;
                }
                this.SendEvent<GameStartEvent>();
                GameCtrl.Instance.InitGameCtrl();
                CloseSelf();
            });

            BtnInfo.onClick.AddListener(() =>
            {
                ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
            });

            int startID = 6; //������ʼID
            for (int i = 0; i < addItemBtns.Length; i++)
            {
                //�հ�
                int _itemId = i + startID;
                addItemBtns[i].onClick.AddListener(() =>
                {
                    if (CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))
                        UIKit.OpenPanel<UIBuyItem>(UILevel.Common, new UIBuyItemData() { item = _itemId });
                });
            }

            for (int i = 0; i < selectBtns.Length; i++)
            {
                int _itemId = i + startID;
                var _tempIndex = i;

                selectBtns[i].onClick.AddListener(() =>
                {
                    if (stageModel.ItemDic[_itemId] > 0 && CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN)) 
                    {
                        var show = !selectImgs[_tempIndex].gameObject.activeSelf;
                        selectImgs[_tempIndex].gameObject.SetActive(show);
                        if (show)
                            AddItemIfNotExists(_itemId);
                        else
                            RemoveItemIfExists(_itemId);
                    }
                });
            }

            ImgReward.Hide();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            BtnClose.onClick.RemoveAllListeners();
            BtnStart.onClick.RemoveAllListeners();
            BtnInfo.onClick.RemoveAllListeners();

            foreach (var btn in selectBtns)
            {
                btn.onClick.RemoveAllListeners();
            }
            foreach (var btn in addItemBtns)
            {
                btn.onClick.RemoveAllListeners();
            }
        }

        void RigesterEvent()
        {
            this.RegisterEvent<RefreshItemEvent>(e =>
            {
                UpdateItem();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<UnlimtItemEvent>(e =>
            {
                UpdateItem();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>
        /// Я������
        /// </summary>
        /// <param name="itemId"></param>
        void AddItemIfNotExists(int itemId)
        {
            //�����ظ�����
            if (!LevelManager.Instance.takeItem.Contains(itemId))
                LevelManager.Instance.takeItem.Add(itemId);
        }

        /// <summary>
        /// �Ƴ�Я���ĵ���
        /// </summary>
        /// <param name="itemId"></param>
        void RemoveItemIfExists(int itemId)
        {
            //����ȡ��ѡ����Я��
            if (LevelManager.Instance.takeItem.Contains(itemId))
                LevelManager.Instance.takeItem.Remove(itemId);
        }

        /// <summary>
        /// ������ʤ�����ʾ
        /// </summary>
        void UpdateWinNum()
        {
            int _curWinNum = stageModel.CountinueWinNum;

            int _winNum_Gift = _curWinNum > CONTINUE_WIN_NUM_ItemGift ? CONTINUE_WIN_NUM_ItemGift : _curWinNum;
            int _winNum_Coin = _curWinNum > GameDefine.GameConst.CONTINUE_WIN_NUM_COIN ? GameDefine.GameConst.CONTINUE_WIN_NUM_COIN : _curWinNum;

            TxtProgress.text = $"{_winNum_Gift} / {CONTINUE_WIN_NUM_ItemGift}";
            ImgProgress.fillAmount = _winNum_Gift * 1f / CONTINUE_WIN_NUM_ItemGift;

            TxtCoinWinProgress.text = $"{_winNum_Coin}/{GameDefine.GameConst.CONTINUE_WIN_NUM_COIN}";
            //0.081f * ��ʤ���� + 0.095fӳ��ֵ(1-10��ʤӳ�乫ʽ)
            ImgCoinWinProcess.fillAmount = 0.081f * _winNum_Coin + 0.095f;

            //0-3ʤ������ͼ��
            if (_winNum_Gift == 0 || _winNum_Gift == 1)
            {
                ImgBox.sprite = giftSprites[0];
                return;
            }
            ImgBox.sprite = giftSprites[_winNum_Gift - 1];
        }

        /// <summary>
        /// ���µ�����ʾ״̬
        /// </summary>
        void UpdateItem()
        {
            if (!CountDownTimerManager.Instance.IsTimerFinished(GameDefine.GameConst.UNLIMIT_ITEM_SIGN))
            {
                UnLimitNode.Show();
                AddItemIfNotExists(6);
                AddItemIfNotExists(7);
                AddItemIfNotExists(8);

                return;
            }
            else
            {
                StringEventSystem.Global.Send("ClearTakeItem");
                UnLimitNode.Hide();
            }

            UpdateItemDisplay(stageModel.ItemDic[6], itemNumTxts[0], addItemBtns[0]);
            UpdateItemDisplay(stageModel.ItemDic[7], itemNumTxts[1], addItemBtns[1]);
            UpdateItemDisplay(stageModel.ItemDic[8], itemNumTxts[2], addItemBtns[2]);
        }

        /// <summary>
        /// ���µ��߽Ǳ�״̬
        /// </summary>
        /// <param name="itemCount"></param>
        /// <param name="txtItem"></param>
        /// <param name="btnAdd"></param>
        void UpdateItemDisplay(int itemCount, TextMeshProUGUI txtItem, Button btnAdd)
        {
            if (itemCount > 0)
            {
                btnAdd.Hide();
                txtItem.transform.parent.Show();
                txtItem.text = itemCount.ToString();
            }
        }
    }
}
