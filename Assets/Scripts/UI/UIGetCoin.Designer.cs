using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:12d4f85f-679b-49e7-99b7-5b054705fe29
	public partial class UIGetCoin
	{
		public const string Name = "UIGetCoin";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRetry;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIGetCoinData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtCoin = null;
			TxtLevel = null;
			BtnContinue = null;
			TxtRetry = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIGetCoinData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGetCoinData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGetCoinData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
