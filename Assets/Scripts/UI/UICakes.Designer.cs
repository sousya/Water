using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:49a07381-51cf-4e01-b17b-c6a437f2ac15
	public partial class UICakes
	{
		public const string Name = "UICakes";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnLeft;
		[SerializeField]
		public UnityEngine.UI.Button BtnRight;
		[SerializeField]
		public UnityEngine.UI.Button BtnReturn;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCake;
		[SerializeField]
		public UnityEngine.UI.Image ImgTeach;
		[SerializeField]
		public Animator CakeFlyNode;
		
		private UICakesData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnLeft = null;
			BtnRight = null;
			BtnReturn = null;
			TxtCake = null;
			ImgTeach = null;
			CakeFlyNode = null;
			
			mData = null;
		}
		
		public UICakesData Data
		{
			get
			{
				return mData;
			}
		}
		
		UICakesData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UICakesData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
