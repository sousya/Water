using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:722c0d0f-234d-47a8-86d9-c4fa14bf6a10
	public partial class UIDefeat
	{
		public const string Name = "UIDefeat";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtFailBg;
		[SerializeField]
		public UnityEngine.UI.Button BtnRetry;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRetry;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIDefeatData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtFailBg = null;
			BtnRetry = null;
			TxtRetry = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIDefeatData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIDefeatData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIDefeatData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
