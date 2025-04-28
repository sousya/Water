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

        #region obsolete
        //关于倒计时文本的更新，可以直接获取HealthManager的RecoverTimerStr
        /*void CheckVitality()
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
        }*/

        /*void CheckVitality(VitalityTimeChangeEvent e)
        {
            //打开界面的时候注册监听事件，导致界面关闭时出现问题
            int heartNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();
            //Debug.Log(heartNum);
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
        }*/

        #endregion

        protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
			BindBtn();
            //RegisterEvent();//弃用
            //CheckVitality();//弃用
            TxtNextHeart.text = HealthManager.Instance.CurRecoverySlot.ToString();
        }

        protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
        }

        #region obsolete
        /* private void RegisterEvent()
        {
            Debug.Log("监听体力恢复");

            //弃用
            this.RegisterEvent<VitalityTimeChangeEvent>(e =>
            {
                CheckVitality(e);
            });
        }*/
        #endregion

        private void BindBtn()
        {
            var coin = CoinManager.Instance.Coin;
            TxtCoinCost.color = coin < 900 ? Color.red : Color.white;

            BtnClose.onClick.AddListener(() =>
            {
                this.CloseSelf();
            });

            BtnCoinBuy.onClick.AddListener(() =>
            {
                if (HealthManager.Instance.UsedHp > 0)
                {
                    CoinManager.Instance.CostCoin(900, () =>
                    {
                        HealthManager.Instance.SetNowHpToMax();
                        TxtCoinCost.color = CoinManager.Instance.Coin < 900 ? Color.red : Color.white;
                    });
                }
            });

            BtnAD.onClick.AddListener(() =>
            {
                //放在这是修改广告回调奖励？
                TopOnADManager.Instance.rewardAction = () =>
                {
                    //看广告恢复体力，是恢复一点体力还是恢复满？
                    //this.GetUtility<SaveDataUtility>().AddVitalityNum(1);
                    HealthManager.Instance.AddHp();
                };
                TopOnADManager.Instance.ShowRewardAd();
            });
        }

        private void Update()
        {
            TxtTime.text = HealthManager.Instance.RecoverTimerStr;
            TxtNextHeart.text = HealthManager.Instance.CurRecoverySlot.ToString();
        }
    }
}
