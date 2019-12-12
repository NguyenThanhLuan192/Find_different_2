using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class HandObjectUI : MonoBehaviour
    {
        public Transform iconHand;
        [SerializeField] private float duration = 0.5f;
        public bool enableStart;
        [SerializeField] private Tweener _tweener;

        private void Awake()
        {
            MessageBroker.Default.Receive<ShowHandTutorialMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                enableStart = mes.active;
                gameObject.SetActive(mes.active);
                iconHand.transform.position = mes.pos;
                iconHand.localScale = Vector3.one;
              _tweener =  iconHand.DOPunchScale(Vector3.one * 0.2f, duration, 1).SetLoops(-1);

            });
        }

        private void Start()
        {
            gameObject.SetActive(enableStart);
        }

        private void OnDisable()
        {
            _tweener?.Kill();
        }
    }
    internal class ShowHandTutorialMessage
    {
        public bool active;
        public Vector3 pos { get; set; }
    }
}
