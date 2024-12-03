using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UISetData : UIPanelData
	{
	}
	public partial class UISet : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UISetData ?? new UISetData();
            // please add init code here
        }
        void Start()
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

            BtnLanguage.onClick.AddListener(() =>
            {
                if (TextManager.Instance.GetLanguage() == "ZH")
                {
                    TextManager.Instance.ChangeLanguage(GameDefine.LanguageType.en);
                }
                else if (TextManager.Instance.GetLanguage() == "EN")
                {
                    TextManager.Instance.ChangeLanguage(GameDefine.LanguageType.zh);
                }
                AudioKit.PlaySound("resources://Audio/btnClick");
            });

            BtnSound.onClick.AddListener(() =>
            {
                if(!LevelManager.Instance.musicOn)
                {
                    AudioKit.Settings.IsSoundOn.Value = true;
                    AudioKit.Settings.IsMusicOn.Value = true;
                    AudioKit.Settings.IsVoiceOn.Value = true;
                }
                else
                {
                    AudioKit.Settings.IsSoundOn.Value = false;
                    AudioKit.Settings.IsMusicOn.Value = false;
                    AudioKit.Settings.IsVoiceOn.Value = false;
                }
            });

            //BtnShare.onClick.AddListener(() =>
            //{

            //});

            BtnReturn.onClick.AddListener(() =>
            {
                UIKit.HidePanel<UISet>();
                AudioKit.PlaySound("resources://Audio/btnClick");
            });
        }
        void SetText()
        {
            TxtLanguage.text = this.GetUtility<SaveDataUtility>().GetSelectLanguage();
            TxtSet.text = TextManager.Instance.GetConvertText("Text_Set");
            TxtReturn.text = TextManager.Instance.GetConvertText("Text_Return");
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
