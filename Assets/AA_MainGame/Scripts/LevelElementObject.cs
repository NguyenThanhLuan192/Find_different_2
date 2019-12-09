using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Facebook.Unity;
using UnityEngine;

public class LevelElementObject : EnhancedScrollerCellView
{
    [SerializeField] private LevelObject[] _levelObjects;

    private void Awake()
    {
        _levelObjects = GetComponentsInChildren<LevelObject>();
    }

    public void SetValue(LevelData[] datas)
    {
        for (int i = 0; i < _levelObjects.Length; i++)
        {
            _levelObjects[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < datas.Length; i++)
        {
            _levelObjects[i].gameObject.SetActive(true);
            _levelObjects[i].SetValue(datas[i]);
        }
    }
}