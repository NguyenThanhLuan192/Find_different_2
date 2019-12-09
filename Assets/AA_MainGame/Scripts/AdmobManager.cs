using System;
using GoogleMobileAds.Api;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class AdmobManager : IAdsManager
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
            return bannerView != null;
        }

        public bool IsReadyIntersitial()
        {
            if (interstitial == null)
                return false;
            return interstitial.IsLoaded();
        }

        public bool IsReadyVideo()
        {
            if (rewardedAd == null)
                return false;
            return rewardedAd.IsLoaded();
        }

        public void ShowBanner()
        {
            if (conditionShowBanner.Invoke())
            {
                bannerView.Show();
            }
        }

        public void ShowIntersitial(Action cbInterstitial)
        {
            this.cbInterstitial = cbInterstitial;
#if UNITY_EDITOR
            cbInterstitial?.Invoke();
#else
            interstitial.Show();
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
            rewardedAd.Show();
#endif
        }

        public void HiderBanner()
        {
            bannerView.Hide();
        }

        #endregion

        #region banner

        private BannerView bannerView;

        private void RequestBanner()
        {
#if UNITY_ANDROID
        string adUnitId = Admob.bannerIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = Admob.bannerIdIos;
#else
            string adUnitId = "unexpected_platform";
#endif

            bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

            bannerView.OnAdLoaded += HandleOnAdLoadedBanner;
            bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoadBanner;
            bannerView.OnAdOpening += HandleOnAdOpenedBanner;
            bannerView.OnAdClosed += HandleOnAdClosedBanner;
            bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplicationBanner;
            AdRequest request = new AdRequest.Builder().Build();
            //    request.TestDevices.Add("13A0079B1F0FDD35F61E7E0929BC0859");
            bannerView.LoadAd(request);
        }

        public void HandleOnAdLoadedBanner(object sender, EventArgs args)
        {
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

            Debug.Log("HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoadBanner(object sender, AdFailedToLoadEventArgs args)
        {
            bannerView.Destroy();
            RequestBanner();
            Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
        }

        public void HandleOnAdOpenedBanner(object sender, EventArgs args)
        {
            Debug.Log("HandleAdOpened event received");
        }

        public void HandleOnAdClosedBanner(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplicationBanner(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLeavingApplication event received");
        }

        #endregion

        #region interstitial

        private InterstitialAd interstitial;

        private void RequestInterstitial()
        {
#if UNITY_ANDROID
        string adUnitId = Admob.interstitialIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = Admob.interstitialIdIos;
#else
        string adUnitId = "unexpected_platform";
#endif

            this.interstitial = new InterstitialAd(adUnitId);

            this.interstitial.OnAdLoaded += HandleOnAdLoadedInterstitial;
            this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadInterstitial;
            this.interstitial.OnAdOpening += HandleOnAdOpenedInterstitial;
            this.interstitial.OnAdClosed += HandleOnAdClosedInterstitial;
            this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplicationInterstitial;

            AdRequest request = new AdRequest.Builder().Build();
#if BUILD_TEST_ADS
            request.TestDevices.Add("13A0079B1F0FDD35F61E7E0929BC0859");
#endif            
            this.interstitial.LoadAd(request);
        }

        public void HandleOnAdLoadedInterstitial(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLoaded event received");
        }

        public void HandleOnAdFailedToLoadInterstitial(object sender, AdFailedToLoadEventArgs args)
        {
            interstitial.Destroy();
            RequestInterstitial();
            Debug.Log("HandleFailedToReceiveAd event received with message: "
                      + args.Message);
        }

        public void HandleOnAdOpenedInterstitial(object sender, EventArgs args)
        {
            handlerShowAd?.Invoke();
            MonoBehaviour.print("HandleAdOpened event received");
        }

        public void HandleOnAdClosedInterstitial(object sender, EventArgs args)
        {
            Observable.TimerFrame(2).Subscribe(_ =>
            {
                handlerCloseAd?.Invoke();
                cbInterstitial?.Invoke();
                cbInterstitial = null;
            });
#if UNITY_IPHONE
            interstitial.Destroy();
            RequestInterstitial();
#elif UNITY_ANDROID
        interstitial.Destroy();
        RequestInterstitial();
#else
            interstitial.Destroy();
            RequestInterstitial();
#endif
            Debug.Log("HandleAdClosed event received");
        }

        public void HandleOnAdLeavingApplicationInterstitial(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleAdLeavingApplication event received");
        }

        #endregion

        #region VideoReward

        private RewardedAd rewardedAd;

        //private RewardedAd rewardedAd2;


        public void RequestRewardedAd()
        {
#if UNITY_ANDROID
        string adUnitId = Admob.rewardedVideoIdAndroid;
#elif UNITY_IPHONE
            string adUnitId = Admob.rewardedVideoIdIos;
#else
          string adUnitId = "unexpected_platform";
#endif
            this.rewardedAd = CreateAndLoadRewardedAd(adUnitId);
            //this.rewardedAd2 = CreateAndLoadRewardedAd(adUnitId);
        }

        public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
        {
            RewardedAd rewardedAd = new RewardedAd(adUnitId);

            rewardedAd.OnAdLoaded += HandleRewardedAdLoadedVideo;
            rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoadVideo;
            rewardedAd.OnAdOpening += HandleRewardedAdOpeningVideo;
            rewardedAd.OnUserEarnedReward += HandleUserEarnedRewardVideo;
            rewardedAd.OnAdClosed += HandleRewardedAdClosedVideo;
            rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShowVideo;

            AdRequest request = new AdRequest.Builder().Build();
            //    request.TestDevices.Add("13A0079B1F0FDD35F61E7E0929BC0859");
            rewardedAd.LoadAd(request);
            return rewardedAd;
        }

        public void HandleRewardedAdLoadedVideo(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardedAdLoaded event received");
        }

        public void HandleRewardedAdFailedToLoadVideo(object sender, AdErrorEventArgs args)
        {
            MonoBehaviour.print(
                "HandleRewardedAdFailedToLoad event received with message: "
                + args.Message);
        }

        public void HandleRewardedAdOpeningVideo(object sender, EventArgs args)
        {
            handlerShowAd?.Invoke();
            MonoBehaviour.print("HandleRewardedAdOpening event received");
        }

        public void HandleRewardedAdFailedToShowVideo(object sender, AdErrorEventArgs args)
        {
            completeReward = false;

            Debug.Log(
                "HandleRewardedAdFailedToShow event received with message: "
                + args.Message);
        }

        bool completeReward;

        public void HandleRewardedAdClosedVideo(object sender, EventArgs args)
        {
            Observable.TimerFrame(2).Subscribe(_ =>
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent("watch_rewarded_video_complete_" + completeReward);
                cbRewardedVideo?.Invoke(completeReward);
                cbRewardedVideo = null;
                completeReward = false;
                this.RequestRewardedAd();
                handlerCloseAd?.Invoke();
            });
            Debug.Log("HandleRewardedAdClosed event received");
        }

        public void HandleUserEarnedRewardVideo(object sender, Reward args)
        {
            completeReward = true;
            /*string type = args.Type;
            double amount = args.Amount;
            MonoBehaviour.print(
                "HandleRewardedAdRewarded event received for "
                            + amount.ToString() + " " + type);       */
        }

        #endregion
    }
}