using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIPersonalData : UIPanelData
	{
	}
	public partial class UIPersonal : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIPersonalData ?? new UIPersonalData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			BtnClose.onClick.RemoveAllListeners();
			BtnClose.onClick.AddListener(() =>
            {
                UIKit.ClosePanel(this);
            });
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
