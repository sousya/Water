using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:aee74efa-d555-4165-8bd9-b0dc44441160
	public partial class UIUnlockScene
	{
		public const string Name = "UIUnlockScene";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnBox;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgressBg;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtImgprogress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtComingSoon;
		[SerializeField]
		public UnityEngine.RectTransform ImgReward;
		
		private UIUnlockSceneData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			BtnBox = null;
			ImgProgressBg = null;
			ImgProgress = null;
			TxtImgprogress = null;
			TxtComingSoon = null;
			ImgReward = null;
			
			mData = null;
		}
		
		public UIUnlockSceneData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIUnlockSceneData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIUnlockSceneData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
