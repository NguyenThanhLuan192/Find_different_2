using System;
using System.IO;
using UniRx.Async;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IceFoxStudio
{
    public class LoadDataFromResource
    {
        #region SAVE ITEM INFO

        public static void SaveItemInfo(string itemInfo, string name)
        {
            string path = null;
#if UNITY_EDITOR
            path = "Assets/AAMainGame/Resources/" + name + ".json";
#endif
#if UNITY_STANDALONE
        // You cannot add a subfolder, at least it does not work for me
        //  path = "MyGame_Data/Resources/"+_path+".json";
#endif
            string str = itemInfo.ToString();
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(str);
                }
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        #endregion

        public const string PathAdmobData = "AdmobData";


        public static GameObject LoadLevelBy(int level)
        {
            Debug.Log("Load Level " + level);
            return Resources.Load("Lvls/Level" + level) as GameObject;
        }


        internal static AdmobData LoadAdmodData()
        {
            return Resources.Load<AdmobData>(PathAdmobData);
        }

        public static async UniTask<object> LoadDataSourceAsync(string path)
        {
            var resource = await Resources.LoadAsync(path);
            return (resource as object);
        }


        public static GameObject LoadLevelOnlineBy(int lvl)
        {
            return Resources.Load("Lvls_Online/Level" + lvl) as GameObject;
        }

        public static Sprite LoadFlag(string countryCode)
        {
            return Resources.Load<Sprite>("Images/Flags/" + countryCode);
        }

        public static Sprite LoadAvatarOpponent(int index)
        {
             return Resources.Load<Sprite>("Images/Avatars/" + index);
        }
    }
}