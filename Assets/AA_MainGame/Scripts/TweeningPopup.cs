using UnityEngine;
using DG.Tweening;

namespace IceFoxStudio
{
    [HideInInspector]
    public class TweeningPopup : MonoBehaviour
    {
        public float durationOpen = 0.3f;
        public float durationClose = 0.1f;
        public Vector3 punch = Vector3.one * .1f;
        public int vibrato = 1;
        public int elasticity = 1;
        public Vector3 closeValue = Vector3.zero;
        public Ease openType = Ease.InBounce;
        public Ease closeType = Ease.InSine;

        public TypeAnimTweening typeAnimTweening;
        IAnimTweening animTweening;
        public IAnimTweening AnimTweening
        {
            get
            {
                if (animTweening == null)
                {
                    if (typeAnimTweening == TypeAnimTweening.PUNCH)
                    {
                        animTweening = new PunchAnim();
                    }
                    else
                    {
                        animTweening = new ScaleAnim();
                    }
                    animTweening.Transform = transform;
                }
                return animTweening;
            }
        }

        public void Open(TweenCallback complete = null)
        {
            //transform.localPosition = Vector3.one;
            AnimTweening.Open(punch, durationOpen, vibrato, elasticity, openType, complete);
            //transform.DOPunchScale(punch, durationOpen, vibrato, elasticity).SetEase(openType).OnComplete(complete);
        }

        public void Close(TweenCallback complete = null)
        {
            AnimTweening.Close(closeValue, durationClose, closeType, complete);
            //transform.DOScale(closeValue, durationClose).SetEase(closeType).OnComplete(() => { complete?.Invoke(); transform.localScale = Vector3.one; });
        }

        #region test Tweening

        [ContextMenu("Test Open")]
        void TestOpen()
        {
            Open(null);
        }
        [ContextMenu("Test Close")]
        void TestClose()
        {
            Close(null);
        }
        #endregion
    }

    public enum TypeAnimTweening
    {
        PUNCH,
        SCALE
    }

    public interface IAnimTweening
    {
        Transform Transform { get; set; }
        void Open(Vector3 punch, float durationOpen, int vibrato, int elasticity, Ease openType, TweenCallback complete = null);
        void Close(Vector3 closeValue, float durationClose, Ease closeType, TweenCallback complete = null);
    }

    public class PunchAnim : IAnimTweening
    {
        public Transform Transform { get; set; }

        public void Close(Vector3 closeValue, float durationClose, Ease closeType, TweenCallback complete = null)
        {
            Transform.DOScale(closeValue, durationClose).SetEase(closeType).OnComplete(() => { complete?.Invoke(); Transform.localScale = Vector3.one; });
        }

        public void Open(Vector3 punch, float durationOpen, int vibrato, int elasticity, Ease openType, TweenCallback complete = null)
        {
            Transform.localScale = Vector3.one;
            Transform.DOPunchScale(punch, durationOpen, vibrato, elasticity).SetEase(openType).OnComplete(complete);
        }
    }

    public class ScaleAnim : IAnimTweening
    {
        public Transform Transform { get; set; }
        public void Close(Vector3 closeValue, float durationClose, Ease closeType, TweenCallback complete = null)
        {
            Transform.DOScale(closeValue, durationClose).SetEase(closeType).OnComplete(() => { complete?.Invoke(); Transform.localScale = Vector3.one; });
        }
        public void Open(Vector3 punch, float durationOpen, int vibrato, int elasticity, Ease openType, TweenCallback complete = null)
        {
            Transform.localScale = Vector3.one * 1f;
            Transform.DOScale(punch, durationOpen).SetEase(openType).OnComplete(complete);
        }
    }
}