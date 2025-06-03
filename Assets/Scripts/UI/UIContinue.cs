using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIContinueData : UIPanelData
	{
	}
	public partial class UIContinue : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        private const int RESTART_COST = 90;

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
            BtnContinue.onClick.RemoveAllListeners();
            BtnClose.onClick.RemoveAllListeners();
            BtnAddCoin.onClick.RemoveAllListeners();
        }

        private void RegisterBtnEvent()
		{
            BtnContinue.onClick.AddListener(() =>
            {
                if (CoinManager.Instance.Coin >= RESTART_COST)
                {
                    CoinManager.Instance.CostCoin(RESTART_COST, () =>
                    {
                        string _del = $"重置关卡:{this.GetUtility<SaveDataUtility>().GetLevelClear()}," +
                            $"当前关卡进度:{this.GetUtility<SaveDataUtility>().GetLevelClear()}";
                        AnalyticsManager.Instance.SendLevelEvent(_del);
                        LevelManager.Instance.RefreshLevel();
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
                UIKit.OpenPanel<UIDeleteLife>();
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

            TxtCoinCost.color = coin < RESTART_COST ? Color.red : Color.white;
        }
    }
}
