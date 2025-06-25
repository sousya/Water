using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using GameDefine;

namespace QFramework.Example
{
	public class UIVictoryData : UIPanelData
	{
	}
	public partial class UIVictory : UIPanel, ICanSendEvent, ICanGetUtility, ICanGetModel
    {
		private int mLastRankingScore;
		private bool mRankingEnd;

        public IArchitecture GetArchitecture()
		{
			return GameMainArc.Interface;
		}

		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIVictoryData ?? new UIVictoryData();
			// please add init code here
		}

		protected override void OnOpen(IUIData uiData = null)
		{
			string _del = $"�û�ͨ���ؿ�:{this.GetUtility<SaveDataUtility>().GetLevelClear() - 1}," +
				$"��ǰ�ؿ�����:{this.GetUtility<SaveDataUtility>().GetLevelClear()}";
			AnalyticsManager.Instance.SendLevelEvent(_del);
        }

		protected override void OnShow()
		{
            //ͨ�����߹ؿ�����ʤ�
            if (this.GetUtility<SaveDataUtility>().GetLevelClear() == 8)
            {
                StringEventSystem.Global.Send("StartPotionActivity");
                //�������а�
                CountDownTimerManager.Instance.StartTimer(GameConst.RANKA_ACTIVITY_SIGN, 1440f);
            }
            //�������ͣ��ʱ����
            mRankingEnd = CountDownTimerManager.Instance.IsTimerFinished(GameConst.RANKA_ACTIVITY_SIGN);

            //��ʤ�����/���а���״̬
            if (!CountDownTimerManager.Instance.IsTimerFinished(GameConst.POTION_ACTIVITY_SIGN)
                || !mRankingEnd)
            {
                var potionActivityModel = this.GetModel<PotionActivityModel>();
				mLastRankingScore = potionActivityModel.PotionActivityTotalGoal;
                potionActivityModel.AddPotionActivityGoal();
            }

            ShowAnim();
			TxtLevel.text = (this.GetUtility<SaveDataUtility>().GetLevelClear() - 1).ToString();
			StartCoroutine(WaitClose());
        }

		protected override void OnHide()
		{
		}

		protected override void OnClose()
		{
		}

		void ShowAnim()
		{
			//Ŀǰ������
			//AnimGo.Play("victoryAnim");
			HornGo1.Play("hornRotation");
			HornGo2.Play("hornRotation");
			HornGo3.Play("hornRotation");
			HornGo4.Play("hornRotation");

			HornSpine1.AnimationState.SetAnimation(0, "animation", false);
			HornSpine2.AnimationState.SetAnimation(0, "animation", false);
			HornSpine3.AnimationState.SetAnimation(0, "animation", false);
			HornSpine4.AnimationState.SetAnimation(0, "animation", false);
		}

		IEnumerator WaitClose()
        {
			yield return new WaitForSeconds(3f);
			if (!mRankingEnd)
			{
				UIKit.OpenPanel<UIRankA>(new UIRankAData { LastRankScore = mLastRankingScore});
			}else
				UIKit.OpenPanel<UIGetCoin>();
			CloseSelf();
        }
    }
}
