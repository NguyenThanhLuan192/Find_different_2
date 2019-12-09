using System;
using System.Collections;
using System.Collections.Generic;
using IceFoxStudio;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StartBoardObject : MonoBehaviour
{
    [SerializeField] private Image[] highLights;
    private int count = 0;

    private void Awake()
    {
        count = 0;
        for (int i = 0; i < highLights.Length; i++)
        {
            highLights[i].enabled = false;
        }

        MessageBroker.Default.Receive<ShowStarsMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
        {
            var duration = 1;
            var _tempCount = count;
            Observable.Timer(TimeSpan.FromSeconds(duration)).TakeUntilDestroy(gameObject).Subscribe(_ =>
            {
                highLights[_tempCount].enabled = true;
            });
            MessageBroker.Default.Publish(new ShowEffectSelectCorrectMessage()
                {endPos = highLights[count].transform.position, duration = duration});
            count++;
        });
    }
}