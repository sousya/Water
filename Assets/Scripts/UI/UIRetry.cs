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
            var coin = CoinManager.Instance.Coin;
            TxtCoin.text = coin.ToString();

            if (coin < 900)
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
                //退出中断连胜
                this.GetUtility<SaveDataUtility>().SetCountinueWinNum(0);
                UIKit.OpenPanel<UIContinue>();
            });

            BtnRetry.onClick.RemoveAllListeners();
            BtnRetry.onClick.AddListener(() =>
            {
                if (coin >= 900)
                {
                    CoinManager.Instance.CostCoin(900, () =>
                    {
                        //增加管子
                        LevelManager.Instance.AddBottle(false,null);
                    });
                    CloseSelf();
                }
                else
                {
                    //唤起商店
                }
            });

            BtnClose.onClick.AddListener(() =>
            {
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
