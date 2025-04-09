using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections;

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
		}

		protected override void OnShow()
		{
			BindButton();
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

		void BindButton()
		{
			BtnClose.onClick.AddListener(() =>
			{
				this.SendEvent<LevelClearEvent>();
				CloseSelf();
			});
		}

		void ShowAnim()
		{
			AnimGo.Play("victoryAnim");
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
