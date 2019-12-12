using System.Collections;
using System.Collections.Generic;
using IceFoxStudio;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePopup : PopupBase
{
   [SerializeField] private Image _sound;
   [SerializeField] private Image _music;
   [SerializeField] private Color _colorOn;
   [SerializeField] private Color _colorOff;
   protected override void Awake()
   {
      base.Awake();
      MessageBroker.Default.Receive<ShowPausePopupMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
         {
            enablePopupWhenStart = true;
            gameObject.SetActive(true);
         });

      GameData.Singleton.Sound.TakeUntilDestroy(gameObject).Subscribe(value =>
         {
            _sound.color = value ? _colorOn : _colorOff;
         });
      GameData.Singleton.Music.TakeUntilDestroy(gameObject).Subscribe(value =>
      {
         _music.color = value ? _colorOn : _colorOff;
      });
   }

   public void ClickSound()
   {
      Firebase.Analytics.FirebaseAnalytics.LogEvent("pause_p_c_sound");

      GameData.Singleton.Sound.Value = !GameData.Singleton.Sound.Value;
   }

   public void ClickMusic()
   {
      Firebase.Analytics.FirebaseAnalytics.LogEvent("pause_p_c_music");
      GameData.Singleton.Music.Value = !GameData.Singleton.Music.Value;
   }

   public void ClickHome()
   {
      Firebase.Analytics.FirebaseAnalytics.LogEvent("pause_p_c_home");
      
      AdsManager.singleton.ShowInterstitial(() =>
      {
         LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.HOME_SCENE); }));
      });
   }

   public void ClickSelectLvl()
   {
      Firebase.Analytics.FirebaseAnalytics.LogEvent("pause_p_c_select_lvl");
      AdsManager.singleton.ShowInterstitial(() =>
      {
         LoadingPopup.singleton.ShowLoading(null, null,
            (() => { SceneManager.LoadScene(GameConstant.SELECT_LEVEL_SCENE); }));
      });
   }

   public void ClickResume()
   {
      Firebase.Analytics.FirebaseAnalytics.LogEvent("pause_p_c_resume");
      ClickClose();
   }
}

public class ShowSettingPopupMessage
{
}