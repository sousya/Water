using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;
using System.Collections.Generic;

namespace QFramework.Example
{
	public class UIVictoryData : UIPanelData
	{
	}
	public partial class UIVictory : UIPanel, ICanSendEvent, ICanGetUtility
	{
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
			string _del = $"用户通过关卡:{this.GetUtility<SaveDataUtility>().GetLevelClear() - 1}," +
				$"当前关卡进度:{this.GetUtility<SaveDataUtility>().GetLevelClear()}";
			AnalyticsManager.Instance.SendLevelEvent(_del);
        }

		protected override void OnShow()
		{
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
			//目前不播放
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
			
            UIKit.OpenPanel<UIGetCoin>();
			CloseSelf();
        }
    }
}
