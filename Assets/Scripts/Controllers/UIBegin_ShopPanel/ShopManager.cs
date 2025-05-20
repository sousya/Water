using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace QFramework.Example
{
	public partial class ShopManager : ViewController
	{
        [SerializeField] private List<Button> buyGiftPackBtns;

        private GooglePayManager googlePay;

        private void Awake()
        {
            googlePay = GooglePayManager.Instance;
        }

        private void OnEnable()
        {
            ShopScrollView.verticalNormalizedPosition = 1f;
        }

        private void Start()
		{
            //注册按钮和对应礼包购买成功事件
            foreach (var btn in buyGiftPackBtns)
            {
                if (!btn.TryGetComponent<GiftPack>(out GiftPack _giftPack))
                    continue;
                var _packSo = _giftPack.giftPack;

                btn.onClick.AddListener(()=> BuyGiftPackEvent(_packSo));
                StringEventSystem.Global.Register(_packSo.ID, () => OnPaySuccess(_packSo)).UnRegisterWhenGameObjectDestroyed(gameObject);
            }
        }

        /// <summary>
        /// 购买礼包事件
        /// </summary>
        /// <param name="_packSo"></param>
        private void BuyGiftPackEvent(GiftPackSO _packSo)
        {
            //Debug.Log("礼包ID ： " + _packSo.ID);
            googlePay.BuyProduct(_packSo.ID);
        }

        /// <summary>
        /// 礼包购买成功回调
        /// </summary>
        private void OnPaySuccess(GiftPackSO _packSo)
        {
            PlayAnimaton();
            //Debug.Log($"礼包{_packSo.ID}购买成功");
            CoinManager.Instance.AddCoin(_packSo.Coins);
            //HealthManager.Instance.SetUnLimitHp(_packSo.UnlimitedHp);
            //
        }

        private void PlayAnimaton()
        {
            Debug.Log("购买成功回调动画");
        }
    }
}
