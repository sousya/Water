using UnityEngine;
using QFramework;

namespace QFramework.Example
{
	public partial class SettingPanelCtrl : ViewController
	{
		void Awake()
        {
			//读取保存的设置
			//在UIBegin里要初始化保存的设置(游戏音量，音效等)	
        }

        void Start()
		{
			//音量相关设置等...
			//存储相关设置
			BtnSelect.onClick.AddListener(() =>
			{
				ImgSelected.gameObject.SetActive(!ImgSelected.gameObject.activeSelf);
            });
		}
	}
}
