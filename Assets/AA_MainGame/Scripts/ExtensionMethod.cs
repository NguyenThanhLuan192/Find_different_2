using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class ExtensionMethod
{
    public static bool isMoveFinger;

    public static void SetRectTransformRuntime(this Transform trans, Transform parent)
    {
        trans.transform.SetParent(parent, false);
        var rectLoading = trans.transform.GetComponent<RectTransform>();
        rectLoading.localPosition = Vector3.zero;
        rectLoading.anchorMax = Vector3.one;
        rectLoading.anchorMin = Vector3.zero;
        rectLoading.sizeDelta = Vector2.zero;
    }

    public static void SetObjectInCamRuntime(this GameObject go, Transform parent)
    {
        go.transform.SetParent(parent, false);
        go.transform.localPosition = new Vector3(0, -9.7f, 11);
    }

    public static string convertToUnSign3(this string s)
    {
        Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
        if (string.IsNullOrEmpty(s))
        {
            Debug.Log("String Is Null Or Empty convertToUnSign3");
            return s;
        } 
        string temp = s.Normalize(NormalizationForm.FormD);
        return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }

    public static string ConvertStringTwelveLetter(this string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("String Is Null Or Empty ConvertStringTwelveLetter");
        }
        
        if (name != null && !string.IsNullOrEmpty(name))
        {
            var nameTxt = name;
            if (nameTxt.Length > 12)
            {
                return nameTxt = nameTxt.Remove(12);
            }
        }

        return name;
    }

    public static string GetTimeToMMSS(this TimeSpan time)
    {
        var m = time.Minutes.ToString();
        m = m.Length < 2 ? "0" + m : m;
        var s = time.Seconds.ToString();
        s = s.Length < 2 ? "0" + s : s;
        return String.Format("{0}:{1}", m, s);
    }

    public static Tweener DoEffectPunch(this Transform trans, TweenCallback action, float duration = 0.5f)
    {
        trans.localScale = Vector3.one;
        return trans.DOPunchScale(Vector3.one * 0.2f, duration, 1).OnComplete(action);
    }

    public static IEnumerator loadLeaderboardTimeUpAndHandleAvatar(string url, Action<byte[]> saveImage,
        Action loadImage)
    {
        //ImageUrl = "https://graph.facebook.com/" + userId + "/picture?width=500&height=500"
        UnityWebRequest web = new UnityWebRequest(url.Replace("width=500&height=500", "width=150&height=150"));
        web.downloadHandler = new DownloadHandlerBuffer();
        yield return web.SendWebRequest();
        Texture2D tex = new Texture2D(0, 0);
        if (web.isNetworkError || web.isHttpError)
        {
            Debug.Log(web.error);
        }
        else
        {
            saveImage?.Invoke(web.downloadHandler.data);
            if (tex.LoadImage(web.downloadHandler.data))
            {
                //var sprite = Sprite.Create(tex, new Rect(0, 0, 150, 150), new Vector2());
            }

            yield return null;
            loadImage?.Invoke();
            MessageBroker.Default.Publish(new NotifySaveImageMessage());
        }
    }

    static void SaveGoogleSheetData(string name, string contentCsv)
    {
        var subPath = "";

        if (!Directory.Exists(Application.persistentDataPath + subPath))
            Directory.CreateDirectory(Application.persistentDataPath + subPath);
        File.WriteAllText(Application.persistentDataPath + subPath + "/" + name + ".csv", contentCsv);
    }

    public static void SaveImage(string name, byte[] dataImage)
    {
        var subPath = "/Image";

        if (!Directory.Exists(Application.persistentDataPath + subPath))
            Directory.CreateDirectory(Application.persistentDataPath + subPath);
        File.WriteAllBytes(Application.persistentDataPath + subPath + "/" + name, dataImage);
        
    }

    public static byte[] loadImage(string name)
    {
        byte[] dataByte = null;
        var subPath = "/Image";
        string path = subPath;

        //Exit if Directory or File does not exist

        if (!File.Exists(Application.persistentDataPath + subPath + "/" + name))
        {
            Debug.Log("1 Loaded Data from: " + path.Replace("/", "\\"));
            if (!Directory.Exists(Application.persistentDataPath + subPath))
            {
                Debug.Log("4 Loaded Data from: " + path.Replace("/", "\\"));
                Debug.LogWarning("Directory does not exist");
                Directory.CreateDirectory(Application.persistentDataPath + subPath);
            }
            return null;
        }

        try
        {
            dataByte = File.ReadAllBytes(Application.persistentDataPath + subPath + "/" + name);
            Debug.Log("2 Loaded Data from: " + path.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.Log("Exception 3 Loaded Data from: " + path.Replace("/", "\\") + e);
            Debug.LogWarning("Failed To Load Data from: " + path.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        return dataByte;
    }

    /* ES3Spreadsheet ReadFileCsv(string name)
     {
         if (!File.Exists(Application.persistentDataPath + subPath + "/" + name + ".csv"))
         {
             Debug.LogWarning("Directory does not exist");
             if (!Directory.Exists(Application.persistentDataPath + subPath))
                 Directory.CreateDirectory(Application.persistentDataPath + subPath);
             File.WriteAllText(Application.persistentDataPath + subPath + "/" + name + ".csv",
                 Resources.Load<TextAsset>("EventInYear/" + name).ToString());
         }

         var sheet = new ES3Spreadsheet();
         sheet.Load(Application.persistentDataPath + subPath + "/" + name + ".csv");
         // Output the first row of the spreadsheet to console.
         return sheet;
     }*/
}

public class NotifySaveImageMessage
{
}