using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:5fd34537-92a3-4bf0-af6a-017a19d4dacb
	public partial class UIChooseAvatar
	{
		public const string Name = "UIChooseAvatar";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnSave;
		
		private UIChooseAvatarData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			BtnSave = null;
			
			mData = null;
		}
		
		public UIChooseAvatarData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIChooseAvatarData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIChooseAvatarData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
