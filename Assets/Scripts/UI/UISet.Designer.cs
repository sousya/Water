using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:b72ab71d-161b-488a-8de2-1092c247caae
	public partial class UISet
	{
		public const string Name = "UISet";
		
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtSet;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtReturn;
		[SerializeField]
		public UnityEngine.UI.Button BtnSound;
		[SerializeField]
		public UnityEngine.UI.Button BtnShare;
		[SerializeField]
		public UnityEngine.UI.Button BtnLanguage;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLanguage;
		
		private UISetData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			TxtSet = null;
			BtnReturn = null;
			TxtReturn = null;
			BtnSound = null;
			BtnShare = null;
			BtnLanguage = null;
			TxtLanguage = null;
			
			mData = null;
		}
		
		public UISetData Data
		{
			get
			{
				return mData;
			}
		}
		
		UISetData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UISetData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
