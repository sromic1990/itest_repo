

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AdManager
{
    public interface IAdSDK
    {
        bool IsInitialized { get; set; }
        AdsSDKs SDK { get; set; }
        List<string> GameIds { get; set; }
        String AdsIdForPlatform { get; set; }
        Platforms CurrentPlatform { get; set; }

        void Initialize(AdsSDKs sdk, List<string> gameIds, List<AdIdsPerPlatform> adIdsPerPlatform);
        void ShowAds(AdType adType, string zone = "");
        void HideAds(AdType adType);
        void BindCallback(Action OnFailed, Action OnSucceeded, Action OnSkipped);

        Action AdFailed { get; set; }
        Action AdSkipped { get; set; }
        Action AdFinished { get; set; }
    }
    
    public enum AdType
    {
        AppId,
        Banner,
        Interstitial,
        VideoAd,
        VideoRewardAd
    }

    public enum AdsSDKs
    {
        UnityAds,
        Chartboost,
        AdMob,
        AdColony
    }

    public enum Platforms
    {
        Android,
        IOS
    }

    public struct AdIdsPerPlatform
    {
        public Platforms _Platform { get; set; }
        public string _AdID { get; set; }
        public AdType _AdType { get; set; }
    }
}
