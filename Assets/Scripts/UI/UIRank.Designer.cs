using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:d4dfbbed-cac1-43e4-a273-421f1adf67f9
	public partial class UIRank
	{
		public const string Name = "UIRank";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnStar;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTitle;
		
		private UIRankData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtTime = null;
			BtnClose = null;
			BtnStar = null;
			TxtTitle = null;
			
			mData = null;
		}
		
		public UIRankData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRankData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRankData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
