using UnityEngine;
using System.Collections;

namespace Memoria.Managers
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public static SoundManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundManager>();
                    
                    if (_instance == null)
                    {
                        throw new UnityException("SoundManager is not found.");
                    }
                }

                return _instance;
            }
        }

        [SerializeField]
        private AudioClip[] sounds;

        [SerializeField]
        private AudioClip[] bgms;

        private AudioSource _soundAudioSource;
        private AudioSource soundAudioSource
        {
            get 
            {
                if (_soundAudioSource == null)
                {
                    _soundAudioSource = gameObject.AddComponent<AudioSource>();
                }

                return _soundAudioSource;
            }
        }
        
        private AudioSource _bgmAudioSource;
        private AudioSource bgmAudioSource
        {
            get
            {
                if (_bgmAudioSource == null)
                {
                    _bgmAudioSource = gameObject.AddComponent<AudioSource>();
                    _bgmAudioSource.loop = true;
                }

                return _bgmAudioSource;
            }
        }

        void Awake()
        {
            if (this.Equals(instance))
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void PlaySound(int index)
        {
            soundAudioSource.PlayOneShot(sounds[index]);
        }
        
        public void PlayBGM(int index)
        {
            StopBGM();

            bgmAudioSource.clip = bgms[index];
            bgmAudioSource.Play();
        }
        
        public void PauseBGM()
        {
            bgmAudioSource.Pause();
        }
        
        public void ResumeBGM()
        {
            bgmAudioSource.UnPause();
        }
        
        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }
    }
}