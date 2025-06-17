using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:ad123ed6-fd22-49f2-9471-ac13cffab9ea
	public partial class UIBegin
	{
		public const string Name = "UIBegin";
		
		[SerializeField]
		public UnityEngine.UI.ScrollRect ShopScrollView;
		[SerializeField]
		public UnityEngine.UI.Button BtnStart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStart;
		[SerializeField]
		public UnityEngine.UI.Button BtnArea;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtArea;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgressBg;
		[SerializeField]
		public UnityEngine.UI.Image ImgProgress;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtImgprogress;
		[SerializeField]
		public UnityEngine.UI.Button BtnHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtHeart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public UnityEngine.UI.Button BtnCoin;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCoin;
		[SerializeField]
		public UnityEngine.UI.Button BtnStar;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtStar;
		[SerializeField]
		public Transform RewardNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnGetReward;
		[SerializeField]
		public RectTransform Target;
		[SerializeField]
		public RectTransform Begin8;
		[SerializeField]
		public RectTransform Begin7;
		[SerializeField]
		public RectTransform Begin6;
		[SerializeField]
		public RectTransform Begin5;
		[SerializeField]
		public RectTransform Begin4;
		[SerializeField]
		public RectTransform Begin3;
		[SerializeField]
		public RectTransform Begin2;
		[SerializeField]
		public RectTransform Begin1;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem8;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem7;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem6;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem5;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem4;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem3;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem2;
		[SerializeField]
		public UnityEngine.UI.Image ImgItem1;
		[SerializeField]
		public ParticleTargetMoveCtrl RewardCoinFx;
		[SerializeField]
		public UnityEngine.Animator TxtCoinAdd;
		[SerializeField]
		public UnityEngine.UI.Button BtnHead;
		[SerializeField]
		public UnityEngine.UI.Image ImgHeadFrame;
		[SerializeField]
		public UnityEngine.UI.Button BtnSelect;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelected;
		[SerializeField]
		public RectTransform BottomMenuBtns;
		
		private UIBeginData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ShopScrollView = null;
			BtnStart = null;
			TxtStart = null;
			BtnArea = null;
			TxtArea = null;
			ImgProgressBg = null;
			ImgProgress = null;
			TxtImgprogress = null;
			BtnHeart = null;
			TxtHeart = null;
			TxtTime = null;
			BtnCoin = null;
			TxtCoin = null;
			BtnStar = null;
			TxtStar = null;
			RewardNode = null;
			BtnGetReward = null;
			Target = null;
			Begin8 = null;
			Begin7 = null;
			Begin6 = null;
			Begin5 = null;
			Begin4 = null;
			Begin3 = null;
			Begin2 = null;
			Begin1 = null;
			ImgItem8 = null;
			ImgItem7 = null;
			ImgItem6 = null;
			ImgItem5 = null;
			ImgItem4 = null;
			ImgItem3 = null;
			ImgItem2 = null;
			ImgItem1 = null;
			RewardCoinFx = null;
			TxtCoinAdd = null;
			BtnHead = null;
			ImgHeadFrame = null;
			BtnSelect = null;
			ImgSelected = null;
			BottomMenuBtns = null;
			
			mData = null;
		}
		
		public UIBeginData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIBeginData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIBeginData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
