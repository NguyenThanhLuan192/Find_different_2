using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using IceFoxStudio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour, IEnhancedScrollerDelegate
{
    [SerializeField] private EnhancedScroller _scroller;
    [SerializeField] private EnhancedScrollerCellView _prefabEnhancedLevel;
    [SerializeField] private float _cellSize = 965;
    [SerializeField] private ChapterData _chapterData;
    List<LevelData[]> levelDatases;
    private void Awake()
    {
        InitLevelDatases();
        _scroller.Delegate = this;
    }

    public void ClicBack()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("menu_level_c_back");
        LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.SELECT_CHAPTER_SCENE); }));
    }


    void InitLevelDatases()
    {
        _chapterData =  ChapterDataManager.singleton.chapterDatas[GameData.Singleton.CurrentChapterPlay];
        if (levelDatases == null)
            levelDatases = new List<LevelData[]>();
        levelDatases.Clear();
        var count = _chapterData.levelDatas.Count;
        if (count > 0)
        {
            var _amount = count % 3;
            var number = count / 3;
            if (_amount > 0)
            {
                number += 1;
            }

            count = number;

            for (int i = 0; i < count; i++)
            {
                LevelData[] datas;

                if (i == count - 1)
                {
                    if (_amount == 1)
                    {
                        datas = new[] {_chapterData.levelDatas[3 * i]};
                    }
                    else if (_amount == 2)
                    {
                        datas = new[]
                        {
                            _chapterData.levelDatas[3 * i], _chapterData.levelDatas[3 * i + 1]
                        };
                    }
                    else
                    {
                        datas = new[]
                        {
                            _chapterData.levelDatas[3 * i], _chapterData.levelDatas[3 * i + 1],
                            _chapterData.levelDatas[3 * i + 2]
                        };
                    }
                }
                else
                {
                    datas = new[]
                    {
                        _chapterData.levelDatas[3 * i], _chapterData.levelDatas[3 * i + 1],
                        _chapterData.levelDatas[3 * i + 2]
                    };
                }

                levelDatases.Add(datas);
            }
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return levelDatases.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return _cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var lvl = Instantiate(_prefabEnhancedLevel) as LevelElementObject;
        lvl.SetValue(levelDatases[dataIndex]);
        return lvl;
    }
}