using UnityEngine;
using UnityEngine.UI;

namespace IceFoxStudio
{
    public class BtnSound : MonoBehaviour
    {
        public string nameSound = "button_click_sound";
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.singleton.PlaySound(nameSound);
            });
        }
    }
}