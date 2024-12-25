using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:2e031f9d-9220-4388-b943-57ec3acb639d
	public partial class UIBegin
	{
		public const string Name = "UIBegin";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnRefresh;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRefreshNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddBottle;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddBottleNum;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		
		private UIBeginData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnRefresh = null;
			TxtRefreshNum = null;
			BtnAddBottle = null;
			TxtAddBottleNum = null;
			TxtLevel = null;
			
			mData = null;
		}
		
		public UIBeginData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBeginData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBeginData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
