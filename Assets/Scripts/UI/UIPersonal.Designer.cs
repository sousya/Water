using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:58f94f00-79b1-4eb1-a908-bbfcb0d8f269
	public partial class UIPersonal
	{
		public const string Name = "UIPersonal";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIPersonalData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			
			mData = null;
		}
		
		public UIPersonalData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIPersonalData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIPersonalData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
