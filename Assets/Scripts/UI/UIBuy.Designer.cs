using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:8d8fc893-fb88-441b-96e5-7be5766c2969
	public partial class UIBuy
	{
		public const string Name = "UIBuy";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtSet;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtReturn;
		[SerializeField]
		public UnityEngine.UI.Button BtnBuy;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtPay;
		
		private UIBuyData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtSet = null;
			BtnReturn = null;
			TxtReturn = null;
			BtnBuy = null;
			TxtPay = null;
			
			mData = null;
		}
		
		public UIBuyData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBuyData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBuyData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
