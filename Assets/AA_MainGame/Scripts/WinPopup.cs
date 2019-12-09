using IceFoxStudio;
using UniRx;
using UnityEngine.SceneManagement;

public class WinPopup : PopupBase
{
    protected override void Awake()
    {
        base.Awake();
        MessageBroker.Default.Receive<ShowWinPopupMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
        {
            enablePopupWhenStart = true;
            gameObject.SetActive(true);
        });
    }

    public void ClickSelectLevel()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("win_p_c_back_menu_level_"+GameData.Singleton.CurrentLevelPlay.Value);

        if (GameData.Singleton.CurrentLevelPlay.Value >
            ChapterDataManager.singleton.TotalLvlOf(GameData.Singleton.CurrentChapterPlay)) ;
        GameData.Singleton.CurrentLevelPlay.Value += 1;
        SceneManager.LoadScene(GameConstant.SELECT_LEVEL_SCENE);
    }

    public void ClickNext()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("win_p_c_next_"+GameData.Singleton.CurrentLevelPlay.Value);

        if (GameData.Singleton.CurrentLevelPlay.Value >
            ChapterDataManager.singleton.TotalLvlOf(GameData.Singleton.CurrentChapterPlay)) ;
        GameData.Singleton.CurrentLevelPlay.Value += 1;
        SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE);
    }
}