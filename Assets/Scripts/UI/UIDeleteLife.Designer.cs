using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:55763aa1-1f29-4d76-aef2-5d6610feef94
	public partial class UIDeleteLife
	{
		public const string Name = "UIDeleteLife";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnQuit;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIDeleteLifeData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnQuit = null;
			BtnClose = null;
			
			mData = null;
		}
		
		public UIDeleteLifeData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIDeleteLifeData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIDeleteLifeData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
