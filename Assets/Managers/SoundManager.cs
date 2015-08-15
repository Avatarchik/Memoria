using UnityEngine;
using System.Collections;

namespace Memoria.Managers
{
    public class SoundManager : MonoBehaviour
    {
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
                }

                return _bgmAudioSource;
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

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