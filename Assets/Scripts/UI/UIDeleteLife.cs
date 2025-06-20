using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIDeleteLifeData : UIPanelData
	{
	}
	public partial class UIDeleteLife : UIPanel, ICanSendEvent, IController
    {
		private SaveDataUtility saveData;

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
            saveData = this.GetUtility<SaveDataUtility>();
            BtnClose.onClick.AddListener(() =>
			{
                CloseSelf();
            });
			BtnQuit.onClick.AddListener(() =>
			{
                string _del = $"�û��˳��ؿ�:{saveData.GetLevelClear()}," +
                $"��ǰ�ؿ�����:{saveData.GetLevelClear()}";
                AnalyticsManager.Instance.SendLevelEvent(_del);

                HealthManager.Instance.UseHp();
				UIKit.ClosePanel<UIGameNode>();
                this.GetModel<StageModel>().ResetCountinueWinNum();
                this.SendEvent<ReturnMainEvent>(new ReturnMainEvent());
                CloseSelf();
            });
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			BtnQuit.onClick.RemoveAllListeners();
            saveData = null;
        }
    }
}
