using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIDeleteLifeData : UIPanelData
	{
	}
	public partial class UIDeleteLife : UIPanel, ICanSendEvent, ICanGetUtility, ICanGetModel
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIDeleteLifeData ?? new UIDeleteLifeData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BtnClose.onClick.AddListener(() =>
			{
                CloseSelf();
            });
			BtnQuit.onClick.AddListener(() =>
			{
				LevelManager.Instance.InitBottle();
                this.SendEvent<ReturnMainEvent>();
                this.GetModel<StageModel>().ResetCountinueWinNum();
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
