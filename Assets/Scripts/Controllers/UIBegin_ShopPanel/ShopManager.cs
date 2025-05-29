using UnityEngine;
using QFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace QFramework.Example
{
	public partial class ShopManager : ViewController, ICanGetUtility, ICanSendEvent
	{
        [SerializeField] private List<Button> buyGiftPackBtns;

        private GooglePayManager googlePay;
        private Dictionary<string ,Action> giftPackBuySuccessActions;
        private SaveDataUtility saveDataUtility;

        private void Awake()
        {
            googlePay = GooglePayManager.Instance;
            saveDataUtility = this.GetUtility<SaveDataUtility>();

            // 初始化购买成功回调
            giftPackBuySuccessActions = new Dictionary<string, Action>();
            foreach (var btn in buyGiftPackBtns)
            {
                if (!btn.TryGetComponent<GiftPack>(out GiftPack _giftPack))
                    continue;
                var _packSo = _giftPack.giftPack;
                giftPackBuySuccessActions[_packSo.ID] = () => OnPaySuccess(_packSo);
            }
        }

        private void OnEnable()
        {
            ShopScrollView.verticalNormalizedPosition = 1f;

            // 注册购买成功事件
            foreach (var kvp in giftPackBuySuccessActions)
            {
                StringEventSystem.Global.Register(kvp.Key, kvp.Value).UnRegisterWhenGameObjectDestroyed(gameObject);
            }
        }

        private void OnDisable()
        {
            // 卸载购买成功事件(避免从UIKit打开商店购买导致重复发放奖励)
            foreach (var kvp in giftPackBuySuccessActions)
            {
                StringEventSystem.Global.UnRegister(kvp.Key, kvp.Value);
            }
        }

        private void Start()
		{
            //注册按钮
            foreach (var btn in buyGiftPackBtns)
            {
                if (!btn.TryGetComponent<GiftPack>(out GiftPack _giftPack))
                    continue;
                var _packSo = _giftPack.giftPack;

                btn.onClick.AddListener(() => BuyGiftPackEvent(_packSo));
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

            //金币发放
            CoinManager.Instance.AddCoin(_packSo.Coins);
            //道具发放
            foreach (var item in _packSo.ItemReward)
            {
                saveDataUtility.AddItemNum(item.ItemIndex, item.Quantity);
            }
            //无限体力发放
            HealthManager.Instance.SetUnLimitHp(_packSo.UnlimitedHp);
            //无限道具
            //...
            UIKit.ClosePanel<UIShop>();
        }

        private void PlayAnimaton()
        {
            Debug.Log("购买成功回调动画");
        }

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
    }
}
