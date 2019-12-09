using System;
using UnityEngine;

namespace IceFoxStudio
{
    public class PopupBase : MonoBehaviour
    {
        public bool enablePopupWhenStart;

        protected IDisposable disposable;

        public TweeningPopup tweeningPopup;

        protected virtual void Awake()
        {
            tweeningPopup = GetComponent<TweeningPopup>();
        }

        protected virtual void Start()
        {
            gameObject.SetActive(enablePopupWhenStart);
        }

        protected virtual void OnDestroy()
        {
            disposable?.Dispose();
        }

        protected virtual void OnEnable()
        {
            tweeningPopup?.Open();
        }

        public virtual void ClickClose()
        {
            tweeningPopup?.Close(() => gameObject.SetActive(false));
        }
    }
}