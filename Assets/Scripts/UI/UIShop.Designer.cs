using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:5aabac31-7da8-40ee-b33a-6ad8200a5c64
	public partial class UIShop
	{
		public const string Name = "UIShop";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIShopData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			
			mData = null;
		}
		
		public UIShopData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIShopData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIShopData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
