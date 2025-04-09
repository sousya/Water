using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:cc39a6fc-47aa-4de9-9b20-9db66a27cc52
	public partial class UIRetry
	{
		public const string Name = "UIRetry";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.RectTransform ImgReward;
		[SerializeField]
		public UnityEngine.UI.Button BtnGiveUp;
		[SerializeField]
		public UnityEngine.UI.Button BtnRetry;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRetry;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoinCost;
		
		private UIRetryData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnCoin = null;
			TxtCoin = null;
			ImgReward = null;
			BtnGiveUp = null;
			BtnRetry = null;
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
