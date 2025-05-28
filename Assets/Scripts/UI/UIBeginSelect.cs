using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using System.Collections;
using DG.Tweening;
using TMPro;

namespace QFramework.Example
{
    public class UIBeginSelectData : UIPanelData
    {
    }
    public partial class UIBeginSelect : UIPanel, ICanGetUtility, ICanSendEvent, ICanRegisterEvent
    {
        [SerializeField] private Sprite[] giftSprites;

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
            RigesterEvent();

            UpdateItemNum();
            StringEventSystem.Global.Send("ClearTakeItem");

            BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

            BtnItem1.onClick.RemoveAllListeners();
            BtnItem1.onClick.AddListener(() =>
            {
                if (this.GetUtility<SaveDataUtility>().GetItemNum(6) > 0)
                {
                    var show = !ImgSelect1.gameObject.activeSelf;
                    ImgSelect1.gameObject.SetActive(show);
                    if (show)
                        AddItemIfNotExists(6);
                    else
                        RemoveItemIfExists(6);
                }
            });

            BtnItem2.onClick.RemoveAllListeners();
            BtnItem2.onClick.AddListener(() =>
            {
                if (this.GetUtility<SaveDataUtility>().GetItemNum(7) > 0)
                {
                    var show = !ImgSelect2.gameObject.activeSelf;
                    ImgSelect2.gameObject.SetActive(show);

                    if (show)
                        AddItemIfNotExists(7);
                    else
                        RemoveItemIfExists(7);
                }
            });

            BtnItem3.onClick.RemoveAllListeners();
            BtnItem3.onClick.AddListener(() =>
            {
                if (this.GetUtility<SaveDataUtility>().GetItemNum(8) > 0)
                {
                    var show = !ImgSelect3.gameObject.activeSelf;
                    ImgSelect3.gameObject.SetActive(show);

                    if (show)
                        AddItemIfNotExists(8);
                    else
                        RemoveItemIfExists(8);
                }
            });

            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                if (!HealthManager.Instance.HasHp)
                {
                    UIKit.OpenPanel<UIMoreLife>();
                    return;
                }
                this.SendEvent<GameStartEvent>();
                GameCtrl.Instance.InitGameCtrl();
                CloseSelf();
            });

            BtnInfo.onClick.RemoveAllListeners();
            BtnInfo.onClick.AddListener(() =>
            {
                ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
            });

            BtnAddItem1.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBuyItem>(UILevel.Common, new UIBuyItemData() { item = 6 });
            });

            BtnAddItem2.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBuyItem>(UILevel.Common, new UIBuyItemData() { item = 7 });
            });

            BtnAddItem3.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIBuyItem>(UILevel.Common, new UIBuyItemData() { item = 8 });
            });
            ImgReward.Hide();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }

        void RigesterEvent()
        {
            this.RegisterEvent<RefreshItemEvent>(e =>
            {
                UpdateItem();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        /// <summary>
        /// 携带道具
        /// </summary>
        /// <param name="itemId"></param>
        void AddItemIfNotExists(int itemId)
        {
            //避免重复入列
            if (!LevelManager.Instance.takeItem.Contains(itemId))
                LevelManager.Instance.takeItem.Add(itemId);
        }

        /// <summary>
        /// 移除携带的道具
        /// </summary>
        /// <param name="itemId"></param>
        void RemoveItemIfExists(int itemId)
        {
            //避免取消选中仍携带
            if (LevelManager.Instance.takeItem.Contains(itemId))
                LevelManager.Instance.takeItem.Remove(itemId);
        }

        void UpdateItemNum()
        {
            var saveU = this.GetUtility<SaveDataUtility>();
            int winNum = saveU.GetCountinueWinNum();
            TxtProgress.text = winNum + " / 3";
            ImgProgress.fillAmount = winNum * 1f / 3;
            UpdateItem();

            //0-3胜，更新图标
            if (winNum == 0 || winNum == 1)
            {
                ImgBox.sprite = giftSprites[0];
                return;
            }
            ImgBox.sprite = giftSprites[winNum - 1];
        }

        void UpdateItem()
        {
            var saveU = this.GetUtility<SaveDataUtility>();
            var item1 = saveU.GetItemNum(6);
            var item2 = saveU.GetItemNum(7);
            var item3 = saveU.GetItemNum(8);
            UpdateItemDisplay(item1, TxtItem1, BtnAddItem1);
            UpdateItemDisplay(item2, TxtItem2, BtnAddItem2);
            UpdateItemDisplay(item3, TxtItem3, BtnAddItem3);
        }

        /// <summary>
        /// 更新道具角标状态
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
