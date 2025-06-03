using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:c93fafa2-64a7-498e-9b83-a9e57b6c5df8
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
		public UnityEngine.UI.Button BtnClose;
		
		private UIGetCoinData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtCoin = null;
			TxtLevel = null;
			BtnContinue = null;
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
