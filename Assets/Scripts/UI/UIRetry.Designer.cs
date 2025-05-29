using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:27bcd359-f7b2-4092-8572-c42c539dd65b
	public partial class UIRetry
	{
		public const string Name = "UIRetry";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddCoin;
		[SerializeField]
		public UnityEngine.UI.Button BtnGiveUp;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddBottle;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRetry;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoinCost;
		
		private UIRetryData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			TxtCoin = null;
			BtnAddCoin = null;
			BtnGiveUp = null;
			BtnAddBottle = null;
			TxtRetry = null;
			TxtCoinCost = null;
			
			mData = null;
		}
		
		public UIRetryData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRetryData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRetryData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
