using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UILessStarData : UIPanelData
	{
	}
	public partial class UILessStar : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UILessStarData ?? new UILessStarData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BtnClose.onClick.RemoveAllListeners();
			BtnClose.onClick.AddListener(() =>
			{
				CloseSelf();
			});

			BtnStart.onClick.RemoveAllListeners();
			BtnStart.onClick.AddListener(() =>
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
