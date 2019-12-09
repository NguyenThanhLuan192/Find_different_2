using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UniRx;
using TMPro;
using UniRx.Async;

namespace IceFoxStudio
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public class IapManager : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController; // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        public static string package1 = "find.different.1";
        public static string package2 = "find.different.2";
        public static string package3 = "find.different.3";
        public static string package4 = "find.different.4";
        public static string package5 = "find.different.5";

        public static string packageRemoveAdsUnlimitedLive = "find.different.saleoff";
        //public static string package6 = "find.different.6";



        public int countVideo = 0;

        public static IapManager singleton;

        private void Awake()
        {
            singleton = this;
        }


        void Start()
        {
            if (m_StoreController == null)
            {
                InitializePurchasing();
            }

        }

        public void InitializePurchasing()
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(package1, ProductType.Consumable);
            builder.AddProduct(package2, ProductType.Consumable);
            builder.AddProduct(package3, ProductType.Consumable);
            builder.AddProduct(package4, ProductType.Consumable);
            builder.AddProduct(package5, ProductType.Consumable);
            // builder.AddProduct(package6, ProductType.Consumable);
            builder.AddProduct(packageRemoveAdsUnlimitedLive, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }


        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        public void ClickRemoveAds()
        {
#if BUILD_TEST
            GameData.Singleton.NoAds.Value = true;
            GameData.Singleton.UnlimitedLive.Value = true;
#else
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_packRemoveAds_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(packageRemoveAdsUnlimitedLive);
#endif
        }

        public void ClickPackage1()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack1_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(package1);
        }

        public void ClickPackage2()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack2_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(package2);
        }

        public void ClickPackage3()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack3_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(package3);
        }

        public void ClickPackage4()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack4_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(package4);
        }

        public void ClickPackage5()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack5_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            BuyProductID(package5);
        }

        public void ClickPackage6()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("shop_p_c_pack6_level_" +
                                                          GameData.Singleton.CurrentLevelPlay.Value);
            //BuyProductID(package6);
        }

        public void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log(
                        "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("RestorePurchases started ...");

                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result) =>
                {
                    Debug.Log("RestorePurchases continuing: " + result +
                              ". If no further messages, no purchases available to restore.");
                });
            }
            else
            {
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: PASS");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
         /*   var data = shopdata.packageDataShops.FirstOrDefault(p =>
                String.Equals(args.purchasedProduct.definition.id, p.namePackage, StringComparison.Ordinal));

            if (data != null)
            {
                data.shopItems.ForEach(item =>
                {
                    if (item.itemType == ItemType.HINT)
                        GameData.Singleton.Hint.Value += item.number;
                    if (item.itemType == ItemType.TIME1)
                        GameData.Singleton.BoostTime1.Value += item.number;
                    if (item.itemType == ItemType.TIME3)
                        GameData.Singleton.BoostTime3.Value += item.number;
                    if (item.itemType == ItemType.HEART)
                        GameData.Singleton.Heart.Value += item.number;
                    if (item.itemType == ItemType.MONEY)
                    {
                        GameData.Singleton.Money.Value += item.number;
                        MessageBroker.Default.Publish(new EffectUIMessage());
                    }

                    if (item.itemType == ItemType.SCOPE)
                        GameData.Singleton.Scope.Value += item.number;
                });
            }
            else
            {
                if (String.Equals(args.purchasedProduct.definition.id, packageRemoveAdsUnlimitedLive,
                    StringComparison.Ordinal))
                {
                    GameData.Singleton.NoAds.Value = true;
                    GameData.Singleton.UnlimitedLive.Value = true;
                }
                else
                {
                    Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'",
                        args.purchasedProduct.definition.id));
                }
            }*/

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason));
        }
    }
}