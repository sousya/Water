using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:1a92ed74-7c67-4198-9b04-32429d01bdca
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
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCost;
		
		private UIBuyItemData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			TxtTitle = null;
			ImgItem = null;
			TxtDesc = null;
			BtnStart = null;
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
