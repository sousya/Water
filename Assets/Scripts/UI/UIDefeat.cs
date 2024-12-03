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
        void Start()
        {
            RegisterEvents();
            SetText();
            //AudioKit.PlayMusic("resources://Audio/bgm");
        }
        void SetText()
		{
			TxtLevel.text = TextManager.Instance.GetConvertText("Text_Playagain");
			TxtSkip.text = TextManager.Instance.GetConvertText("Text_Skip");
        }

		void RegisterEvents()
		{

            BtnLevel.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/btnClick");
                LevelManager.Instance.ReStart();
                UIKit.ClosePanel<UIDefeat>();
            });

            BtnSkip.onClick.AddListener(() =>
            {
                AudioKit.PlaySound("resources://Audio/btnClick");
                UIKit.ClosePanel<UIDefeat>();
                LevelManager.Instance.SkipClear();
                LevelManager.Instance.WaitStart();
            });
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
