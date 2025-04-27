using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using System.Collections;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;

namespace QFramework.Example
{
    public class UIBeginSelectData : UIPanelData
    {
    }
    public partial class UIBeginSelect : UIPanel, ICanGetUtility, ICanSendEvent
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
            UpdateItemNum();

            BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
            {
                StringEventSystem.Global.Send("ClearTakeItem");
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
                else
                {
                    ImgSelect1.gameObject.SetActive(false);
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
                else
                {
                    ImgSelect2.gameObject.SetActive(false);
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
                else
                {
                    ImgSelect3.gameObject.SetActive(false);
                }
            });

            BtnStart.onClick.RemoveAllListeners();
            BtnStart.onClick.AddListener(() =>
            {
                this.SendEvent<GameStartEvent>();
                GameCtrl.Instance.InitGameCtrl();
                CloseSelf();
            });

            BtnInfo.onClick.RemoveAllListeners();
            BtnInfo.onClick.AddListener(() =>
            {
                ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
            });

            ImgReward.Hide();
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
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
            TxtItem1.text = saveU.GetItemNum(6) + "";
            TxtItem2.text = saveU.GetItemNum(7) + "";
            TxtItem3.text = saveU.GetItemNum(8) + "";
            ImgProgress.fillAmount = winNum * 1f / 3;

            //0-3胜，更新图标
            if (winNum == 0 || winNum == 1)
            {
                ImgBox.sprite = giftSprites[0];
                return;
            }
            ImgBox.sprite = giftSprites[winNum - 1];
        }
    }
}
