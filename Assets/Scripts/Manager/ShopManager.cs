using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class ShopManager : MonoBehaviour, IDetailedStoreListener, ICanGetUtility
{
    IStoreController m_StoreController; // The Unity Purchasing system.
    static public ShopManager Instance;
    public IArchitecture GetArchitecture()
    {
        return GameMainArc.Interface;
    }
    async void Start()
    {
        try
        {
            var options = new InitializationOptions()
                .SetEnvironmentName("cake");

            await UnityServices.InitializeAsync(options);
        }
        catch (Exception exception)
        {
            // An error occurred during initialization.
        }
        Instance = this;
        InitializePurchasing();
    }


    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        
        //Add products that will be purchasable and indicate its type.
        //初始化产品列表，这里要跟IOS和Google后台的产品列表一致
        builder.AddProduct("noads", ProductType.NonConsumable);
        Debug.Log("BeginInitialized");

        UnityPurchasing.Initialize(this, builder);
    }
    //测试用的，正式代码可删除
    public void BuyNoAD()
    {
        BuyProduct("noads");
    }

    //购买时调用的接口，外部只需调用这一个接口即可
    public void BuyProduct(string pruductid)
    {
        //开始购买
        m_StoreController.InitiatePurchase(m_StoreController.products.WithID(pruductid));

        
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //初始化成功
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
        //StartCoroutine(CheckReceipt());
    }

    IEnumerator CheckReceipt()
    {
        yield return new WaitForEndOfFrame();
        if (m_StoreController != null)
        {
            CheckSubscribeReceiptAndorid();
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        //初始化失败
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        //初始化失败
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //LevelManager.Instance.NoAD = true;
        //this.GetUtility<SaveDataUtility>().SaveNoAD(true);

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //付款失败
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        //付款失败
        Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}");
    }
    //检查谷歌订阅状态的方法，该方法需要同时导入另外一个脚本GooglePurchaseData 解析谷歌支付的receipt
    public void CheckSubscribeReceiptAndorid()
    {
        if(m_StoreController.products != null)
        {
            Product p = m_StoreController.products.WithID("noads");
            if (p != null && p.hasReceipt)
            {
                // Debug.Log("recepit all:" + p.receipt);
                GooglePurchaseData data = new GooglePurchaseData(p.receipt);
                if (data.json.purchaseState == "1") //0=购买成功, 1=已取消, 2=待处理
                {
                    //LevelManager.Instance.NoCostVitality = true;
                    //this.GetUtility<SaveDataUtility>().SaveUnlock(1);
                }
                // Debug.Log("recepit autoRenewing:" + data.json.autoRenewing);
                // /*
                // Debug.Log("recepit orderId:" + data.json.orderId);
                // Debug.Log("recepit packageName:" + data.json.packageName);
                // Debug.Log("recepit productId:" + data.json.productId);
                // Debug.Log("recepit purchaseTime:" + data.json.purchaseTime);
                // Debug.Log("recepit purchaseState:" + data.json.purchaseState);
                // Debug.Log("recepit purchaseToken:" + data.json.purchaseToken);
                //*/
                // if (bool.Parse(data.json.autoRenewing))
                // {
                //     LevelManager.Instance.NoCostVitality = true;
                //     Debug.Log("sub is active");
                // }
                // else
                // {
                //     //CheckWrong();
                // }

            }
        }
       
    }
}

