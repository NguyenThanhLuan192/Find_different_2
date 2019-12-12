using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IceFoxStudio
{
    internal class GamePlayUIManager : MonoBehaviour
    {
        [Header("UI Information")] [SerializeField]
        private TextMeshProUGUI _timeTxt;

        [SerializeField] private TextMeshProUGUI _coolTimeEff;
        private float _posEffTimeWrong;
        private Tweener _tweener1;
        private Tweener _tweener2;
        [SerializeField] private float _durationEff = 0.5f;
        [SerializeField] private Ease _easeTypeColor;
        [SerializeField] private float _localMoveY = 100;
        public Transform mask_1;
        public Transform mask_2;
        private Tweener _tweenerHint;
        private IDisposable _disposable4;
        [SerializeField] private Ease _easeType;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Transform _startPosHintMove;
        [SerializeField] private Button _hintBtn;

        private void Awake()
        {
            _posEffTimeWrong = _coolTimeEff.transform.localPosition.y;
            _coolTimeEff.gameObject.SetActive(false);

            MessageBroker.Default.Receive<SelectWrongMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                //eff 
                _coolTimeEff.gameObject.SetActive(true);
                _coolTimeEff.text = "-10s";
                var pos = _coolTimeEff.transform.localPosition;
                _coolTimeEff.transform.localPosition = new Vector3(pos.x, _posEffTimeWrong, pos.z);
                _tweener1?.Kill();
                _tweener1 = _coolTimeEff.transform.DOLocalMoveY(_posEffTimeWrong + _localMoveY, _durationEff)
                    .OnComplete(() => { _coolTimeEff.gameObject.SetActive(false); });
                _coolTimeEff.color = Color.red;
                _tweener2?.Kill();
                _tweener2 = _coolTimeEff.DOColor(Color.clear, _durationEff).SetEase(_easeTypeColor);

                SetWrongPositionEffect(mes.position);
            });

        }


        public void SetWrongPositionEffect(Vector3 objPosition)
        {
        }

        public void UpdateTime(TimeSpan time)
        {
            _timeTxt.text = time.GetTimeToMMSS();
        }


        public void UpdateMyHint(int number)
        {
            _hintBtn.interactable = number != 0;
        }

        public void UpdateOpponentHint(int number)
        {
        }

        public void ClickHint()
        {
            MessageBroker.Default.Publish(new UseHintMessage());
        }

        public void ClearLvl()
        {
            var childMask = mask_1.GetComponentsInChildren<PictureFindManager>();
            var childMask2 = mask_2.GetComponentsInChildren<PictureFindManager>();
            for (int i = 0; i < childMask.Length; i++)
            {
                DestroyImmediate(childMask[i].gameObject);
            }

            for (int i = 0; i < childMask2.Length; i++)
            {
                DestroyImmediate(childMask2[i].gameObject);
            }
        }

        public void ClickPause()
        {
            MessageBroker.Default.Publish(new ShowPausePopupMessage());
        }
    }

}