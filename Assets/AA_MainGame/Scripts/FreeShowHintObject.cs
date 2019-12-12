using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class FreeShowHintObject : MonoBehaviour
    {
        [SerializeField] private Transform _freeScopeHint;
        Tweener _tweenerFreeHint;
        [SerializeField] private float _duration = 2;
        private IDisposable _disposable;

        private void Awake()
        {
            gameObject.SetActive(false);
            _disposable?.Dispose();
        _disposable =    MessageBroker.Default.Receive<FreeHintMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                gameObject.SetActive(true);
                FreeShowHint(mes.pos);
            });
        }

        public void FreeShowHint(Vector3 pos)
        {
            _freeScopeHint.gameObject.SetActive(true);
            _tweenerFreeHint?.Kill();
            _freeScopeHint.transform.position = pos;
            _tweenerFreeHint = _freeScopeHint.transform.DoEffectPunch(() =>
            {
                _freeScopeHint.gameObject.SetActive(false);
            },_duration);
        }

        private void OnDestroy()
        {
            _tweenerFreeHint?.Kill();
            _disposable?.Dispose();
        }
    }
    
    internal class FreeHintMessage
    {
        public Vector3 pos;
    }
}