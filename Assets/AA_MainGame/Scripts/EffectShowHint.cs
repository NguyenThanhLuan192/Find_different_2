using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    internal class ShowEffectHintMessage
    {
        public Vector3 Pos { get; set; }
    }

    public class EffectShowHint : MonoBehaviour
    {
        [SerializeField] private Transform startPos;
        [SerializeField] private float _duration = 1;
        private Tweener _tween;
        private IDisposable _dispose;

        private void Awake()
        {
            Debug.Log("Effect Show Hint " + gameObject.name);
            gameObject.SetActive(false);
            _dispose = MessageBroker.Default.Receive<ShowEffectHintMessage>().TakeUntilDestroy(gameObject).Subscribe(
                mes =>
                {
                    Debug.Log("Effect Show Hint");
                    gameObject.SetActive(true);
                    HandleHint(mes.Pos);
                });
            gameObject.SetActive(false);
        }

        private void HandleHint(Vector3 objPos)
        {
            var dis = Vector3.Distance(startPos.position, objPos);
            var duration = 1 * _duration;
            transform.position = startPos.position;
            _tween?.Kill();
            _tween = transform.DOMove(objPos, duration).OnComplete(() => gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void OnDestroy()
        {
            _dispose?.Dispose();
            _tween?.Kill();
        }
    }

}