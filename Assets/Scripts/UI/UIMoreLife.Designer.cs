using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:7c1627c6-1db6-4563-a2a8-f29d7fb206e3
	public partial class UIMoreLife
	{
		public const string Name = "UIMoreLife";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoinBuy;
		[SerializeField]
		public UnityEngine.UI.Button BtnAD;
		
		private UIMoreLifeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtHeart = null;
			TxtTime = null;
			BtnClose = null;
			BtnCoinBuy = null;
			BtnAD = null;
			
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
