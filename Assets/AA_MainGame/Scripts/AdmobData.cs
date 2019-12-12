using UnityEngine;

/* 
Name:  Find Difference Your Life
Package: differences.find.your.life.com
App Facebook: 531083300778753


Dùng mediation ironsource:, tích hợp thêm Facebook, admob, unity

Find The Difference Your Life 
APP KEY: aba224d5

Access Key: 29b3488d8f71 
Secret Key: da307a2f86a3ca8671fcf8eb244398cb 
Refresh Token: 365e124e417e692ccd6efaf0210a63e5
 Advertiser Password / ID 51758074 / 119503

———admob————

AppID: ca-app-pub-1667805177474426~6756690242  
Banner: ca-app-pub-1667805177474426/5443608579
Interstitial: ca-app-pub-1667805177474426/6706032432
Video: ca-app-pub-1667805177474426/4050635858


——Unity———
Game:  3385432
rewardedVideo
interstitial

——FAN———

--------IOS------------

Name:  Find Difference Your Life
Package: differences.find.your.life
App Facebook: 531083300778753


Dùng mediation ironsource:, tích hợp thêm Facebook, admob, unity

Find Difference Your Life 
APP KEY: ac07b8f5

Access Key: 29b3488d8f71
 Secret Key: da307a2f86a3ca8671fcf8eb244398cb
Refresh Token: 365e124e417e692ccd6efaf0210a63e5 
Advertiser Password:   ID 51758074 / 119503

———admob————
AppID:   ca-app-pub-1667805177474426~1906482962 
Banner:  ca-app-pub-1667805177474426/8418198226
Interstitial:  ca-app-pub-1667805177474426/3165871545
Video:  ca-app-pub-1667805177474426/8366227330


——Unity———
Game: 3385433
rewardedVideo
interstitial

——FAN———


This app is using mediation


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