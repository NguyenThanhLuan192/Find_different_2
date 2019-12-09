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

        public void Init()
        {
            Admob = AdmobData.singleton;
            RequestBanner();
            RequestInterstitial();
            RequestRewardedAd();
        }

        public bool IsReadyBanner()
        {
            return  isloadBanner;
        }

        public bool IsReadyIntersitial()
        {
            return IronSource.Agent.isInterstitialReady();
        }

        public bool IsReadyVideo()
        {
            return IronSource.Agent.isRewardedVideoAvailable();
        }

        public void ShowBanner()
        {
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
            IronSource.Agent.hideBanner();
        }

        #endregion

        #region banner

        private bool isloadBanner;

        private void RequestBanner()
        {
            
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            IronSource.Agent.loadBanner(new IronSourceBannerSize(320, 50), IronSourceBannerPosition.BOTTOM);
            
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;        
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
        }

        private void BannerAdLoadedEvent()
        {
            isloadBanner = false;
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

        }

        private void BannerAdClickedEvent()
        {
           
        }

        private void BannerAdScreenPresentedEvent()
        {
            
        }

        private void BannerAdScreenDismissedEvent()
        {
            
        }

        private void BannerAdLeftApplicationEvent()
        {
            
        }

        #endregion

        #region interstitial


        private void RequestInterstitial()
        {
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
            handlerCloseAd?.Invoke();
            cbInterstitial?.Invoke();
            cbInterstitial = null;
        }

        private void InterstitialAdOpenedEvent()
        {
            handlerShowAd?.Invoke();
        }

        private void InterstitialAdClickedEvent()
        {
        }

        private void InterstitialAdShowFailedEvent(IronSourceError error)
        {
        }

        private void InterstitialAdShowSucceededEvent()
        {
        }

        private void InterstitialAdLoadFailedEvent(IronSourceError fail)
        {
        }

        private void InterstitialAdReadyEvent()
        {
            
        }

        #endregion

        #region VideoReward

        private bool completeReward;

        public void RequestRewardedAd()
        {
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
                        handlerShowAd?.Invoke();
        }

        private void RewardedVideoAdClosedEvent()
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_rewarded_video_complete_" + completeReward);
            cbRewardedVideo?.Invoke(completeReward);
            cbRewardedVideo = null;
            completeReward = false;
            this.RequestRewardedAd();
            handlerCloseAd?.Invoke();
        }

        private void RewardedVideoAdEndedEvent()
        {
            completeReward = true;
        }

        private void RewardedVideoAdStartedEvent()
        {
        }

        private void RewardedVideoAvailabilityChangedEvent(bool available)
        {
         
        }

        private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
           
        }

        private void RewardedVideoAdShowFailedEvent(IronSourceError err)
        {
        }

        #endregion
    }
}