using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:640bc6ba-2395-4666-a3fb-055efe501bdd
	public partial class UIMoreLife
	{
		public const string Name = "UIMoreLife";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtNextHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoinBuy;
		[SerializeField]
		public UnityEngine.UI.Button BtnAD;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHeart2;
		
		private UIMoreLifeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtNextHeart = null;
			TxtTime = null;
			BtnClose = null;
			BtnCoinBuy = null;
			BtnAD = null;
			TxtHeart2 = null;
			
			mData = null;
		}
		
		public UIMoreLifeData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIMoreLifeData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIMoreLifeData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
