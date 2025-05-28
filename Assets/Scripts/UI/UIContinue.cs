using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIContinueData : UIPanelData
	{
	}
	public partial class UIContinue : UIPanel, ICanGetUtility
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIContinueData ?? new UIContinueData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
            var coin = CoinManager.Instance.Coin;
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

            BtnContinue.onClick.RemoveAllListeners();
			BtnContinue.onClick.AddListener(() =>
			{
				if (coin >= 90)
				{
					CoinManager.Instance.CostCoin(90, () =>
					{
                        LevelManager.Instance.RefreshLevel();
                    });
                    CloseSelf();
                }
				else
				{
					//»½ÆðÉÌµê
				}
				
            });

			BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
			{
				CloseSelf();
				UIKit.OpenPanel<UIDeleteLife>();
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
