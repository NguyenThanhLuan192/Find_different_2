using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    public void ClickSetting()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Home_Menu_c_setting");
        MessageBroker.Default.Publish(new ShowSettingPopupMessage());
    }

    public void ClickPlay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("Home_Menu_c_play");
        SceneManager.LoadScene(GameConstant.SELECT_CHAPTER_SCENE);
    }
}
