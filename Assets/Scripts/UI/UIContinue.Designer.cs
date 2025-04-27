using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:e2c1517f-56a2-49e0-a68d-63981d594c2f
	public partial class UIContinue
	{
		public const string Name = "UIContinue";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnCoin;
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
			BtnCoin = null;
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
