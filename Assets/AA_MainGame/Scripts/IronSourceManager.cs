using System;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class IronSourceManager : IAdsManager
    {
        AdmobData Admob;
        public Action<bool> cbRewardedVideo { get; set; }
        public Action cbInterstitial { get; set; }

        #region implement IAdsManager

        public Func<bool> conditionShowBanner { get; set; }
        public Action handlerShowAd { get; set; }
        public Action handlerCloseAd { get; set; }

        #if UNITY_ANDROID
                public const string YOUR_APP_KEY = "aba224d5";

#elif UNITY_IOS
        public const string YOUR_APP_KEY = "ac07b8f5";
      
#else
        public const string YOUR_APP_KEY = "example";
#endif
        
        public void Init()
        {
            Debug.Log("Init ADs " +YOUR_APP_KEY);

            IronSource.Agent.init (YOUR_APP_KEY, IronSourceAdUnits.REWARDED_VIDEO);
            IronSource.Agent.init (YOUR_APP_KEY, IronSourceAdUnits.INTERSTITIAL);
            IronSource.Agent.init (YOUR_APP_KEY, IronSourceAdUnits.OFFERWALL);
            IronSource.Agent.init (YOUR_APP_KEY, IronSourceAdUnits.BANNER);

            IronSource.Agent.validateIntegration();
            
            RequestBanner();
            RequestInterstitial();
            RequestRewardedAd();
        }

        public bool IsReadyBanner()
        {
            Debug.Log("IsReadyIntersitial " +isloadBanner);

            return  isloadBanner;
        }

        public bool IsReadyIntersitial()
        {
            Debug.Log("IsReadyIntersitial " + IronSource.Agent.isInterstitialReady());

            return IronSource.Agent.isInterstitialReady();
        }

        public bool IsReadyVideo()
        {
            Debug.Log("IsReadyVideo " + IronSource.Agent.isRewardedVideoAvailable());

            return IronSource.Agent.isRewardedVideoAvailable();
        }

        public void ShowBanner()
        {
            Debug.Log("ShowBanner ");

            if (conditionShowBanner.Invoke())
            {
                IronSource.Agent.displayBanner();
            }
        }

        public void ShowIntersitial(Action cbInterstitial)
        {
            this.cbInterstitial = cbInterstitial;
#if UNITY_EDITOR
            cbInterstitial?.Invoke();
#else
         IronSource.Agent.showInterstitial();
#endif
        }

        public void ShowVideoReward(System.Action<bool> cbRewardedVideo)
        {
            completeReward = false;
            this.cbRewardedVideo = cbRewardedVideo;
#if UNITY_EDITOR
            completeReward = true;
            cbRewardedVideo?.Invoke(completeReward);
#else
          IronSource.Agent.showRewardedVideo();
#endif
        }

        public void HiderBanner()
        {
            Debug.Log("HiderBanner ");
            IronSource.Agent.hideBanner();
        }

        #endregion

        #region banner

        private bool isloadBanner;

        private void RequestBanner()
        {
            Debug.Log("RequestBanner ");

            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;        
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        }

        private void BannerAdLoadedEvent()
        {
            Debug.Log("BannerAdLoadedEvent ");
            isloadBanner = true;
            if (conditionShowBanner.Invoke())
            {
                Debug.Log("SHOW BANNER");
                ShowBanner();
            }
            else
            {
                Debug.Log("HIDE BANNER");
                HiderBanner();
            }
        }

        private void BannerAdLoadFailedEvent(IronSourceError obj)
        {
            isloadBanner = false;
            Debug.Log("BannerAdLoadFailedEvent " + obj.getDescription());
        }

        private void BannerAdClickedEvent()
        {
            Debug.Log("BannerAdClickedEvent");
        }

        private void BannerAdScreenPresentedEvent()
        {
            Debug.Log("BannerAdScreenPresentedEvent");
        }

        private void BannerAdScreenDismissedEvent()
        {
            Debug.Log("BannerAdScreenDismissedEvent");

        }

        private void BannerAdLeftApplicationEvent()
        {
            Debug.Log("BannerAdLeftApplicationEvent");

        }

        #endregion

        #region interstitial


        private void RequestInterstitial()
        {
            Debug.Log("RequestInterstitial");

            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;        
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent; 
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent; 
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
            IronSource.Agent.loadInterstitial();
        }

        private void InterstitialAdClosedEvent()
        {
            Debug.Log("InterstitialAdClosedEvent");

             Observable.Timer(TimeSpan.FromSeconds(0.1f),Scheduler.MainThreadIgnoreTimeScale).Subscribe(_ =>
            {
                handlerCloseAd?.Invoke();
                cbInterstitial?.Invoke();
                cbInterstitial = null;
                IronSource.Agent.loadInterstitial();    
            });
        }

        private void InterstitialAdOpenedEvent()
        {
            Debug.Log("InterstitialAdOpenedEvent");
            handlerShowAd?.Invoke();
        }

        private void InterstitialAdClickedEvent()
        {
            Debug.Log("InterstitialAdClickedEvent");
        }

        private void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            Debug.Log("InterstitialAdShowFailedEvent " + error.getDescription());
        }

        private void InterstitialAdShowSucceededEvent()
        {         
            Debug.Log("InterstitialAdShowSucceededEvent");
        }

        private void InterstitialAdLoadFailedEvent(IronSourceError fail)
        {
            Debug.Log("InterstitialAdLoadFailedEvent " + fail.getDescription());
        }

        private void InterstitialAdReadyEvent()
        {
            Debug.Log("InterstitialAdReadyEvent");
        }

        #endregion

        #region VideoReward

        private bool completeReward;

        public void RequestRewardedAd()
        {
            Debug.Log("RequestRewardedAd");
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent; 
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent; 
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
        }

        private void RewardedVideoAdOpenedEvent()
        {
            Debug.Log("RewardedVideoAdOpenedEvent");
                        handlerShowAd?.Invoke();
        }

        private void RewardedVideoAdClosedEvent()
        {
            Debug.Log("RewardedVideoAdClosedEvent");
            Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_rewarded_video_complete_" + completeReward);
            Observable.Timer(TimeSpan.FromSeconds(0.1f),Scheduler.MainThreadIgnoreTimeScale).Subscribe(_ =>
            {
                cbRewardedVideo?.Invoke(completeReward);
                cbRewardedVideo = null;
                completeReward = false;
                this.RequestRewardedAd();
                handlerCloseAd?.Invoke();
            });
        }

        private void RewardedVideoAdEndedEvent()
        {
            Debug.Log("RewardedVideoAdEndedEvent");
            completeReward = true;
        }

        private void RewardedVideoAdStartedEvent()
        {
            Debug.Log("RewardedVideoAdStartedEvent");
        }

        private void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            Debug.Log("RewardedVideoAvailabilityChangedEvent " + available);
        }

        private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            Debug.Log("RewardedVideoAdRewardedEvent " + placement.getPlacementName());

        }

        private void RewardedVideoAdShowFailedEvent(IronSourceError err)
        {
            Debug.Log("RewardedVideoAdShowFailedEvent " + err.getDescription());
        }

        #endregion
    }
}