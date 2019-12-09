using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IceFoxStudio
{
    public class DetectClickUp : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
    {
        private bool _pause;

        private void Awake()
        {
            MessageBroker.Default.Receive<PauseMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
            {
                _pause = mes.pause;
            });
        }

        private void Start()
        {
            var img = GetComponent<Image>();
            if (img != null)
                img.color = Color.clear;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
//            if (TutorialManager.singleton != null && !TutorialManager.singleton.CompleteTutorialFindPointDifferent ||
  //              _pause) return;

            Debug.Log("OnPointerClick ");

            if (ExtensionMethod.isMoveFinger) return;

            MessageBroker.Default.Publish(new SelectWrongMessage() {position = Input.mousePosition});
        }

        public void OnPointerUp(PointerEventData eventData)
        {
         //   if (TutorialManager.singleton != null &&
          //      !TutorialManager.singleton.CompleteTutorialFindPointDifferent) return;
            Debug.Log("OnPointer UP");
        }
    }

    internal class PauseMessage
    {
        public bool pause;
    }

    public class SelectWrongMessage
    {
        public Vector3 position { get; set; }
    }
}