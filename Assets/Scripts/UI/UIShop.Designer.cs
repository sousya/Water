using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:fcf58e82-5be0-42cc-b71a-27e427bd6e43
	public partial class UIShop
	{
		public const string Name = "UIShop";
		
		
		private UIShopData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
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
