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
                if (CoinManager.Instance.Coin >= GameDefine.GameConst.ADD_BOTTLE_COST)
                {
                    //���ӹ���
                    LevelManager.Instance.AddBottle(false, () =>
                    {
                        CoinManager.Instance.CostCoin(GameDefine.GameConst.ADD_BOTTLE_COST);
                    });
                    CloseSelf();
                }
                else
                {
                    //�����̵�
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
            TxtCoinCost.color = coin < GameDefine.GameConst.ADD_BOTTLE_COST ? Color.red : Color.white;
        }
    }
}
