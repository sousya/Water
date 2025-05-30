using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:e3d92ea6-2180-4754-8975-f375e8c64675
	public partial class UIBeginSelect
	{
		public const string Name = "UIBeginSelect";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Image ImgBox;
		[SerializeField]
		public UnityEngine.UI.Button BtnInfo;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtProgress;
		[SerializeField]
		public RectTransform UnLimitNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgReward;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		
		private UIBeginSelectData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			ImgBox = null;
			BtnInfo = null;
			ImgProgress = null;
			TxtProgress = null;
			UnLimitNode = null;
			ImgReward = null;
			BtnStart = null;
			
			mData = null;
		}
		
		public UIBeginSelectData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBeginSelectData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBeginSelectData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
