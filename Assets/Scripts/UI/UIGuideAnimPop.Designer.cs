using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:ed5d6508-3b64-4103-a9ef-c25f9465192e
	public partial class UIGuideAnimPop
	{
		public const string Name = "UIGuideAnimPop";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI GuideText;
		[SerializeField]
		public UnityEngine.Animation GuideAni;
		
		private UIGuideAnimPopData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			GuideText = null;
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
