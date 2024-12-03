using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:f2b69d8e-98bc-4aa1-81af-33be6b1252a8
	public partial class UIClear
	{
		public const string Name = "UIClear";
		
		[SerializeField]
		public UnityEngine.UI.Image ImgStar3;
		[SerializeField]
		public Transform ThreeNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnLevel;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public Transform UnlockNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgScroll;
		[SerializeField]
		public UnityEngine.UI.Image ImgLight2;
		[SerializeField]
		public UnityEngine.UI.Image ImgCake2;
		[SerializeField]
		public UnityEngine.UI.Image ImgLight1;
		[SerializeField]
		public UnityEngine.UI.Image ImgCake1;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtUnlock;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtUnlockLevel;
		[SerializeField]
		public Transform UnlockBgNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgBgScroll;
		[SerializeField]
		public UnityEngine.UI.Image ImgBg;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtUnlockBg;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtUnlockBgLevel;
		[SerializeField]
		public UnityEngine.UI.Image ImgFlyCake;
		[SerializeField]
		public UnityEngine.UI.Image ImgFlyBg;
		[SerializeField]
		public Transform NoThreeNode;
		[SerializeField]
		public UnityEngine.UI.Image ImgStar1;
		[SerializeField]
		public UnityEngine.UI.Image ImgStar2;
		[SerializeField]
		public UnityEngine.UI.Button BtnRetry;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtRestart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtAddStep;
		[SerializeField]
		public UnityEngine.UI.Button BtnContinue;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtContinue;
		
		private UIClearData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			ImgStar3 = null;
			ThreeNode = null;
			BtnLevel = null;
			TxtLevel = null;
			UnlockNode = null;
			ImgScroll = null;
			ImgLight2 = null;
			ImgCake2 = null;
			ImgLight1 = null;
			ImgCake1 = null;
			TxtUnlock = null;
			TxtUnlockLevel = null;
			UnlockBgNode = null;
			ImgBgScroll = null;
			ImgBg = null;
			TxtUnlockBg = null;
			TxtUnlockBgLevel = null;
			ImgFlyCake = null;
			ImgFlyBg = null;
			NoThreeNode = null;
			ImgStar1 = null;
			ImgStar2 = null;
			BtnRetry = null;
			TxtRestart = null;
			TxtAddStep = null;
			BtnContinue = null;
			TxtContinue = null;
			
			mData = null;
		}
		
		public UIClearData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIClearData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIClearData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
