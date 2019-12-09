using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class InitGame : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private List<Func<bool>> conditionChapter;


    private void Init()
    {
        if (PlayerPrefs.GetInt("HighestLvlOfChapter" + 0, -1) == -1)
        {
            PlayerPrefs.SetInt("HighestLvlOfChapter" + 0, 0);
        }

        var chapterDatas = ChapterDataManager.singleton.chapterDatas;
        conditionChapter = new List<Func<bool>>()
        {
            () => true,
            () =>
            {
                Debug.Log(chapterDatas[0].highestLvlUnlock + " - " + (chapterDatas[0].highestLvlUnlock > 5));
                return chapterDatas[0].highestLvlUnlock > 5;
            },
            () => chapterDatas[1].highestLvlUnlock > 5,
            () => chapterDatas[2].highestLvlUnlock > 5,
            () => chapterDatas[3].highestLvlUnlock > 5,
            () => chapterDatas[4].highestLvlUnlock > 5
        };

        for (int i = 0; i < chapterDatas.Count; i++)
        {
            chapterDatas[i].condition = new Func<bool>(conditionChapter[i]);
        }
    }
}