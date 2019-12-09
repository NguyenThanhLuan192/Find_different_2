using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class NotifyTxtObject : MonoBehaviour
    {
        public TextMeshProUGUI txt;
        public bool enableWhenStart;
        private IDisposable _disposable;
        private void Awake()
        {
            MessageBroker.Default.Receive<ShowNotifyTxtMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                enableWhenStart = true;
                gameObject.SetActive(true);
                txt.text = mes.content;

                _disposable?.Dispose();
                _disposable =  Observable.Timer(TimeSpan.FromSeconds(2)).TakeUntilDestroy(gameObject)
                    .Subscribe(_ => { gameObject.SetActive(false); });
            });
        }

        private void Start()
        {
            gameObject.SetActive(enableWhenStart);
        }
    }

    internal class ShowNotifyTxtMessage
    {
        public string content;
    }
}