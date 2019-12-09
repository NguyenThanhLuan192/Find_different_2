using System;
using Facebook.Unity;
using I2.Loc;
using IceFoxStudio;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : PopupBase
{
    [SerializeField] private Image _sound;
    [SerializeField] private Image _music;
    [SerializeField] private Color _colorOn;
    [SerializeField] private Color _colorOff;

    [SerializeField] private TMP_Dropdown _dropdown;

    protected override void Awake()
    {
        base.Awake();
        MessageBroker.Default.Receive<ShowSettingPopupMessage>().TakeUntilDestroy(gameObject).Subscribe(mes =>
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

    protected override void OnEnable()
    {
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
        {
            _dropdown.captionText.text = LocalizationManager.CurrentLanguage;
        });
        base.OnEnable();
    }

    public void ClickFacebook()
    {
        FacebookManager.Singleton.Login((value) =>
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent("setting_p_c_facebook_" + FB.IsLoggedIn);

            if (FB.IsLoggedIn)
            {
                Application.OpenURL("fb://page/172170846726448");
            }
        });
        Observable.Timer(TimeSpan.FromSeconds(3)).TakeUntilDestroy(gameObject).Subscribe(_ =>
        {
            if (PlayerPrefs.GetInt(KeyPlayerPref.LogginFb, 0) == 0)
            {
            }
        });
    }

    public void ClickInstagram()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("setting_p_c_instagram");
        //       Application.OpenURL("instagram://user?username=icefox.com.vn");
        Application.OpenURL("https://www.instagram.com/icefox.com.vn");
    }

    public void ClickSound()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("setting_p_c_sound");
        GameData.Singleton.Sound.Value = !GameData.Singleton.Sound.Value;
    }

    public void ClickMusic()
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent("setting_p_c_music");
        GameData.Singleton.Music.Value = !GameData.Singleton.Music.Value;
    }

    public void ClickSelectLanguage()
    {
        var _language = _dropdown.options[_dropdown.value].text;
        if (LocalizationManager.HasLanguage(_language))
        {
            LocalizationManager.CurrentLanguage = _language;
        }

        Firebase.Analytics.FirebaseAnalytics.LogEvent("setting_p_c_language_" + LocalizationManager.CurrentLanguage);
    }
}