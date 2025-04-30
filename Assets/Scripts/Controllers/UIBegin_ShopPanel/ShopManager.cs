using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace QFramework.Example
{
	public partial class ShopManager : ViewController
	{
        [SerializeField] private List<Button> buyGiftPackBtns;
	  	private void Start()
		{
            foreach (var btn in buyGiftPackBtns)
            {
                btn.onClick.AddListener(()=> BuyGiftPackEvent(btn));
            }
        }

        private void BuyGiftPackEvent(Button btn)
        {
            // 调用谷歌购买等...

            // 应该是传入回调(或者注册购买成功的事件)
            // 购买成功先播放动画，然后调用GiftPake的方法
            var pack = btn.GetComponent<GiftPackButton>().giftPack;
            Debug.Log(pack.name);
            //pack.BuyGiftPack();
        }

        private void PlayAnimaton()
        {
            Debug.Log("购买成功回调动画");
        }
    }
}
