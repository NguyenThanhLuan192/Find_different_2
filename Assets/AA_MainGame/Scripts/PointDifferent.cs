using System;
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
        public Action<string,Vector3> cb;
        public Image itemImg;
        public Sprite item1, item2;
        public bool isClue;
        private Action _cbTutorial;
        private bool _isTutorial;

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
//            Firebase.Analytics.FirebaseAnalytics.LogEvent("game_play_select_correct_lvl_"+GameData.Singleton.CurrentLevelPlay.Value);
         //   if (TutorialManager.singleton!= null && !TutorialManager.singleton.CompleteTutorialFindPointDifferent && !_isTutorial) return;
            _isTutorial = false;
            _cbTutorial?.Invoke();    
            if (_cbTutorial != null) _cbTutorial = null;
            
            if(HasPickUp) return;
            
            HasPickUp = true;
            cb?.Invoke(gameObject.name,transform.position);

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
    }

    public class ShowItemCluePopupMessage
    {
        public Sprite icon;
    }
}