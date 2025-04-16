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
                CloseSelf();
            });

            //查看奖励
            BtnInfo.onClick.RemoveAllListeners();
            BtnInfo.onClick.AddListener(() =>
            {
                ImgReward.gameObject.SetActive(!ImgReward.gameObject.activeSelf);
            });

            BtnBox.onClick.RemoveAllListeners();
            BtnBox.onClick.AddListener(() =>
            {
                //判断是否能打开奖励（3胜及以上），
                if (this.GetUtility<SaveDataUtility>().GetCountinueWinNum() >= 3)
                {
                    RewardNode.Show();
                    ImgItem1.Show();
                    ImgItem2.Show();
                    ImgItem3.Show();
                }
            });

            CloseReward.onClick.RemoveAllListeners();
            CloseReward.onClick.AddListener(() =>
            {
                //打开奖励，触发动画，并开启遮罩
                UIKit.OpenPanel<UIMask>(UILevel.PopUI);
                FlyReward();
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
            //避免没选中仍携带
            if (LevelManager.Instance.takeItem.Contains(itemId))
                LevelManager.Instance.takeItem.Remove(itemId);
        }

        void FlyReward()
        {
            RewardNode.Hide();

            //需要确保动画时长一致
            var seq = DOTween.Sequence();

            seq.Join(ImgItem1.transform.DOMove(TargetPos.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem1.gameObject.SetActive(false);
                    ImgItem1.transform.position = BeginPos1.transform.position;
                }));

            seq.Join(ImgItem2.transform.DOMove(TargetPos.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem2.gameObject.SetActive(false);
                    ImgItem2.transform.position = BeginPos2.transform.position;
                }));

            seq.Join(ImgItem3.transform.DOMove(TargetPos.transform.position, 1f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    ImgItem3.gameObject.SetActive(false);
                    ImgItem3.transform.position = BeginPos3.transform.position;
                }));

            //回调
            seq.AppendCallback(() =>
            {
                //宝箱进度清空
                this.GetUtility<SaveDataUtility>().SetCountinueWinNum(0);

                //道具数量增加
                var saveU = this.GetUtility<SaveDataUtility>();
                saveU.AddItemNum(6, 1);
                saveU.AddItemNum(7, 1);
                saveU.AddItemNum(8, 1);
                UpdateItemNum();

                //关闭遮罩
                UIKit.ClosePanel<UIMask>();
                //Debug.Log("所有动画完成了！");
            });
        }

        void UpdateItemNum()
        {
            var saveU = this.GetUtility<SaveDataUtility>();
            TxtProgress.text = saveU.GetCountinueWinNum() + " / 3";
            TxtItem1.text = saveU.GetItemNum(6) + "";
            TxtItem2.text = saveU.GetItemNum(7) + "";
            TxtItem3.text = saveU.GetItemNum(8) + "";
            ImgProgress.fillAmount = saveU.GetCountinueWinNum() * 1f / 3;
        }
    }
}
