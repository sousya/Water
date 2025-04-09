using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:13fab098-e2f7-49fa-8b10-2a216a1db89f
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
		public UnlockItemCtrl ImgUnlockItem5;
		[SerializeField]
		public UnlockItemCtrl ImgUnlockItem4;
		[SerializeField]
		public UnlockItemCtrl ImgUnlockItem3;
		[SerializeField]
		public UnlockItemCtrl ImgUnlockItem2;
		[SerializeField]
		public UnlockItemCtrl ImgUnlockItem1;
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
			ImgUnlockItem5 = null;
			ImgUnlockItem4 = null;
			ImgUnlockItem3 = null;
			ImgUnlockItem2 = null;
			ImgUnlockItem1 = null;
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
