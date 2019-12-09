using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UniRx;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterDataManager", menuName = "Inventory/ChapterDataManager", order = 7)]
public class ChapterDataManager : ScriptableObject
{
    private static ChapterDataManager _manager;

    private bool isHasLoad;

    public static ChapterDataManager singleton
    {
        get
        {
            if (_manager == null)
            {
                _manager = Resources.Load<ChapterDataManager>("ChapterDataManager");
            }

            return _manager;
        }
    }


    public List<ChapterData> chapterDatas
    {
        get { return chapterDataObject._chapterDatas; }
    }

    [SerializeField] private ChapterDataObject chapterDataObject;

    public int TotalLvlOf(int idChapter)
    {
        return chapterDataObject._chapterDatas[idChapter].levelDatas.Count;
    }

    [ContextMenu("auto set id chapter and lvl")]
    void AutoSetIdChapterAndLvl()
    {
        int i = 0;
        chapterDataObject._chapterDatas.ForEach(c =>
        {
            c.idChapter = i;
            int id = 0;
            c.levelDatas.ForEach(l =>
            {
                l.lvl = id;
                l.idChapter = i;
                id++;
            });
            i++;
        });
    }
}

[Serializable]
public class ChapterDataObject
{
    public List<ChapterData> _chapterDatas;
}