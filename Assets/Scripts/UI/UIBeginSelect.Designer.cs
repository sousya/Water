using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:04b02828-05f3-414f-90aa-a9d052ee3281
	public partial class UIBeginSelect
	{
		public const string Name = "UIBeginSelect";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnBox;
		[SerializeField]
		public UnityEngine.UI.Button BtnInfo;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtProgress;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem1;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem1;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect1;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem2;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem2;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect2;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem3;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem3;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect3;
		[SerializeField]
		public UnityEngine.UI.Image ImgReward;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public Transform RewardNode;
		[SerializeField]
		public UnityEngine.UI.Button CloseReward;
		[SerializeField]
		public RectTransform TargetPos;
		[SerializeField]
		public RectTransform BeginPos3;
		[SerializeField]
		public RectTransform BeginPos2;
		[SerializeField]
		public RectTransform BeginPos1;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem3;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem2;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem1;
		
		private UIBeginSelectData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnClose = null;
			BtnBox = null;
			BtnInfo = null;
			ImgProgress = null;
			TxtProgress = null;
			BtnItem1 = null;
			TxtItem1 = null;
			ImgSelect1 = null;
			BtnItem2 = null;
			TxtItem2 = null;
			ImgSelect2 = null;
			BtnItem3 = null;
			TxtItem3 = null;
			ImgSelect3 = null;
			ImgReward = null;
			BtnStart = null;
			RewardNode = null;
			CloseReward = null;
			TargetPos = null;
			BeginPos3 = null;
			BeginPos2 = null;
			BeginPos1 = null;
			ImgItem3 = null;
			ImgItem2 = null;
			ImgItem1 = null;
			
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
