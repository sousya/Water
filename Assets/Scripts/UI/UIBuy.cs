using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIBuyData : UIPanelData
	{
	}
	public partial class UIBuy : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBuyData ?? new UIBuyData();
			// please add init code here
		}
        void Start()
        {
            RegisterEvent();
            SetText();
        }
        void RegisterEvent()
        {
            BtnReturn.onClick.AddListener(() =>
            {
                UIKit.ClosePanel<UIBuy>();
                AudioKit.PlaySound("resources://Audio/btnClick");
            });

            BtnBuy.onClick.AddListener(() =>
            {
                ShopManager.Instance.BuyNoAD();
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
        }
        void SetText()
        {
            TxtSet.text = TextManager.Instance.GetConvertText("Text_Buy");
            TxtReturn.text = TextManager.Instance.GetConvertText("Text_Return");
            TxtPay.text = TextManager.Instance.GetConvertText("Text_Pay");
        }
        protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
