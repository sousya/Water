using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:9c0079b6-e0a6-4f3b-b87c-5e16b843d6e0
	public partial class UIRankClear
	{
		public const string Name = "UIRankClear";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtExcellent;
		
		private UIRankClearData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnContinue = null;
			TxtContinue = null;
			TxtExcellent = null;
			
			mData = null;
		}
		
		public UIRankClearData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIRankClearData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIRankClearData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
