using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Events;
using UnityEngine.UI;

public class Purchasing : MonoBehaviour, IStoreListener
{
    [SerializeField] private UnityEvent _success;

    private static IStoreController _storeController;
    private static IExtensionProvider _storeExtensionProvider;

    private static string Gems = nameof(Gems);

    private void Start()
    {
        if (_storeController == null)
            InitializePurchasing();
    }

    public void BuyGems() => BuyProductID(Gems);

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (string.Equals(args.purchasedProduct.definition.id, Gems, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            _success?.Invoke();
        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        _storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
       
    }

    public Product GetProduct(string productId)
    {
        return _storeController.products.WithStoreSpecificID(productId);
    }

    private void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(Gems, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = _storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                _storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    private bool IsInitialized()
    {
        return _storeController != null && _storeExtensionProvider != null;
    }
}
