using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

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
			TxtProgress.text = this.GetUtility<SaveDataUtility>().GetCountinueWinNum() + " / 3";
			TxtItem1.text = this.GetUtility<SaveDataUtility>().GetItemNum(6) + "";
			TxtItem2.text = this.GetUtility<SaveDataUtility>().GetItemNum(7) + "";
			TxtItem3.text = this.GetUtility<SaveDataUtility>().GetItemNum(8) + "";
            ImgProgress.fillAmount = this.GetUtility<SaveDataUtility>().GetCountinueWinNum() * 1f / 3;

            BtnItem1.onClick.RemoveAllListeners();
			BtnItem1.onClick.AddListener(() =>
            {
				if(this.GetUtility<SaveDataUtility>().GetItemNum(6) > 0)
				{
                    var show = !ImgSelect1.gameObject.activeSelf;
                    ImgSelect1.gameObject.SetActive(show);
                    if (show)
                    {
                        LevelManager.Instance.takeItem.Add(6);
                    }
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
                    {
                        LevelManager.Instance.takeItem.Add(7);
                    }
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
                    {
                        LevelManager.Instance.takeItem.Add(8);
                    }
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
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
