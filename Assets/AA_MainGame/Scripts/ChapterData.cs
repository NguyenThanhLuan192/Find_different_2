using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[Serializable]
public class ChapterData
{
    public Func<bool> condition;
    public string _nameBg;
    public int idChapter;

    public bool isUnlock()
    {
        return condition.Invoke();
    }

    public List<LevelData> levelDatas;
    [HideInInspector]
    public int highestLvlUnlock => PlayerPrefs.GetInt("HighestLvlOfChapter" + idChapter,0);
}

[Serializable]
public class LevelData
{
    [HideInInspector] public int idChapter;
    public int lvl;

    public int numberStar
    {
        get { return PlayerPrefs.GetInt("StarLvl" + lvl + "OfChapter" + idChapter); }
    }

    public bool isUnlock
    {
        get { return PlayerPrefs.GetInt("HighestLvlOfChapter" + idChapter, 0) >= lvl; }
    }
}