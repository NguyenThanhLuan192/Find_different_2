using UnityEngine;

/*  AppID: ca-app-pub-1667805177474426~3791151569
   Banner: ca-app-pub-1667805177474426/6225743219
   Interstitial: ca-app-pub-1667805177474426/7610637764
   Video: ca-app-pub-1667805177474426/8851906559
*/
   
      /*
   
Name: Find The Difference \
Package: difference.story.action.icefox\
App Facebook: 1466037986906063\
\
\
AppID: ca-app-pub-1667805177474426~3482790107\
Banner:  ca-app-pub-1667805177474426/8543545097\
Interstitial: ca-app-pub-1667805177474426/5917381755\
Video: ca-app-pub-1667805177474426/5718426425\
\
}
   */

namespace IceFoxStudio
{
    [CreateAssetMenu(fileName = "AdmobData", menuName = "Inventory/AdmobData", order = 7)]
    public class AdmobData : ScriptableObject
    {
        private static AdmobData _instance;

        public static AdmobData singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<AdmobData>("ScriptableObject/AdmobData");
                }

                return _instance;
            }
        }

        public string AppAdmobId = "ca-app-pub-3940256099942544~3347511713";
        public string AppAdmobIdIos = "ca-app-pub-1667805177474426~3482790107";
        public string bannerIdAndroid = "ca-app-pub-3940256099942544/6300978111";
        public string bannerIdIos = "ca-app-pub-3940256099942544/2934735716";
        public string interstitialIdAndroid = "ca-app-pub-3940256099942544/1033173712";
        public string interstitialIdIos = "ca-app-pub-3940256099942544/4411468910";
        public string rewardedVideoIdAndroid = "ca-app-pub-3940256099942544/5224354917";
        public string rewardedVideoIdIos = "ca-app-pub-3940256099942544/1712485313";
    }
}