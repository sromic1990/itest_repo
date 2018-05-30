using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
//using Postie.Scripts.UIRelated;
using UnityEngine;

#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace AdManager
{
    public class AdMobAdSDK : IAdSDK
    {
        public bool IsInitialized { get; set; }

        public AdsSDKs SDK { get; set; }

        public List<string> GameIds { get; set; }

        public String AdsIdForPlatform { get; set; }

        public Platforms CurrentPlatform { get; set; }

        public Action AdFailed { get; set; }

        public Action AdSkipped { get; set; }

        public Action AdFinished { get; set; }

        BannerView bannerView;
        InterstitialAd interstitial;

        string deviceId = "";

        public static string Md5Sum(string strToEncrypt)
        {
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }

        public void Initialize(AdsSDKs sdk, List<string> gameIds, List<AdIdsPerPlatform> adIdsPerPlatform)
        {
            Debug.Log("<color=red>Admob Initialize</color>");
#if UNITY_ANDROID
            CurrentPlatform = Platforms.Android;
            Application.RequestAdvertisingIdentifierAsync(
           (string advertisingId, bool trackingEnabled, string error) => {
               deviceId = advertisingId;
                Debug.Log("advertisingId " + advertisingId + " " + trackingEnabled + " " + error); 
            }
         );
#elif UNITY_IOS
            CurrentPlatform = Platforms.IOS;
            deviceId = Md5Sum(Device.advertisingIdentifier);

            Debug.Log("Device = " + deviceId);
#endif

            SDK = sdk;
            Debug.Log("App ID = " + ReturnAdID(adIdsPerPlatform, CurrentPlatform, AdType.AppId));
            MobileAds.Initialize(ReturnAdID(adIdsPerPlatform, CurrentPlatform, AdType.AppId));

            RequestBannerAd(adIdsPerPlatform);
            RequestInterstitialAd(adIdsPerPlatform);
            RequestRewardAd(adIdsPerPlatform);

        }

        public void ShowAds(AdType adType, string zone = "")
        {
            Debug.Log("AdMobSdk::ShowAds, adType = " + adType.ToString());
            switch (adType)
            {
                case AdType.Banner:
                    bannerView.Show();
                    break;

                case AdType.Interstitial:
                    if (interstitial.IsLoaded())
                    {
                        interstitial.Show();
                    }
                    break;

                case AdType.VideoRewardAd:
                    if (RewardBasedVideoAd.Instance.IsLoaded())
                    {
                        RewardBasedVideoAd.Instance.Show();
                    }
                    break;
            }
        }

        public void HideAds(AdType adType)
        {
            //Since no other type of ads can be hidden, only banner view ads are getting hidden.
            bannerView.Hide();
        }

        public void BindCallback(Action OnFailed, Action OnSucceeded, Action OnSkipped)
        {
            AdFailed = OnFailed;
            AdFinished = OnSucceeded;
            AdSkipped = OnSkipped;
        }

        //--------------------------OTHER METHODS---------->
        void RequestBannerAd(List<AdIdsPerPlatform> adIdsPerPlatform)
        {
            Debug.Log("Request Banner Ad");
            string adUnitId = ReturnAdID(adIdsPerPlatform, CurrentPlatform, AdType.Banner);

            bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
//            AdRequest request = new AdRequest.Builder().AddTestDevice(deviceId).Build();
            AdRequest request = new AdRequest.Builder().Build();
            bannerView.OnAdLoaded += (sender, e) =>
            {
                Debug.Log("<color=blue>Bannerview loaded</color>");
                //UIManager.Instance.BannerAdLoaded();
            };
            bannerView.OnAdClosed += (sender, e) =>
            {
                Debug.Log("BannerView Closed");
            };
            bannerView.OnAdFailedToLoad += (sender, e) =>
            {
                Debug.Log("<color=red>BannerView Ad Failed to load</color>");
                Debug.Log("<color=red>" + e.Message + "</color>");
            };
            bannerView.LoadAd(request);
            //bannerView.OnAdLoaded += (sender, e) => 
            //{
            //    Debug.Log("Showing admob");
            //    //HideAds(AdType.Banner);
            //};
        }

        void RequestInterstitialAd(List<AdIdsPerPlatform> adIdsPerPlatform)
        {
            Debug.Log("Request Interstitial Ad");
            string adUnitId = ReturnAdID(adIdsPerPlatform, CurrentPlatform, AdType.Interstitial);

            interstitial = new InterstitialAd(adUnitId);
            //AdRequest request = new AdRequest.Builder().AddTestDevice(deviceId).Build();
            AdRequest request = new AdRequest.Builder().Build();
            interstitial.OnAdFailedToLoad += (sender, e) =>
            {
                Debug.Log("<color=red>Interstitial ad failed to load</color>");
                Debug.Log("<color=red>" + e.Message + "</color>");
            };
            interstitial.OnAdLoaded += (sender, e) =>
            {
                Debug.Log("<color=blue>Interstitial ad loaded</color>");
            };
            interstitial.LoadAd(request);
            interstitial.OnAdClosed += (object sender, EventArgs e) =>
            {
                Debug.Log("Interstitial closed");
                RequestInterstitialAd(adIdsPerPlatform);
            };

        }

        void RequestRewardAd(List<AdIdsPerPlatform> adIdsPerPlatform)
        {
            Debug.Log("Request Reward Ad");
            string adUnitId = ReturnAdID(adIdsPerPlatform, CurrentPlatform, AdType.VideoRewardAd);

            Debug.Log(adUnitId);

            RewardBasedVideoAd.Instance.OnAdFailedToLoad += (sender, e) =>
            {
                UIManager.Instance.ChangeButtonFor_GetMoreLives(ActivateDeactivateAction.Deactivate, UIDeactivableButton.RewardAd);
            };

            RewardBasedVideoAd.Instance.OnAdLoaded += (sender, e) =>
            {
                UIManager.Instance.ChangeButtonFor_GetMoreLives(ActivateDeactivateAction.Activate, UIDeactivableButton.RewardAd);
            };

            //RewardBasedVideoAd.Instance.LoadAd(new AdRequest.Builder().AddTestDevice(deviceId).Build(), adUnitId);
            RewardBasedVideoAd.Instance.LoadAd(new AdRequest.Builder().Build(), adUnitId);

            RewardBasedVideoAd.Instance.OnAdClosed += (object sender, EventArgs e) =>
            {
                //RewardBasedVideoAd.Instance.LoadAd(new AdRequest.Builder().AddTestDevice(deviceId).Build(), adUnitId);
                RewardBasedVideoAd.Instance.LoadAd(new AdRequest.Builder().Build(), adUnitId);
            };

            RewardBasedVideoAd.Instance.OnAdRewarded += (object sender, Reward e) =>
            {
                if (AdFinished != null)
                {
                    AdFinished.Invoke();
                }
            };

        }

        private string ReturnAdID(List<AdIdsPerPlatform> adIdsPerPlatform, Platforms platform, AdType adtype)
        {
            string id = "";

            for (int i = 0; i < adIdsPerPlatform.Count; i++)
            {
                if (adIdsPerPlatform[i]._Platform.Equals(platform))
                {
                    if (adIdsPerPlatform[i]._AdType.Equals(adtype))
                    {
                        id = adIdsPerPlatform[i]._AdID;
                    }
                }
            }

            return id;
        }
    }
    
}
