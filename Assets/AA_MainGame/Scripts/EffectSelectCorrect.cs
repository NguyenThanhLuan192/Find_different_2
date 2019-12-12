using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    internal class ShowEffectSelectCorrectMessage
    {
        public Vector3 endPos;
        public Vector3 startPos;
        public float duration;
    }

    public class EffectSelectCorrect : MonoBehaviour
    {
        [SerializeField] private float _duration = 1;
        private TweenerCore<Vector3, Vector3, VectorOptions> _tween;
        private Vector3 _tempStart;
        private Vector3 _tempEnd;
        private IDisposable _dispose;

        private void Awake()
        {
            gameObject.SetActive(false);
            _dispose = MessageBroker.Default.Receive<ShowEffectSelectCorrectMessage>().TakeUntilDestroy(gameObject)
                .Subscribe(mes =>
                {
                    gameObject.SetActive(true);
                    if (mes.startPos != Vector3.zero)
                        _tempStart = mes.startPos;
                    if (mes.endPos != Vector3.zero)
                        _tempEnd = mes.endPos;

                    if (_tempStart != Vector3.zero && _tempEnd != Vector3.zero)
                        ShowEffect(_tempStart, _tempEnd, mes.duration);
                });
        }

        private void ShowEffect(Vector3 objStartPos, Vector3 objEndPos, float objDuration)
        {
            _tween?.Kill();
            transform.position = objStartPos;
            _tween = transform.DOMove(objEndPos, objDuration == 0 ? _duration : objDuration)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    _tempStart = Vector3.zero;
                    _tempEnd = Vector3.zero;
                });
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        private void OnDestroy()
        {
            _tween?.Kill();
            _dispose?.Dispose();
        }
    }
}