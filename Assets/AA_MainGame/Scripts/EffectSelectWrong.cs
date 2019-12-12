using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IceFoxStudio
{
    public interface IHandleSelectWrong
    {
        void HandleWrong(Vector3 pos);
    }

    public class EffectSelectWrong : MonoBehaviour, IHandleSelectWrong
    {
        [SerializeField] Image _imgWrong;
        [SerializeField] private float _duration;
        private Tweener _tweener;
        private IDisposable _dispose;

        private void Awake()
        {
            gameObject.SetActive(false);
          _dispose =  MessageBroker.Default.Receive<SelectWrongMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                gameObject.SetActive(true);
                HandleWrong(mes.position);
            });
        }

        public void HandleWrong(Vector3 pos)
        {
            transform.position = pos;
            _tweener?.Kill();
            _imgWrong.fillAmount = 0;
            _tweener = DOTween.To(() => _imgWrong.fillAmount, value => _imgWrong.fillAmount = value, 1f, _duration).SetEase(Ease.Linear)
                .OnComplete(
                    () => { gameObject.SetActive(false); });
        }

        private void OnDisable()
        {
            _tweener?.Kill();
        }

        private void OnDestroy()
        {
            _tweener?.Kill();
            _dispose?.Dispose();
        }
    }


    
}