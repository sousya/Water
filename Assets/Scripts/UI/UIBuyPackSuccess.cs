using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIBuyPackSuccessData : UIPanelData
	{
	}
	public partial class UIBuyPackSuccess : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIBuyPackSuccessData ?? new UIBuyPackSuccessData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BtnClose.onClick.AddListener(() =>
            {
                CloseSelf();
            });

			BtnContinue.onClick.AddListener(() =>
            {
				CloseSelf();
            });
        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
			BtnContinue.onClick.RemoveAllListeners();
            BtnClose.onClick.RemoveAllListeners();
        }
	}
}
