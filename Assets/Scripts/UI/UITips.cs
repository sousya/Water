using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UITipsData : UIPanelData
	{
	}
	public partial class UITips : UIPanel, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        public List<Sprite> changeTab;
        public List<string> changeText;

        string nowString;

        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UITipsData ?? new UITipsData();
			// please add init code here
		}

        private void Start()
        {
            nowString = changeText[0];
            RegisterAllEvents();
            SetText();
        }

        void RegisterAllEvents()
        {
            this.RegisterEvent<RefreshUITextEvent>(
                e =>
                {
                    SetText();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            BtnBomb.onClick.AddListener(()=>
            {
                ChangeTab(0);
                ImgBomb.gameObject.SetActive(true);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnLock.onClick.AddListener(()=>
            {
                ChangeTab(1);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(true);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnConnect.onClick.AddListener(()=>
            {
                ChangeTab(2);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(true);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnFixed.onClick.AddListener(()=>
            {
                ChangeTab(3);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(true);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnStepHide.onClick.AddListener(()=>
            {
                ChangeTab(4);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(true);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnAllHide.onClick.AddListener(()=>
            {
                ChangeTab(5);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(true);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnThreeEmpty.onClick.AddListener(()=>
            {
                ChangeTab(6);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(true);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnOneColor.onClick.AddListener(()=>
            {
                ChangeTab(7);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(true);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnMoreLayers.onClick.AddListener(()=>
            {
                ChangeTab(8);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(true);
                ImgCountDown.gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnCountDown.onClick.AddListener(()=>
            {
                ChangeTab(9);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown.gameObject.SetActive(true);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnLeftLevel.onClick.AddListener(()=>
            {
                ChangeTab(10);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown .gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(true);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnCombine.onClick.AddListener(()=>
            {
                ChangeTab(11);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown .gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(true);
                ImgExplore.gameObject.SetActive(false);
            });
            BtnExplore.onClick.AddListener(()=>
            {
                ChangeTab(12);
                ImgBomb.gameObject.SetActive(false);
                ImgLock.gameObject.SetActive(false);
                ImgConnect.gameObject.SetActive(false);
                ImgFixed.gameObject.SetActive(false);
                ImgStepHide.gameObject.SetActive(false);
                ImgAllHide.gameObject.SetActive(false);
                ImgThreeEmpty.gameObject.SetActive(false);
                ImgOneColor.gameObject.SetActive(false);
                ImgMoreLayers.gameObject.SetActive(false);
                ImgCountDown .gameObject.SetActive(false);
                ImgLeftLevel.gameObject.SetActive(false);
                ImgCombine.gameObject.SetActive(false);
                ImgExplore.gameObject.SetActive(true);
            });
            BtnReturn.onClick.AddListener(() =>
            {
                UIKit.ClosePanel<UITips>();
            });
        }

        void ChangeTab(int idx)
        {
            ImgShow.sprite = changeTab[idx];
            nowString = changeText[idx];
            TxtContent.text = TextManager.Instance.GetConvertText(nowString);
        }

        void SetText()
		{
            TxtRank.text = TextManager.Instance.GetConvertText("Text_TipRank");
            TxtOrder.text = TextManager.Instance.GetConvertText("Text_TipOrder");
            TxtJigsaw.text = TextManager.Instance.GetConvertText("Text_TipJigsaw");
            TxtChallenge.text = TextManager.Instance.GetConvertText("Text_TipChallenge");
            TxtExplore.text = TextManager.Instance.GetConvertText("Text_TipWait");
            TxtBomb.text = TextManager.Instance.GetConvertText("Text_Bomb");
            TxtLock.text = TextManager.Instance.GetConvertText("Text_Lock");
            TxtFixed.text = TextManager.Instance.GetConvertText("Text_FixedPosition");
            TxtConnect.text = TextManager.Instance.GetConvertText("Text_Connect");
            TxtStepHide.text = TextManager.Instance.GetConvertText("Text_StepHide");
            TxtAllHide.text = TextManager.Instance.GetConvertText("Text_AllHide");
            TxtThreeEmpty.text = TextManager.Instance.GetConvertText("Text_ThreeEmpty");
            TxtOneColor.text = TextManager.Instance.GetConvertText("Text_LockColor");
            TxtMoreLayers.text = TextManager.Instance.GetConvertText("Text_MoreLayers");
            TxtCountDown.text = TextManager.Instance.GetConvertText("Text_CountDown");
            TxtLeftLevel.text = TextManager.Instance.GetConvertText("Text_LeftLevel");
            TxtCombine.text = TextManager.Instance.GetConvertText("Text_CombineLevel");
            TxtExploring.text = TextManager.Instance.GetConvertText("Text_TipWait");
            TxtContent.text = TextManager.Instance.GetConvertText(nowString);
            TxtEvent.text = TextManager.Instance.GetConvertText("Text_Event");
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
