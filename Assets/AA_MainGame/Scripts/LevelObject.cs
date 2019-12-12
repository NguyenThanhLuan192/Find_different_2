using I2.Loc;
using IceFoxStudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class LevelObject : MonoBehaviour
{
    [SerializeField] private Image[] stars;
    [SerializeField] private Sprite starOn;
    [SerializeField] private Sprite starOff;
    [SerializeField] private Image _imgLock;
    [SerializeField] private TextMeshProUGUI _lvlTxt;
    [SerializeField] private Image _mainBg;
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

        if (_isUnlock)
        {
            _mainBg.enabled = true;
            _mainBg.sprite =
                Resources.Load<Sprite>("texture/level/" + GameData.Singleton.CurrentChapterPlay + "/" + _lvl);
        }
        else
        {
            _mainBg.enabled = false;
        }

        _imgLock.enabled = !_isUnlock;
        _lvlTxt.text = string.Format(_lvlTxt.text, _lvl + 1);
    }

    public void ClickPlay()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_level_c_" + _lvl + "_" + _isUnlock + "_chapter_" +
                                                      GameData.Singleton.CurrentChapterPlay);

        if (!_isUnlock) return;
        GameData.Singleton.CurrentLevelPlay.Value = _lvl;
        LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.GAME_PLAY_SCENE); }));
    }
}