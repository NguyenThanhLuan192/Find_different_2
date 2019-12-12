using UnityEngine;

namespace IceFoxStudio
{
    public class TutorialManager : MonoBehaviour
    {
        
        private static TutorialManager _instances;

        public static TutorialManager singleton
        {
            get { return _instances; }
        }

        private void Awake()
        {
            _instances = this;
        }

        public bool CompleteTutorialFindPointDifferent
        {
            get { return PlayerPrefs.GetInt("TutorialPointDifferent", 0) == 1; }
            set { PlayerPrefs.SetInt("TutorialPointDifferent", value ? 1 : 0); }
        }
    }
}