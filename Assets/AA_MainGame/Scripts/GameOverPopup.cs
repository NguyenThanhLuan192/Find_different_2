using IceFoxStudio;
using UniRx;
using UnityEngine.SceneManagement;

public class GameOverPopup : PopupBase
{
    protected override void Awake()
    {
        base.Awake();
        MessageBroker.Default.Receive<ShowGameOverPopupMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
        {
            enablePopupWhenStart = true;
            gameObject.SetActive(true);
            SoundManager.singleton.PlaySound("sfx_lose");
        });
    }

    public void ClickRetry()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("game_over_p_c_retry_lvl_"+GameData.Singleton.CurrentLevelPlay.Value);
        LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE); }));
    }
}