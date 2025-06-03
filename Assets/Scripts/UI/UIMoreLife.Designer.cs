using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:48e00de7-e916-40d5-92ce-ae715f58d9ed
	public partial class UIMoreLife
	{
		public const string Name = "UIMoreLife";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHpNum;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtDetail;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoinBuy;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoinCost;
		[SerializeField]
		public UnityEngine.UI.Button BtnAD;
		
		private UIMoreLifeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtHpNum = null;
			TxtTime = null;
			TxtDetail = null;
			BtnClose = null;
			BtnCoinBuy = null;
			TxtCoinCost = null;
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
