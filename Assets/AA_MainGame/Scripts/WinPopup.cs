using System;
using IceFoxStudio;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPopup : PopupBase
{
    protected override void Awake()
    {
        base.Awake();
        MessageBroker.Default.Receive<ShowWinPopupMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
        {
            
            Debug.Log("ShowWinPopupMessage "+mes.isShowVideo);
            Debug.Log("ShowWinPopupMessage "+AdsManager.singleton.IsReadyRewardVideo());
            
            Action cb = () =>
            {
                enablePopupWhenStart = true;
                gameObject.SetActive(true);
                SoundManager.singleton.PlaySound("sfx_win");
            };

            if (mes.isShowVideo && AdsManager.singleton.IsReadyRewardVideo())
            {
                AdsManager.singleton.ShowRewardedVideo(complete => cb?.Invoke());
            }
            else
            {
                cb?.Invoke();
            }
        });
    }

    public void ClickSelectLevel()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("win_p_c_back_menu_level_" +
                                                      GameData.Singleton.CurrentLevelPlay.Value);

        if (GameData.Singleton.CurrentLevelPlay.Value >
            ChapterDataManager.singleton.TotalLvlOf(GameData.Singleton.CurrentChapterPlay)) ;
        GameData.Singleton.CurrentLevelPlay.Value += 1;

        AdsManager.singleton.ShowInterstitial(() =>
        {
            LoadingPopup.singleton.ShowLoading(null, null,
                (() => { SceneManager.LoadScene(GameConstant.SELECT_LEVEL_SCENE); }));
        });
    }

    public void ClickNext()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("win_p_c_next_" + GameData.Singleton.CurrentLevelPlay.Value);

        if (GameData.Singleton.CurrentLevelPlay.Value >
            ChapterDataManager.singleton.TotalLvlOf(GameData.Singleton.CurrentChapterPlay)) ;
        GameData.Singleton.CurrentLevelPlay.Value += 1;
        LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE); }));
    }
}