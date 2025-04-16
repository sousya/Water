using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIRetryData : UIPanelData
	{
	}
	public partial class UIRetry : UIPanel, ICanGetUtility
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIRetryData ?? new UIRetryData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
        {
            var utility = this.GetUtility<SaveDataUtility>();
            var coin = utility.GetCoinNum();
            TxtCoin.text = coin.ToString();

            if (coin < 90)
            {
                TxtCoin.color = Color.red;
                TxtCoinCost.color = Color.red;
            }
            else
            {
                TxtCoin.color = Color.white;
                TxtCoinCost.color = Color.white;
            }

			BtnGiveUp.onClick.RemoveAllListeners();
			BtnGiveUp.onClick.AddListener(()=>
			{
                CloseSelf();
                UIKit.OpenPanel<UIContinue>();
            });

            BtnRetry.onClick.RemoveAllListeners();
            BtnRetry.onClick.AddListener(() =>
            {
                var utility = this.GetUtility<SaveDataUtility>();
                var coin = utility.GetCoinNum();
                utility.SetCoinNum(coin - 90);
                LevelManager.Instance.RefreshLevel();
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
