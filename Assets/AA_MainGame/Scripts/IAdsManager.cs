using System;

public interface IAdsManager
{
    Func<bool> conditionShowBanner { get; set; }

    Action handlerShowAd { get; set; }
    Action handlerCloseAd { get; set; }

    void Init();

    bool IsReadyVideo();

    bool IsReadyIntersitial();

    bool IsReadyBanner();

    void ShowBanner();

    void ShowVideoReward(System.Action<bool> cbRewardedVideo);

    void ShowIntersitial(Action cbInterstitial);

    void HiderBanner();
}