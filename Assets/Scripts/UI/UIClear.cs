using UnityEngine;
using UnityEngine.UI;
using QFramework;
using DG.Tweening;
using System.Collections;

namespace QFramework.Example
{
	public class UIClearData : UIPanelData
    {
	}
	public partial class UIClear : UIPanel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIClearData ?? new UIClearData();
			// please add init code here
		}
        void Start()
        {
            TxtUnlock.font.material.shader = Shader.Find(TxtUnlock.font.material.shader.name);
            RegisterAllEvents();
            SetText();
            TopOnADManager.Instance.RemoveBannerAd();
            AudioKit.PlaySound("resources://Audio/clear");
        }

        void SetUnlock()
        {
            bool showAnimCake = false;
            bool showAnimBg = false;
            var unlockCakes = LevelManager.Instance.nowLevel.unlockCake;
            int count = unlockCakes.Count;
            if (count > 0 && LevelManager.Instance.levelType == 0)
            {
                UnlockNode.gameObject.SetActive(true);
                ImgCake1.gameObject.SetActive(true);
                ImgLight2.gameObject.SetActive(count > 1);

                if (count == 2)
                {
                    ImgCake2.sprite = LevelManager.Instance.cakeShowFloorImgs[unlockCakes[1]];
                }
                ImgCake1.sprite = LevelManager.Instance.cakeShowFloorImgs[unlockCakes[0]];

                TxtUnlock.text = LevelManager.Instance.nowLevel.unlockNow + "/" + LevelManager.Instance.nowLevel.unlockNeed;
                TxtUnlockLevel.text = TextManager.Instance.GetConvertText("Text_Level") +
                    (LevelManager.Instance.levelId + LevelManager.Instance.nowLevel.unlockNeed - LevelManager.Instance.nowLevel.unlockNow) +
                    TextManager.Instance.GetConvertText("Text_CakeUnlock");
                float fillamount = LevelManager.Instance.nowLevel.unlockNow / (LevelManager.Instance.nowLevel.unlockNeed * 1f);
                float lastAmount = 0;
                if(LevelManager.Instance.nowLevel.unlockNow > 1)
                {
                    lastAmount = (LevelManager.Instance.nowLevel.unlockNow - 1) / (LevelManager.Instance.nowLevel.unlockNeed * 1f);
                }
                ImgScroll.fillAmount = lastAmount;
                ImgScroll.DOFillAmount(fillamount, 0.7f);

                if (LevelManager.Instance.levels[LevelManager.Instance.levelId - 2].unlockNow == LevelManager.Instance.levels[LevelManager.Instance.levelId - 2].unlockNeed && LevelManager.Instance.levelId > 3)
                {
                    showAnimCake = true;
                    ImgFlyCake.sprite = ImgCake1.sprite;
                    BtnLevel.gameObject.SetActive(false);
                }

            }
            else
            {
                UnlockNode.gameObject.SetActive(false);
            }

            if(LevelManager.Instance.levelId > 113 || LevelManager.Instance.levelType != 0)
            {
                UnlockBgNode.gameObject.SetActive(false);
            }
            else
            {
                UnlockBgNode.gameObject.SetActive(true);
                float fillamount = (LevelManager.Instance.levelId - 1) % 8;
                if(fillamount == 0)
                {
                    fillamount = 8;
                }
                float lastAmount = (LevelManager.Instance.levelId - 2) % 8;

                TxtUnlockBg.text = fillamount + "/" +8;
                TxtUnlockBgLevel.text = TextManager.Instance.GetConvertText("Text_Level") +
                     (LevelManager.Instance.levelId + 8 - fillamount) +
                    TextManager.Instance.GetConvertText("Text_CakeUnlock");
                int useBg = (LevelManager.Instance.levelId - 1) / 8;
                ImgBgScroll.fillAmount = lastAmount / 8;
                ImgBgScroll.DOFillAmount(fillamount / 8, 1);

                if (fillamount == 8)
                {
                    ImgBg.sprite = LevelManager.Instance.levelBgs[useBg];
                    showAnimBg = true;
                    BtnLevel.gameObject.SetActive(false);
                    ImgFlyBg.sprite = ImgBg.sprite;
                }
                else
                {
                    ImgBg.sprite = LevelManager.Instance.levelBgs[useBg + 1];
                }
            }

            if (LevelManager.Instance.levelType == 0)
            {
                if (showAnimCake)
                {
                    StartCoroutine(ShowCakeAnim());
                }
                else if (showAnimBg)
                {
                    StartCoroutine(ShowBgAnim());
                }
            }
        }

        IEnumerator ShowCakeAnim()
        {
            yield return new WaitForSeconds(1);
            ImgLight1.gameObject.SetActive(false);
            ImgLight2.gameObject.SetActive(false);
            ImgFlyCake.gameObject.SetActive(true);
        }

        IEnumerator ShowBgAnim()
        {
            yield return new WaitForSeconds(1);
            ImgBg.gameObject.SetActive(false);
            ImgFlyBg.gameObject.SetActive(true);
        }

        void RegisterAllEvents()
        {
            this.RegisterEvent<RefreshUITextEvent>(
                e =>
                {
                    SetText();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            BtnLevel.onClick.AddListener(WaitStart);
            BtnContinue.onClick.AddListener(WaitStart);
            BtnRetry.onClick.AddListener(() =>
            {
                TopOnADManager.Instance.rewardAction = Restart;
                TopOnADManager.Instance.ShowRewardAd();
            });
        }

        void Restart()
        {
            LevelManager.Instance.levelId--;
            WaitStart();
            LevelManager.Instance.moreMoveNum += 5;
            this.SendEvent<MoveCakeEvent>();
        }

        void WaitStart()
        {
            LevelManager.Instance.isClear = true;
            LevelManager.Instance.fxNode.SetActive(false);

            if (LevelManager.Instance.levelType == 1)
            {
                LevelManager.Instance.ClearChallengeLevel();
            }
            else if(LevelManager.Instance.levelType == 0)
            {
                AudioKit.PlaySound("resources://Audio/btnClick");
                LevelManager.Instance.WaitStart();
            }
            else if (LevelManager.Instance.levelType == 2)
            {
                LevelManager.Instance.ClearOrderLevel();
            }
            else if (LevelManager.Instance.levelType == 3)
            {
                LevelManager.Instance.ClearJigsawLevel();
            }
        }

        void SetText()
        {
            TxtLevel.text = TextManager.Instance.GetConvertText("Text_NextLevel");
            TxtRestart.text = TextManager.Instance.GetConvertText("Text_Playagain");
            TxtAddStep.text = TextManager.Instance.GetConvertText("Text_AddStepFive");
            TxtContinue.text = TextManager.Instance.GetConvertText("Text_Continue");
            var unlockCakes = LevelManager.Instance.nowLevel.unlockCake;
            int count = unlockCakes.Count;
            if (count > 0)
            {
                TxtUnlock.text = LevelManager.Instance.nowLevel.unlockNow + "/" + LevelManager.Instance.nowLevel.unlockNeed;
                TxtUnlockLevel.text = TextManager.Instance.GetConvertText("Text_Level") +
                    (LevelManager.Instance.levelId + LevelManager.Instance.nowLevel.unlockNeed - LevelManager.Instance.nowLevel.unlockNow) +
                    TextManager.Instance.GetConvertText("Text_CakeUnlock");
            }

            if (LevelManager.Instance.levelId > 88)
            {
            }
            else
            {
                float fillamount = (LevelManager.Instance.levelId - 1) % 8;
                if (fillamount == 0)
                {
                    fillamount = 8;
                }
                //float lastAmount = (LevelManager.Instance.levelId - 2) % 8;

                TxtUnlockBg.text = fillamount + "/" + 8;
                TxtUnlockBgLevel.text = TextManager.Instance.GetConvertText("Text_Level") +
                     (LevelManager.Instance.levelId + 8 - fillamount) +
                    TextManager.Instance.GetConvertText("Text_CakeUnlock");
            }
        }

        void ShowStar()
        {
            if (LevelManager.Instance.lastStar == 0)
            {
                ThreeNode.gameObject.SetActive(true);
                NoThreeNode.gameObject.SetActive(false);
                ImgStar1.gameObject.SetActive(true);
                ImgStar2.gameObject.SetActive(true);
                ImgStar3.gameObject.SetActive(true);
            }
            else if (LevelManager.Instance.lastStar == 1)
            {
                ThreeNode.gameObject.SetActive(false);
                NoThreeNode.gameObject.SetActive(true);
                ImgStar1.gameObject.SetActive(true);
                ImgStar2.gameObject.SetActive(false);
                ImgStar3.gameObject.SetActive(false);
            }
            else if (LevelManager.Instance.lastStar == 2)
            {
                ThreeNode.gameObject.SetActive(false);
                NoThreeNode.gameObject.SetActive(true);
                ImgStar1.gameObject.SetActive(true);
                ImgStar2.gameObject.SetActive(true);
                ImgStar3.gameObject.SetActive(false);
            }
            else if (LevelManager.Instance.lastStar == 3)
            {
                ThreeNode.gameObject.SetActive(true);
                NoThreeNode.gameObject.SetActive(false);
                ImgStar1.gameObject.SetActive(true);
                ImgStar2.gameObject.SetActive(true);
                ImgStar3.gameObject.SetActive(true);
            }
        }

        protected override void OnOpen(IUIData uiData = null)
		{
        }

        protected override void OnShow()
        {
            ImgFlyCake.gameObject.SetActive(false);
            ImgFlyBg.gameObject.SetActive(false);

            ImgLight1.gameObject.SetActive(true);
            ImgBg.gameObject.SetActive(true);

            BtnLevel.gameObject.SetActive(true);
            SetUnlock();
            ShowStar();
        }

        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}
	}
}
