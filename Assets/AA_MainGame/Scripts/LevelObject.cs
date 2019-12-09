using IceFoxStudio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class LevelObject : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;
    [SerializeField] private Image _imgLock;
    private int _lvl;
    private bool _isUnlock;

    public void SetValue(LevelData data)
    {
        _lvl = data.lvl;
        _isUnlock = data.isUnlock;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = data.numberStar > i ? starOn : starOff;
        }

        _imgLock.enabled = !_isUnlock;
    }

    public void ClickPlay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_level_c_" + _lvl + "_" + _isUnlock + "_chapter_" +
                                                      GameData.Singleton.CurrentChapterPlay);

        if (!_isUnlock) return;
        GameData.Singleton.CurrentLevelPlay.Value = _lvl;

        SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE);
    }
}