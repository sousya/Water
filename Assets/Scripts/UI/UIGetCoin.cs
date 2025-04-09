using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIGetCoinData : UIPanelData
	{
	}
	public partial class UIGetCoin : UIPanel, ICanSendEvent, ICanGetUtility
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIGetCoinData ?? new UIGetCoinData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
        {
			BindClick();

			TxtLevel.text = "Level " + (this.GetUtility<SaveDataUtility>().GetLevelClear() - 1).ToString();
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		void BindClick()
		{
            BtnClose.onClick.AddListener(() =>
            {
                LevelClearEvent e = new LevelClearEvent();
				e.coin = 20;

                this.SendEvent<LevelClearEvent>(e);
				CloseSelf();
            });

            BtnContinue.onClick.AddListener(() =>
            {
                LevelClearEvent e = new LevelClearEvent();
                e.coin = 20;

                this.SendEvent<LevelClearEvent>(e);
				CloseSelf();
            });
        }
	}
}
