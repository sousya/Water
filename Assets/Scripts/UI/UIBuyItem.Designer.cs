using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:4244ea1c-96ad-4cb2-a99e-09d360e341a2
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
