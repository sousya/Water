using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIRetryData : UIPanelData
	{
	}
	public partial class UIRetry : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        private const int ADD_BOTTLE_COST = 900;

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
            SetCoin();

            RegisterBtnEvent();

            this.RegisterEvent<CoinChangeEvent>(e =>
            {
                SetCoin();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
            BtnGiveUp.onClick.RemoveAllListeners();
            BtnAddBottle.onClick.RemoveAllListeners();
            BtnClose.onClick.RemoveAllListeners();
            BtnAddCoin.onClick.RemoveAllListeners();
        }

        private void RegisterBtnEvent()
        {
            BtnGiveUp.onClick.AddListener(() =>
            {
                CloseSelf();
                UIKit.OpenPanel<UIContinue>();
            });

            BtnAddBottle.onClick.AddListener(() =>
            {
                if (CoinManager.Instance.Coin >= ADD_BOTTLE_COST)
                {
                    CoinManager.Instance.CostCoin(ADD_BOTTLE_COST, () =>
                    {
                        //增加管子
                        LevelManager.Instance.AddBottle(false, null);
                    });
                    CloseSelf();
                }
                else
                {
                    //唤起商店
                    UIKit.OpenPanel<UIShop>();
                }
            });

            BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

            BtnAddCoin.onClick.AddListener(() =>
            {
                UIKit.OpenPanel<UIShop>();
            });
        }

        private void SetCoin()
        {
            var coin = CoinManager.Instance.Coin;
            TxtCoin.text = coin.ToString();
            TxtCoinCost.color = coin < ADD_BOTTLE_COST ? Color.red : Color.white;
        }
    }
}
