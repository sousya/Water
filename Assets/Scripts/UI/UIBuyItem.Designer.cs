using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:261316fd-9b63-4bb9-9664-765b7b871789
	public partial class UIBuyItem
	{
		public const string Name = "UIBuyItem";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTitle;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtNum;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtDesc;
		[SerializeField]
		public UnityEngine.UI.Button BtnBuy;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCost;
		
		private UIBuyItemData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			TxtTitle = null;
			ImgItem = null;
			TxtNum = null;
			TxtDesc = null;
			BtnBuy = null;
			TxtCost = null;
			
			mData = null;
		}
		
		public UIBuyItemData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBuyItemData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBuyItemData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
