using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Utilities;
using Prime31;

public class IAPMangager : Singleton<IAPMangager>
{

    public List<IAPUnit> mInAppProducts;

    private List<string> IpaSkus;


    //TODO Adding Public key to IAP
    void Awake()
    {
#if UNITY_ANDROID
        GoogleIAB.init("add_pubic_key_here");
#endif
    }


    void OnEnable()
    {
#if UNITY_ANDROID
        GoogleIABManager.billingNotSupportedEvent += GoogleIABManager_billingNotSupportedEvent;
        GoogleIABManager.billingSupportedEvent += GoogleIABManager_billingSupportedEvent;
        GoogleIABManager.consumePurchaseSucceededEvent += GoogleIABManager_consumePurchaseSucceededEvent;
        GoogleIABManager.purchaseSucceededEvent += GoogleIABManager_purchaseSucceededEvent;
        GoogleIABManager.consumePurchaseFailedEvent += GoogleIABManager_consumePurchaseFailedEvent;
        GoogleIABManager.purchaseFailedEvent += GoogleIABManager_purchaseFailedEvent;
#endif
    }

    #region IAP Functions

    public void BuyProduct(IAP iap)
    {
        IAPUnit mIap = mInAppProducts.Find(f => (f.mIap == iap));
#if UNITY_ANDROID
        GoogleIAB.purchaseProduct(mIap.IAP_Id);
#endif

    }

    private void ConsumeProduct(string ProdID)
    {

    }

    //TODO
    private void QueryInventory()
    {
#if UNITY_ANDROID
        //Add Original Skus Here
        List<string> prodIDs = new List<string>();

        for (int i = 0; i < mInAppProducts.Count; i++)
            prodIDs.Add(mInAppProducts[i].IAP_Id);

        string[] skus = prodIDs.ToArray();
        GoogleIAB.queryInventory(skus);
#endif
    }

    #endregion

    #region IAP Events

#if UNITY_ANDROID
    private void IABQueryInventorySucceeded(List<GooglePurchase> arg1, List<GoogleSkuInfo> arg2)
    {
        List<string> skus = new List<string>();

        if (arg1.Count > 1)
        {
            for (int i = 0; i < arg1.Count; i++)
            {
                string id = arg1[i].productId;
                IAPUnit iap = mInAppProducts.Find(prod => (prod.IAP_Id == id));

                if (iap.mIAPtypes == IAP_Type.Consumable)
                {
                    skus.Add(iap.IAP_Id);
                }
            }

            GoogleIAB.consumeProducts(skus.ToArray());
        }
    }
#endif

    void GoogleIABManager_purchaseFailedEvent(string arg1, int arg2)
    {
#if UNITY_ANDROID
        //Pop up showing purchase failed with message
        string msg = "Purchase fail with following Error :" + arg1;

        UIManager.Instance.ShowPopUp(msg, null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);

        Debug.LogError(msg + "\nCode :" + arg2);
#endif
    }

    void GoogleIABManager_consumePurchaseFailedEvent(string obj)
    {

    }

#if UNITY_ANDROID
    void GoogleIABManager_purchaseSucceededEvent(GooglePurchase obj)
    {
        string prodId = obj.productId;

        IAPUnit mIapProduct = mInAppProducts.Find(prod => (prod.IAP_Id == prodId));

        if (mIapProduct.isRemoveAds)
            GameManager.Instance.RemoveAds();

        UseProduct(mIapProduct);

        GameManager.Instance.AddDeductCurrency(mIapProduct.mCurrency, AddDeductAction.Add, mIapProduct.prodValue);


    }
#endif

    void UseProduct(IAPUnit mIap)
    {
#if UNITY_ANDROID
        if (mIap.mIAPtypes == IAP_Type.NonConsumable)
        {
            UIManager.Instance.ShowPopUp("Purchase Success : " + mIap.IAP_name, null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
        }
        else
        {
            UIManager.Instance.ShowPopUp("Purchase Success : " + mIap.IAP_name, null, TypeOfPopUpButtons.Ok, TypeOfPopUp.Buttoned, 0, null, null);
            StartCoroutine(OnConsume(mIap));
        }
#endif

    }

    private IEnumerator OnConsume(IAPUnit mIap)
    {
        yield return new WaitForSeconds(2.0f);
#if UNITY_ANDROID
        GoogleIAB.consumeProduct(mIap.IAP_Id);
#endif
    }

#if UNITY_ANDROID
    void GoogleIABManager_consumePurchaseSucceededEvent(GooglePurchase obj)
    {
        Debug.Log("Consume Succcess :" + obj.productId + "    " + obj.packageName);
    }

    void GoogleIABManager_billingSupportedEvent()
    {
        Debug.Log("Billing Supported :D");
        QueryInventory();
    }

    void GoogleIABManager_billingNotSupportedEvent(string obj)
    {
        Debug.Log("Billing Not Supported :(");
    }
#endif

    #endregion

    void OnDisable()
    {
        #if UNITY_ANDROID
        GoogleIABManager.billingNotSupportedEvent -= GoogleIABManager_billingNotSupportedEvent;
        GoogleIABManager.billingSupportedEvent -= GoogleIABManager_billingSupportedEvent;
        GoogleIABManager.consumePurchaseSucceededEvent -= GoogleIABManager_consumePurchaseSucceededEvent;
        GoogleIABManager.purchaseSucceededEvent -= GoogleIABManager_purchaseSucceededEvent;
        GoogleIABManager.consumePurchaseFailedEvent -= GoogleIABManager_consumePurchaseFailedEvent;
        GoogleIABManager.purchaseFailedEvent -= GoogleIABManager_purchaseFailedEvent;
        #endif
    }
}

[System.Serializable]
public class IAPUnit
{
    public string IAP_name;
    public IAP mIap;
    public string IAP_Id;
    public string displayValue;
    public IAP_Type mIAPtypes;
    public bool isRemoveAds;
    public int prodValue;
    public Currency mCurrency;
}

public enum IAP_Type
{
    Consumable,
    NonConsumable
}

public enum IAP
{
    BrainPill,
    BrainOperation,
    NewBrain,
    BananaPlate,
    BananaBasket,
    BananaTree,
    BananaFarm,
    BananaMarket
}
