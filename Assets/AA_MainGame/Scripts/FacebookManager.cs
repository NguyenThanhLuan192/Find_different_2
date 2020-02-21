using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Facebook.Unity;
using UnityEngine;

namespace IceFoxStudio
{
    public class FacebookManager : MonoBehaviour
    {
        public Action cbShare;
        public Action<ILoginResult> cbLogin;
        public Action<IGraphResult> cbDisplaySuccess;

        public static FacebookManager Singleton { get; private set; }

        private void Awake()
        {
            Singleton = this;
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...\
                if (PlayerPrefs.GetInt(KeyPlayerPref.LogginFb) == 1)
                    Login(null);
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        private static void OnHideUnity(bool isGameShown)
        {
            Time.timeScale = !isGameShown ? 0 : 1;
        }

        // LOGIN
        public void Login(Action<ILoginResult> cbLoginSuccess, Action<IGraphResult> cbDisplaySuccess = null)
        {
            Debug.Log("User login fb");
            cbLogin = cbLoginSuccess;
            this.cbDisplaySuccess = cbDisplaySuccess;
            if (FB.IsLoggedIn)
            {
                cbLogin?.Invoke(resultLogin);

                ////https://www.facebook.com/groups/2394420524116108/
                Application.OpenURL("fb://page/172170846726448");
                //Application.OpenURL("fb://groups/2394420524116108");
            }
            else
            {
                var perms = new List<string>() {"public_profile,email"};
                FB.LogInWithReadPermissions(perms, AuthCallback);
            }
        }

        public string userId;
        public string ast;
        public ILoginResult resultLogin;

        private void AuthCallback(ILoginResult result)
        {
            resultLogin = result;
            cbLogin?.Invoke(resultLogin);
            if (FB.IsLoggedIn)
            {
                foreach (var key in result.ResultDictionary.Keys)
                {
                    Debug.Log("User login fb AuthCallback " + key + " : " + result.ResultDictionary[key]);
                }

                PlayerPrefs.SetInt(KeyPlayerPref.LogginFb, 1);
                // AccessToken class will have session details
                var aToken = AccessToken.CurrentAccessToken;
                // Print current access token's User ID
                userId = result.AccessToken.UserId;
                ast = result.AccessToken.TokenString;
                var ImageUrl = "https://graph.facebook.com/" + userId + "/picture?width=500&height=500";
                StartCoroutine(ExtensionMethod.loadLeaderboardTimeUpAndHandleAvatar(ImageUrl,
                    (bytes) => { ExtensionMethod.SaveImage("avatar", bytes); },
                    null));

                PlayerPrefs.SetString(KeyPlayerPref.UserFb, userId);
                PlayerPrefs.SetString("AccessToken_Fb", ast);
                FB.API("/me/picture?type=square&height=400&width=400", HttpMethod.GET, DisplayProfilePic);
                FB.API("/me?fields=name,email", HttpMethod.GET, DisplayUsername);

                // Print current access token's granted permissions
                foreach (string perm in aToken.Permissions)
                {
                    Debug.Log(perm);
                }

                Application.OpenURL("fb://page/172170846726448");
            }
            else
            {
                Debug.Log("User login cancelled");
            }
        }

        private void DisplayUsername(IGraphResult result)
        {
            if (result.Error == null)
            {
                cbDisplaySuccess?.Invoke(result);
                if (result.ResultDictionary != null)
                {
                    foreach (var key in result.ResultDictionary.Keys)
                    {
                        Debug.Log("Display User Name aaa " + key + " : " + result.ResultDictionary[key]);
                        if (key == "name")
                        {
                            var nameFb = result.ResultDictionary["name"].ToString(); //.convertToUnSign3();
                            GameData.Singleton.Name.Value = nameFb; // 
                        }
                        else if (key == "email")
                        {
                            PlayerPrefs.SetString("email_Fb", result.ResultDictionary["email"].ToString());
                        }
                    }
                }

                /*   PlayFabManager.Singleton.LinkOrLogginFB(PlayerPrefs.GetString("email_Fb", "no email"),
                       PlayerPrefs.GetString("name_Fb"), userId, ast);*/
            }
        }

        private void DisplayProfilePic(IGraphResult result)
        {
            if (result.ResultDictionary != null)
            {
                foreach (string key in result.ResultDictionary.Keys)
                {
                    Debug.Log("Display Profile Pic " + key + " : " + result.ResultDictionary[key]);
                }
            }
        }

        // SHARE

        public void Share(Action cbShareSuccess)
        {
            cbShare = cbShareSuccess;
            FB.ShareLink(
                new Uri("https://play.google.com/store/apps/details?id=troll.stickman.words.story.icefox"),
                callback: ShareCallback);
        }

        private void ShareCallback(IShareResult result)
        {
            if (result.Cancelled || !String.IsNullOrEmpty(result.Error))
            {
                Debug.Log("ShareLink Error: " + result.Error);
            }
            else if (!String.IsNullOrEmpty(result.PostId))
            {
                // Print post identifier of the shared content
                Debug.Log(result.PostId);
            }
            else
            {
                // Share succeeded without postID
                Debug.Log("ShareLink success!");
                cbShare?.Invoke();
            }
        }
    }
}