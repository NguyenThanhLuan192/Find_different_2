using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IceFoxStudio
{
    public class PointDifferent : MonoBehaviour
    {
        public Button btn;
        public Image touch;
        public Image circleRight;
        public bool HasPickUp { get; set; }
        public Action<string, Vector3> cb;
        public Image itemImg;
        public Sprite item1, item2;
        public bool isClue;
        private Action _cbTutorial;
        private bool _isTutorial;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Tweener _tweener;

        private void Awake()
        {
            btn = GetComponent<Button>();
            touch = GetComponent<Image>();
        }

        private void Start()
        {
            btn.enabled = true;
            HasPickUp = false;
            circleRight.gameObject.SetActive(_isTutorial);
            circleRight.enabled = _isTutorial;
            touch.enabled = true;
        }

        public void ClickPoint()
        {
               if (TutorialManager.singleton!= null && !TutorialManager.singleton.CompleteTutorialFindPointDifferent && !_isTutorial) return;
            _isTutorial = false;
            _cbTutorial?.Invoke();
            if (_cbTutorial != null) _cbTutorial = null;

            if (HasPickUp) return;

            HasPickUp = true;
            cb?.Invoke(gameObject.name, transform.position);
            
            circleRight.gameObject.SetActive(true);
            circleRight.enabled = true;
            circleRight.type = Image.Type.Filled;
            _tweener?.Kill();
            circleRight.fillAmount = 0;
            _tweener = DOTween.To(() => circleRight.fillAmount, value => circleRight.fillAmount = value, 1f, _duration)
                .SetEase(Ease.Linear)
                .OnComplete(
                    () => {  });

            btn.enabled = false;
        }

        public void ShowItem(bool on)
        {
            touch.color = new Color(1, 1, 1, on ? 0 : 0);

            itemImg.enabled = true;

            if (item1 != null && on)
            {
                itemImg.sprite = item1;
            }
            else if (item2 != null && !on)
            {
                itemImg.sprite = item2;
            }
            else
            {
                itemImg.enabled = false;
            }
        }

        public void SetDifferentPointTutorial(Action cbTutorial)
        {
            this._cbTutorial = cbTutorial;
            _isTutorial = true;
            circleRight.gameObject.SetActive(_isTutorial);
            circleRight.enabled = _isTutorial;
        }
        
        private void OnDisable()
        {
            _tweener?.Kill();
        }

        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }

    public class ShowItemCluePopupMessage
    {
        public Sprite icon;
    }
}