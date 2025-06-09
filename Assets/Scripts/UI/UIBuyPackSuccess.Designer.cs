using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:ed47044e-612e-498b-977f-cb1355f48d88
	public partial class UIBuyPackSuccess
	{
		public const string Name = "UIBuyPackSuccess";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		
		private UIBuyPackSuccessData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			BtnContinue = null;
			
			mData = null;
		}
		
		public UIBuyPackSuccessData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBuyPackSuccessData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBuyPackSuccessData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
