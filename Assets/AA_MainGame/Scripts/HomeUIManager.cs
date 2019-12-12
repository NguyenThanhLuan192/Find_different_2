using System;
using System.Collections;
using System.Collections.Generic;
using IceFoxStudio;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    private void Start()
    {
        SoundManager.singleton?.PlayMusic("bg_mainmenu");
    }

    public void ClickSetting()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Home_Menu_c_setting");
        MessageBroker.Default.Publish(new ShowSettingPopupMessage());
    }

    public void ClickPlay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Home_Menu_c_play");

        if (TutorialManager.singleton.CompleteTutorialFindPointDifferent)
        {
        
            LoadingPopup.singleton.ShowLoading(null, null,
                (() => { SceneManager.LoadScene(GameConstant.SELECT_CHAPTER_SCENE); }));    
        }
        else
        {
            GameData.Singleton.CurrentLevelPlay.Value = 0;
            GameData.Singleton.CurrentChapterPlay = 0;
            
            LoadingPopup.singleton.ShowLoading(null, null,
                (() => { SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE); }));
        }
        
    }

    public void ClickNoAds()
    {
        IapManager.singleton.ClickRemoveAds();
    }
}