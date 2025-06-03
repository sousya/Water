using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIGetCoinData : UIPanelData
	{
	}
	public partial class UIGetCoin : UIPanel, ICanSendEvent, ICanGetUtility, ICanGetModel
    {
        private StageModel stageModel;

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
            stageModel = this.GetModel<StageModel>();   
        }

        protected override void OnShow()
        {
			BindClick();
            TxtCoin.text = ((int)(GameDefine.GameConst.WIN_COINS * stageModel.GoldCoinsMultiple)).ToString();
            TxtLevel.text = "Level " + (this.GetUtility<SaveDataUtility>().GetLevelClear() - 1).ToString();
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            stageModel = null;
        }

		void BindClick()
		{
            BtnClose.onClick.RemoveAllListeners();
            BtnContinue.onClick.RemoveAllListeners();

            BtnClose.onClick.AddListener(() =>
            {
                BackUIBegin();
            });

            BtnContinue.onClick.AddListener(() =>
            {
                BackUIBegin();
            });
        }

        void BackUIBegin()
        {
            this.SendEvent<LevelClearEvent>(new LevelClearEvent());
            CloseSelf();
        }
	}
}
