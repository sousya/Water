using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:ce6d29d3-2e97-47fe-a082-7262b3989071
	public partial class UIContinue
	{
		public const string Name = "UIContinue";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnAddCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoinCost;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIContinueData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnAddCoin = null;
			TxtCoin = null;
			BtnContinue = null;
			TxtCoinCost = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIContinueData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIContinueData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIContinueData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
