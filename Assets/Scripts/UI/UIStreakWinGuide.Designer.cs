using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:3fe93bac-1acf-4f79-9f78-e48611fc31e0
	public partial class UIStreakWinGuide
	{
		public const string Name = "UIStreakWinGuide";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnPlay;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIStreakWinGuideData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnPlay = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIStreakWinGuideData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIStreakWinGuideData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIStreakWinGuideData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
