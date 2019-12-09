using System;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectChapterManager : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller _scroller;
    [SerializeField] private EnhancedScrollerCellView _prefabEnhancedChapter;
    [SerializeField] private float _cellSize = 965;
    [SerializeField] private ChapterDataManager _chapterDataManager;

    private void Start()
    {
        _scroller.Delegate = this;
    }

    public void ClicBackHome()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_chapter_c_back");
        SceneManager.LoadScene(GameConstant.HOME_SCENE);
    }
    
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _chapterDataManager.chapterDatas.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return _cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var chapter = Instantiate(_prefabEnhancedChapter) as ChapterElementObject;
        chapter.SetValue(_chapterDataManager.chapterDatas[dataIndex]);
        return chapter;
    }
}