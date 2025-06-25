using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:84a3f421-b2cc-4b2f-8d73-472b08703cc2
	public partial class UIRankA
	{
		public const string Name = "UIRankA";
		
		[SerializeField]
		public UnityEngine.UI.ScrollRect RankScroll;
		
		private UIRankAData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			RankScroll = null;
			
			mData = null;
		}
		
		public UIRankAData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRankAData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRankAData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
