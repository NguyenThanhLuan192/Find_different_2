using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasScaleFollowRatioScreen : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;

    public List<CanvasScalerData> canvasScalerDatas;

    private void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
    }

    private void Start()
    {
        _canvasScaler.matchWidthOrHeight = GetScaler(1/Camera.main.aspect);
    }

    private float GetScaler(float mainAspect)
    {
        foreach (var s in canvasScalerDatas)
        {
            var ratio = s.screenY / (float)s.screenX;
            if (Mathf.Abs( mainAspect- ratio)<0.01f)
            {
                return s.scaleData;
            }
        }

        return 0.5f;
    }
}

[Serializable]
public class CanvasScalerData
{
    public int screenX = 9;
    public int screenY = 16;
    public float scaleData = 0.5f;
}
