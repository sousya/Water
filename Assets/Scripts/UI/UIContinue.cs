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
			BtnContinue.onClick.RemoveAllListeners();
			BtnContinue.onClick.AddListener(() =>
			{
				var utility = this.GetUtility<SaveDataUtility>();
				var coin = utility.GetCoinNum();
				utility.SetCoinNum(coin - 90);
				LevelManager.Instance.RefreshLevel();
                CloseSelf();
            });

			BtnClose.onClick.RemoveAllListeners();
            BtnClose.onClick.AddListener(() =>
			{
				CloseSelf();
				UIKit.OpenPanel<UIDeleteLife>();
            });

            var utility = this.GetUtility<SaveDataUtility>();
            var coin = utility.GetCoinNum();
            TxtCoin.text = coin.ToString();

			if(coin < 90)
			{
				TxtCoin.color = Color.red;
                TxtCoinCost.color = Color.red;
			}
			else
			{
				TxtCoin.color = Color.white;
                TxtCoinCost.color = Color.white;
			}
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
