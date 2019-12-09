using EnhancedUI.EnhancedScroller;
using IceFoxStudio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterElementObject : EnhancedScrollerCellView
{
    [SerializeField] private TextMeshProUGUI _nameChapter;
    [SerializeField] private TextMeshProUGUI _conditionUnlock;
    [SerializeField] private Image _mainBg;
    [SerializeField] private Image _imgLock;

    private bool _isUnlock;
    private int _id;

    public void SetValue(ChapterData chapterData)
    {
        _id = chapterData.idChapter;
        _nameChapter.text = chapterData._nameBg;
        _mainBg.sprite = Resources.Load<Sprite>("texture/chapter/chap" + _id);
        _isUnlock = chapterData.isUnlock();
        _imgLock.gameObject.SetActive(!_isUnlock);
    }


    public void ClickChapter()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_chapter_c_" + _id + "_" + _isUnlock);
        if (!_isUnlock) return;
        GameData.Singleton.CurrentChapterPlay = _id;
        SceneManager.LoadScene(GameConstant.SELECT_LEVEL_SCENE);
    }
}