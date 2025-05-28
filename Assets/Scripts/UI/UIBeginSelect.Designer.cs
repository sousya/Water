using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:643110df-b160-4a3b-9017-08b27f5e85b8
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
		public UnityEngine.UI.Button BtnItem1;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem1;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddItem1;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect1;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem2;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem2;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddItem2;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect2;
		[SerializeField]
		public UnityEngine.UI.Button BtnItem3;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtItem3;
		[SerializeField]
		public UnityEngine.UI.Button BtnAddItem3;
		[SerializeField]
		public UnityEngine.UI.Image ImgSelect3;
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
			BtnItem1 = null;
			TxtItem1 = null;
			BtnAddItem1 = null;
			ImgSelect1 = null;
			BtnItem2 = null;
			TxtItem2 = null;
			BtnAddItem2 = null;
			ImgSelect2 = null;
			BtnItem3 = null;
			TxtItem3 = null;
			BtnAddItem3 = null;
			ImgSelect3 = null;
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
