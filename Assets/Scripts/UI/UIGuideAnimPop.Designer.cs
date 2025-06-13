using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:45e4f501-07f9-4eca-9442-a3a6be9f0393
	public partial class UIGuideAnimPop
	{
		public const string Name = "UIGuideAnimPop";
		
		[SerializeField]
		public UnityEngine.UI.Text TxtGuide;
		[SerializeField]
		public UnityEngine.Animation GuideAni;
		
		private UIGuideAnimPopData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtGuide = null;
			GuideAni = null;
			
			mData = null;
		}
		
		public UIGuideAnimPopData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGuideAnimPopData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGuideAnimPopData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
