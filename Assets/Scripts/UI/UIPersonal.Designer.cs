using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:20ecddb4-65c0-4a03-a5b3-92d06020b27b
	public partial class UIPersonal
	{
		public const string Name = "UIPersonal";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnHead;
		[SerializeField]
		public UnityEngine.UI.Image ImgHeadFrame;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		
		private UIPersonalData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnHead = null;
			ImgHeadFrame = null;
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
