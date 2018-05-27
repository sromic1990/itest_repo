using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdManager
{
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class AdManagerMain : MonoBehaviour
    {
        public static AdManagerMain Instance;

        public List<IAdSDK> adSDKs;
        public List<AdsSDKs> AdSdksToInitialize;

        void Awake()
        {
            Instance = this;
            adSDKs = new List<IAdSDK>();
        }

        private void Start()
        {
            InitializeAdSDKs();
        }

        private void InitializeAdSDKs()
        {
            Debug.Log("<color=red>Initialize SDK</color>");

            for (int i = 0; i < AdSdksToInitialize.Count; i++)
            {
                switch (AdSdksToInitialize[i])
                {
                    case AdsSDKs.AdMob:
#if UNITY_ANDROID
                        IAdSDK iad2 = new AdMobAdSDK();

                        List<AdIdsPerPlatform> adIdsPerPlatform = new List<AdIdsPerPlatform>();

                        AdIdsPerPlatform a0 = new AdIdsPerPlatform();
                        a0._Platform = Platforms.Android;
                        a0._AdType = AdType.AppId;
                        a0._AdID = "ca-app-pub-2494437099776771~7408497560";
                        adIdsPerPlatform.Add(a0);

                        //AdIdsPerPlatform a1 = new AdIdsPerPlatform();
                        //a1._Platform = Platforms.Android;
                        //a1._AdType = AdType.Banner;
                        //a1._AdID = "ca-app-pub-2494437099776771/9041568461";
                        ////a1._AdID = "ca-app-pub-3940256099942544/6300978111";
                        //adIdsPerPlatform.Add(a1);

                        AdIdsPerPlatform a2 = new AdIdsPerPlatform();
                        a2._Platform = Platforms.Android;
                        a2._AdType = AdType.VideoRewardAd;
                        a2._AdID = "ca-app-pub-2494437099776771/5210134662";
                        //a2._AdID = "ca-app-pub-3940256099942544/5224354917";
                        adIdsPerPlatform.Add(a2);

                        AdIdsPerPlatform a3 = new AdIdsPerPlatform();
                        a3._Platform = Platforms.Android;
                        a3._AdType = AdType.Interstitial;
                        a3._AdID = "ca-app-pub-2494437099776771/5628512038";
                        //a3._AdID = "ca-app-pub-3940256099942544/1033173712";
                        adIdsPerPlatform.Add(a3);

                        //TODO Uncomment
                        iad2.Initialize(AdsSDKs.AdMob, new List<string>(), adIdsPerPlatform);
                        adSDKs.Add(iad2);
                        //Uncomment
#elif UNITY_IOS
                        IAdSDK iad2 = new AdMobAdSDK();

                        List<AdIdsPerPlatform> adIdsPerPlatform = new List<AdIdsPerPlatform>();

                        AdIdsPerPlatform a0 = new AdIdsPerPlatform();
                        a0._Platform = Platforms.IOS;
                        a0._AdType = AdType.AppId;
                        a0._AdID = "ca-app-pub-2494437099776771/8030923489";
                        adIdsPerPlatform.Add(a0);

                        //AdIdsPerPlatform a1 = new AdIdsPerPlatform();
                        //a1._Platform = Platforms.IOS;
                        //a1._AdType = AdType.Banner;
                        //a1._AdID = "ca-app-pub-2494437099776771/8030923489";
                        ////a1._AdID = "ca-app-pub-3940256099942544/2934735716";
                        //adIdsPerPlatform.Add(a1);

                        AdIdsPerPlatform a2 = new AdIdsPerPlatform();
                        a2._Platform = Platforms.IOS;
                        a2._AdType = AdType.VideoRewardAd;
                        a2._AdID = "ca-app-pub-2494437099776771/3084177542";
                        //a2._AdID = "ca-app-pub-3940256099942544/1712485313";
                        adIdsPerPlatform.Add(a2);

                        AdIdsPerPlatform a3 = new AdIdsPerPlatform();
                        a3._Platform = Platforms.IOS;
                        a3._AdType = AdType.Interstitial;
                        a3._AdID = "ca-app-pub-2494437099776771/8030923489";
                        //a3._AdID = "ca-app-pub-3940256099942544/4411468910";
                        adIdsPerPlatform.Add(a3);

                        //TODO Uncomment
                        iad2.Initialize(AdsSDKs.AdMob, new List<string>(), adIdsPerPlatform);
                        adSDKs.Add(iad2);
                        //Uncomment
#endif
                        break;
                }
            }
        }

        public void ShowAds(AdsSDKs sdk, AdType type, Action OnFailed, Action OnSucceeded, Action OnSkipped)
        {
            //TODO remove
//            return;
            //remove
            Debug.Log("Show Ads");
            Debug.Log(sdk);
            Debug.Log(type);
            for (int i = 0; i < adSDKs.Count; i++)
            {
                if (adSDKs[i].SDK == sdk)
                {
                    adSDKs[i].ShowAds(type);
                    adSDKs[i].BindCallback(OnFailed, OnSucceeded, OnSkipped);
                    break;
                }
            }
        }

        public void HideAdMobBannerAd()
        {
            for (int i = 0; i < adSDKs.Count; i++)
            {
                if (adSDKs[i].SDK.Equals(AdsSDKs.AdMob))
                {
                    adSDKs[i].HideAds(AdType.Banner);
                    break;
                }
            }
        }

    }
    
}
