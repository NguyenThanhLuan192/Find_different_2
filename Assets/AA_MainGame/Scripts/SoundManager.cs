using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace IceFoxStudio
{
    public class SoundManager : MonoBehaviour
    {
        #region singleton

        static SoundManager _instance = null;

        public static SoundManager singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Instantiate(Resources.Load<SoundManager>("SoundManager"));
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                if (soundSources.Count <= 0)
                    soundSources = GetComponents<AudioSource>().ToList();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        private void Start()
        {
            GameData.Singleton.Sound.TakeUntilDestroy(gameObject).Subscribe(value => MuteSound(!value));
            GameData.Singleton.Music.TakeUntilDestroy(gameObject).Subscribe(value => MuteMusic(!value));
        }

        public AudioSource musicSource;
        public List<AudioSource> soundSources;

        public AudioSource soundSource
        {
            get
            {
                var source = soundSources.FirstOrDefault(s => !s.isPlaying);
                if (source == null)
                {
                    source = soundSources[0];
                }

                return source;
            }
        }

        //public List<AudioClip> musicClips;
        //public List<AudioClip> soundClips;

        AudioClip getSoundClipByName(string name)
        {
            //var clip = soundClips.FirstOrDefault(s => s.name == name);
            var clip = Resources.Load<AudioClip>("Sounds/" + name);
            if (clip == null)
                Debug.LogError("Dont Find SOUND Clip With Name = " + name);
            return clip;
        }

        AudioClip getMusicClipByName(string name)
        {
            //var clip = musicClips.FirstOrDefault(s => s.name == name);
            var clip = Resources.Load<AudioClip>("sounds/" + name);

            if (clip == null)
                Debug.LogError("Dont Find MUSIC Clip With Name = " + name);
            return clip;
        }

        void MuteMusic(bool mute)
        {
            musicSource.mute = mute;
        }

        void MuteSound(bool mute)
        {
            soundSources.ForEach(s => s.mute = mute);
        }
 public void TurnOnSound()
        {
            musicSource.volume = 0;
            soundSources.ForEach(s => s.volume = 0);
        }

        public void TurnOffSound()
        {
            musicSource.volume = 0;
            soundSources.ForEach(s => s.volume = 0);
        }
        public void PlaySound(string nameSound, bool on = true)
        {
            if (on)
            {
                if (nameSound == "home_sound")
                {
                    //var source = soundSources.FirstOrDefault(s => s.clip?.name == nameSound);
                    //if (source != null && !source.isPlaying)
                    //{
                    //    source.Play();
                    //}
                    //else
                    //{
                    //    if (source == null)
                    //    {
                    //        soundSource.clip = getSoundClipByName(nameSound);
                    //        soundSource.Play();
                    //    }
                    //}

                    if (musicSource.clip?.name == "ingame_music" && !musicSource.isPlaying)
                    {
                        soundSource.clip = getSoundClipByName(nameSound);
                        soundSource.Play();
                    }
                }
                else
                {
                    soundSource.clip = getSoundClipByName(nameSound);
                    soundSource.Play();
                }
            }
            else
            {
                var source = soundSources.FirstOrDefault(s => s.clip?.name == nameSound);
                source?.Stop();
            }
        }

        public void PlayMusic(string nameMusic, bool on = true, float volume = 1)
        {
            if (on)
            {
                musicSource.clip = getMusicClipByName(nameMusic);
                musicSource.Play();
                musicSource.volume = volume;
            }
            else
            {
                musicSource?.Stop();
            }
        }

        #region Editor Config

        [ContextMenu("Set Sound Source")]
        void SetSoundSource()
        {
            soundSources?.Clear();
            var sources = transform.GetChild(0).GetComponents<AudioSource>().ToList();
            sources.ForEach(s => soundSources?.Add(s));
        }

        #endregion
    }
}