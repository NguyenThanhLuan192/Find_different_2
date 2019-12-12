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
    [SerializeField] private TextMeshProUGUI _commingSoonTxt;
    [SerializeField] private Image _mainBg;
    [SerializeField] private Image _imgLock;

    private bool _isUnlock;
    private int _id;

    public void SetValue(ChapterData chapterData)
    {

        if (chapterData._nameBg == "coming_soon")
        {
            _isUnlock = false;
            _commingSoonTxt.gameObject.SetActive(true);
            _nameChapter.transform.parent.gameObject.SetActive(false);
            _imgLock.gameObject.SetActive(_isUnlock);
            return;
        }
        _commingSoonTxt.gameObject.SetActive(false);
        _nameChapter.transform.parent.gameObject.SetActive(true);
        _id = chapterData.idChapter;
        _nameChapter.text = chapterData._nameBg;
        _mainBg.sprite = Resources.Load<Sprite>("texture/chapter/chap" + _id);
        _isUnlock = chapterData.isUnlock();
        _imgLock.gameObject.SetActive(!_isUnlock);
        _conditionUnlock.text = string.Format(_conditionUnlock.text, 20);
    }


    public void ClickChapter()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_chapter_c_" + _id + "_" + _isUnlock);
        if (!_isUnlock) return;
        GameData.Singleton.CurrentChapterPlay = _id;
        LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.SELECT_LEVEL_SCENE); }));
    }
}