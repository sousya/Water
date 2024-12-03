using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UIJudgeData : UIPanelData
	{
	}
	public partial class UIJudge : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        [SerializeField]
        List<JudgeStarCtrl> starList = new List<JudgeStarCtrl>();
        [SerializeField]
        List<GameObject> showStar = new List<GameObject>();

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIJudgeData ?? new UIJudgeData();
			// please add init code here
		}
		
		protected override void OnOpen(IUIData uiData = null)
		{
			for(int i = 0; i < starList.Count; i++)
			{
				starList[i].idx = i + 1;
				starList[i].showStar = showStar;
            }
		}

        public void Start()
        {
            RegisterEvent();
            SetText();
        }

        void RegisterEvent()
        {
            this.RegisterEvent<RefreshUITextEvent>(
               e =>
               {
                   SetText();

               }).UnRegisterWhenGameObjectDestroyed(gameObject);

            BtnClose.onClick.AddListener(() =>
			{
                UIKit.HidePanel<UIJudge>();
            });
        }

        void SetText()
        {
            TxtJudge.text = TextManager.Instance.GetConvertText("Text_Judge");

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
