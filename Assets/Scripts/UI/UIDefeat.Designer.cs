using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:06ac506f-a262-4e21-afaf-3f8662066af1
	public partial class UIDefeat
	{
		public const string Name = "UIDefeat";
		
		[SerializeField]
		public UnityEngine.UI.Image ImgStar3;
		[SerializeField]
		public UnityEngine.UI.Button BtnLevel;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnSkip;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtSkip;
		
		private UIDefeatData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ImgStar3 = null;
			BtnLevel = null;
			TxtLevel = null;
			BtnSkip = null;
			TxtSkip = null;
			
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
