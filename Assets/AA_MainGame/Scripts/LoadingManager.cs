using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IceFoxStudio
{
    public class LoadingManager : MonoBehaviour
    {
        void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
           // LoadingPopup.singleton.ShowLoading(null, null, () => { SceneManager.LoadScene("Home"); });
        }
    }
}