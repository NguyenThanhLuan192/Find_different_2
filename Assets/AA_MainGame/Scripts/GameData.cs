using TMPro;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IceFoxStudio
{
    public class GameData
    {
        #region singleton

        private static GameData _instance;

        public static GameData Singleton
        {
            get
            {
                if (_instance != null) return _instance;
                Debug.Log("instance create");
                _instance = new GameData();
                return _instance;
            }
        }

        private GameData()
        {
        }

        #endregion


        private BoolReactiveProperty _sound;
        public BoolReactiveProperty Sound
        {
            get
            {
                if (_sound != null) return _sound;
                _sound = new BoolReactiveProperty(PlayerPrefs.GetInt("Sound", 1) == 1);
                _sound.Subscribe(value => PlayerPrefs.SetInt("Sound", value ? 1 : 0));
                return _sound;
            }
        }

        private BoolReactiveProperty _music;
        public BoolReactiveProperty Music
        {
            get
            {
                if (_music != null) return _music;
                _music = new BoolReactiveProperty(PlayerPrefs.GetInt("Music", 1) == 1);
                _music.Subscribe(value => PlayerPrefs.SetInt("Music", value ? 1 : 0));
                return _music;
            }
        }


        private IntReactiveProperty _currentLevelPlay;
        public IntReactiveProperty CurrentLevelPlay
        {
            get
            {
                if (_currentLevelPlay != null) return _currentLevelPlay;
                _currentLevelPlay = new IntReactiveProperty(PlayerPrefs.GetInt("currentLevelPlay", 1));
                _currentLevelPlay.Subscribe(value =>
                {
                    PlayerPrefs.SetInt("currentLevelPlay", value);
                    if (PlayerPrefs.GetInt("HighestLvlOfChapter" + CurrentChapterPlay, 0) < value)
                    {
                        PlayerPrefs.SetInt("HighestLvlOfChapter" + CurrentChapterPlay,value);
                    }
                });
                return _currentLevelPlay;
            }
        }

        private BoolReactiveProperty _noAds;
        public BoolReactiveProperty NoAds
        {
            get
            {
                if (_noAds != null) return _noAds;
                _noAds = new BoolReactiveProperty(PlayerPrefs.GetInt("NoAds", 0) == 1);
                _noAds.Subscribe(value => PlayerPrefs.SetInt("NoAds", value ? 1 : 0));

                return _noAds;
            }
        }


        private StringReactiveProperty _name;

        public StringReactiveProperty Name
        {
            get
            {
                if (_name != null) return _name;

                _name = new StringReactiveProperty(PlayerPrefs.GetString("name_Fb",
                    "Player" + Random.Range(0, 100000)));
                _name.Subscribe(value => { PlayerPrefs.SetString("name_Fb", value); });
                return _name;
            }
        }

        public int CurrentChapterPlay { get; set; }

    }
}