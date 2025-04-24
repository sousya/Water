using UnityEngine;
using UnityEngine.UI;
using QFramework;
using GameDefine;

namespace QFramework.Example
{
	public class UIMoreLifeData : UIPanelData
	{
	}
	public partial class UIMoreLife : UIPanel, ICanGetUtility, ICanRegisterEvent
    {
        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
        protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIMoreLifeData ?? new UIMoreLifeData();
			// please add init code here
		}

        void CheckVitality()
        {
            int lastVitalityNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();
            long recoveryTime = this.GetUtility<SaveDataUtility>().GetVitalityTime() + (5 - lastVitalityNum) * GameConst.RecoveryTime;
            long timeOffset = recoveryTime - this.GetUtility<SaveDataUtility>().GetNowTime();
            if(timeOffset < 0)
            {
                timeOffset = 0;
            }   
            var e = new VitalityTimeChangeEvent() { timeOffset = timeOffset };
            CheckVitality(e);
        }

        void CheckVitality(VitalityTimeChangeEvent e)
        {
            //打开界面的时候注册监听事件，导致界面关闭时出现问题
            int heartNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();
            Debug.Log(heartNum);
            TxtHeart.text = heartNum.ToString();

             var timeOffset = e.timeOffset % GameConst.RecoveryTime;
            if (timeOffset == 0)
            {
                timeOffset = GameConst.RecoveryTime;
            }
            string minuteStr = (int)(timeOffset / 60) + "";
            string secondStr = timeOffset % 60 + "";
            if (minuteStr.Length == 1)
            {
                minuteStr = "0" + minuteStr;
            }
            if (secondStr.Length == 1)
            {
                secondStr = "0" + secondStr;
            }
            TxtTime.text = minuteStr + ":" + secondStr;

            if(e.timeOffset == 0 && heartNum == GameConst.MaxVitality)
            {
                TxtTime.text = "00:00";
            }
        }

        protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BindBtn();
			RegisterEvent();
            CheckVitality();
        }

        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

        void RegisterEvent()
        {
            Debug.Log("监听体力恢复");
            this.RegisterEvent<VitalityTimeChangeEvent>(e =>
            {
                CheckVitality(e);
            });
        }

        void BindBtn()
        {
            BtnClose.onClick.AddListener(() =>
            {
                this.CloseSelf();
            });

            BtnCoinBuy.onClick.AddListener(() =>
            {
                Debug.Log(CoinManager.Instance == null);
                CoinManager.Instance.BuyVitality();
            });

            BtnAD.onClick.AddListener(() =>
            {
                TopOnADManager.Instance.ShowRewardAd();
                TopOnADManager.Instance.rewardAction = () =>
                {
                    this.GetUtility<SaveDataUtility>().AddVitalityNum(1);
                };
            });
        }
    }
}
