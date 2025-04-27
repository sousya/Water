using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:2d361118-cf35-4dbb-b101-e4feb9d15ae1
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
		public TMPro.TextMeshProUGUI TxtCoinCost;
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
			TxtCoinCost = null;
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
