using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:0a57dc8d-fe30-4bec-acf7-da0844f3522f
	public partial class UIBegin
	{
		public const string Name = "UIBegin";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnRefresh;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRefreshNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnRemoveHide;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddBottleNum;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddBottle;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddHalfBottle;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddRemoveAll;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStart;
		[SerializeField]
		public UnityEngine.UI.Button BtnArea;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtArea;
		[SerializeField]
		public UnityEngine.UI.Button BtnHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHeart;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.UI.Button BeginMenuButton1;
		[SerializeField]
		public UnityEngine.UI.Button BeginMenuButton2;
		[SerializeField]
		public UnityEngine.UI.Button BeginMenuButton3;
		[SerializeField]
		public UnityEngine.UI.Button BtnMenu;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		
		private UIBeginData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnRefresh = null;
			TxtRefreshNum = null;
			BtnRemoveHide = null;
			TxtAddBottleNum = null;
			BtnAddBottle = null;
			BtnAddHalfBottle = null;
			BtnAddRemoveAll = null;
			TxtLevel = null;
			BtnStart = null;
			TxtStart = null;
			BtnArea = null;
			TxtArea = null;
			BtnHeart = null;
			TxtHeart = null;
			BtnCoin = null;
			TxtCoin = null;
			BeginMenuButton1 = null;
			BeginMenuButton2 = null;
			BeginMenuButton3 = null;
			BtnMenu = null;
			BtnReturn = null;
			
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
