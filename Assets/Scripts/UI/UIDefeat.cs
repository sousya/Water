using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UIDefeatData : UIPanelData
	{
	}
	public partial class UIDefeat : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIDefeatData ?? new UIDefeatData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BindBtn();

        }
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
        }
        void BindBtn()
        {
            BtnClose.onClick.RemoveAllListeners();
            BtnRetry.onClick.RemoveAllListeners();

            BtnRetry.onClick.AddListener(() =>
            {
                LevelManager.Instance.RefreshLevel();
                UIKit.HidePanel<UIDefeat>();
            });

            BtnClose.onClick.AddListener(() =>
            {
                UIKit.HidePanel<UIDefeat>();
            });

        }
    }
}
