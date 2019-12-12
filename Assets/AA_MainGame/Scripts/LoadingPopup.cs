using System;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IceFoxStudio
{
    public class LoadingPopup : MonoBehaviour
    {
        private static LoadingPopup _instance;

        public static LoadingPopup singleton
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        public Image bgLoading;
        public TextMeshProUGUI nameTxt;
        public TextMeshProUGUI lvlTxt;
        public GameObject scroll;
        public Transform slider;
        public Tweener tweener, tweenerShow, tweenerClose;
        public float durationSlider = 1;
        public Action cb;
        public float durationToBlack = 0.3f;
        public string name, lvl;

        void HandleSlider()
        {
            tweener?.Kill();
            slider.localPosition = new Vector3(-550, 0, 0);
            tweener = slider.DOLocalMoveX(550, durationSlider).SetLoops(-1);
        }

        public void ShowLoading(string name, string lvl, Action cb)
        {
            gameObject.SetActive(true);
            this.cb = cb;
            this.name = name;
            this.lvl = lvl;
            tweenerShow?.Kill();
            bgLoading.color = Color.clear;
            tweenerShow = bgLoading.DOColor(Color.white, durationToBlack).OnComplete(() => { CompleteShowLoading(); });
        }

        public void CompleteShowLoading()
        {
            if (!string.IsNullOrEmpty(name))
            {
                nameTxt.gameObject.SetActive(true);
                nameTxt.text = name;
            }
            else
            {
                nameTxt.gameObject.SetActive(false);
            }

            if (!string.IsNullOrEmpty(lvl))
            {
                slider.gameObject.SetActive(true);
                scroll.gameObject.SetActive(true);
                lvlTxt.gameObject.SetActive(true);
                lvlTxt.text ="Level "+ lvl;
            }
            else
            {
                HandleSlider();
                slider.gameObject.SetActive(false);
                scroll.gameObject.SetActive(false);
                lvlTxt.gameObject.SetActive(false);
            }


            cb?.Invoke();
            cb = null;
            Observable.Timer(TimeSpan.FromSeconds(1)).TakeUntilDestroy(gameObject).Subscribe(_ => { CloseLoading(); });
        }

        public void CloseLoading()
        {
            tweenerClose?.Kill();
            slider?.gameObject.SetActive(false);
            scroll?.gameObject.SetActive(false);
            lvlTxt?.gameObject.SetActive(false);
            nameTxt?.gameObject.SetActive(false);

            bgLoading.color = Color.black;
            tweenerClose = bgLoading.DOColor(Color.clear, durationToBlack)
                .OnComplete(() => { CompleteCloseLoading(); });
        }

        public void CompleteCloseLoading()
        {
            gameObject.SetActive(false);
        }
    }
}