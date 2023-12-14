using AtoGame.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace StickerBook
{
    public class SoundManager<T> : SingletonBindAlive<T> where T : SoundManager<T>
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;


        private SoundMangerSaver saver;

        public void Setup(SoundMangerSaver saver)
        {
            this.saver = saver;
            Load();
        }

        public void Load()
        {
            if(saver.GetMasterEnable())
            {
                mixer.SetFloat("MasterVolume", SoundHelper.ConvertToDecibel(saver.GetMasterVolume()));
            }
            else
            {
                mixer.SetFloat("MasterVolume", SoundHelper.ConvertToDecibel(0));
            }

            if(saver.GetMusicEnable())
            {
                mixer.SetFloat("MusicVolume", SoundHelper.ConvertToDecibel(saver.GetMusicVolume()));
            }
            else
            {
                mixer.SetFloat("MusicVolume", SoundHelper.ConvertToDecibel(0));
            }

            if(saver.GetMusicEnable())
            {
                mixer.SetFloat("SFXVolume", SoundHelper.ConvertToDecibel(saver.GetMusicVolume()));
            }
            else
            {
                mixer.SetFloat("SFXVolume", SoundHelper.ConvertToDecibel(0));
            }
        }

        #region Master
        public bool MasterEnable
        {
            get => saver.GetMasterEnable();
            set 
            {
                saver.SetMasterEnable(value);
                if(value)
                {
                    mixer.SetFloat("MasterVolume", SoundHelper.ConvertToDecibel(saver.GetMasterVolume()));
                }
                else
                {
                    mixer.SetFloat("MasterVolume", SoundHelper.ConvertToDecibel(0));
                }
            }
        }
        public float MasterVolume
        {
            get => saver.GetMasterVolume();
            set => saver.SetMasterVolume(value);
        }
        #endregion

        #region Music
        public bool MusicEnable
        {
            get => saver.GetMusicEnable() && saver.GetMasterEnable();
            set
            {
                saver.SetMusicEnable(value);
                if(value)
                {
                    mixer.SetFloat("MusicVolume", SoundHelper.ConvertToDecibel(saver.GetMusicVolume()));
                }
                else
                {
                    mixer.SetFloat("MusicVolume", SoundHelper.ConvertToDecibel(0));
                }
            }
        }
        public float MusicVolume
        {
            get => saver.GetMusicVolume();
            set => saver.SetMusicVolume(value);
        }

        public void PlayMusic(AudioClip clip, bool fadein = false, float fadeDuration = 1f, bool loop = true, float volume = 1f)
        {
            if(musicAudioSource == null || MusicEnable == false || musicAudioSource.isPlaying)
            {
                return;
            }
            musicAudioSource.clip = clip;
            musicAudioSource.loop = loop;
            musicAudioSource.Play();
            if(fadein)
            {
                FadeIn(musicAudioSource, volume, fadeDuration);
            }
            else
            {
                musicAudioSource.volume = volume;
            }
        }

        public void StopMusic(bool fadeout = false, float fadeDuration = 1f, Action onComplete = null)
        {
            if(musicAudioSource == null || musicAudioSource.isPlaying == false)
            {
                onComplete?.Invoke();
                return;
            }

            if(fadeout)
            {
                FadeOut(musicAudioSource, fadeDuration, ()=> {
                    musicAudioSource.Stop();
                    onComplete?.Invoke();
                });
            }
            else
            {
                musicAudioSource.Stop();
                onComplete?.Invoke();
            }
        }

        public void PauseMusic(bool fadeout = false, float fadeDuration = 1f, Action onComplete = null)
        {
            if(musicAudioSource == null || musicAudioSource.isPlaying == false)
            {
                onComplete?.Invoke();
                return;
            }

            if(fadeout)
            {
                FadeOut(musicAudioSource, fadeDuration, () => {
                    musicAudioSource.Pause();
                    onComplete?.Invoke();
                });
            }
            else
            {
                musicAudioSource.Pause();
                onComplete?.Invoke();
            }
        }

        public void UnpauseMusic(bool fadein = false, float fadeDuration = 1f, float volume = 1f)
        {
            if(musicAudioSource == null || MusicEnable == false || musicAudioSource.isPlaying == false)
            {
                return;
            }

            if(fadein)
            {
                FadeIn(musicAudioSource, volume, fadeDuration);
            }
            else
            {
                musicAudioSource.volume = volume;
            }
            musicAudioSource.UnPause();
        }
        #endregion

        #region SFX
        public bool SFXEnable
        {
            get => saver.GetSFXEnable() && saver.GetMasterEnable();
            set
            {
                saver.SetSFXEnable(value);
                if(value)
                {
                    mixer.SetFloat("SFXVolume", SoundHelper.ConvertToDecibel(saver.GetSFXVolume()));
                }
                else
                {
                    mixer.SetFloat("SFXVolume", SoundHelper.ConvertToDecibel(0));
                }
            }
        }
        public float SFXVolume
        {
            get => saver.GetSFXVolume();
            set => saver.SetSFXVolume(value);
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if(sfxAudioSource == null || SFXEnable == false)
            {
                return;
            }
            sfxAudioSource.PlayOneShot(clip, volume);
        }
        #endregion


        private void FadeIn(AudioSource audio, float toVolume, float duration = 1)
        {
            StartCoroutine(IEFadeSound(audio, 0, toVolume, duration));
        }

        private void FadeOut(AudioSource audio, float duration = 1, Action onCompleted = null)
        {
            StartCoroutine(IEFadeSound(audio, audio.volume, 0, duration, onCompleted));
        }

        IEnumerator IEFadeSound(AudioSource audio, float froVolume, float toVolume, float duration = 1, Action onCompleted = null)
        {
            if(audio == null)
            {
                Debug.LogWarning(string.Format("[SoundManager] Fade Sound Fall! Null audio {0}", audio.name));
                yield break;
            }
            float t = 0;
            while(t < duration)
            {
                t += Time.deltaTime;
                audio.volume = Mathf.Lerp(froVolume, toVolume, t / duration);
                yield return null;
            }
            audio.volume = toVolume;
            onCompleted?.Invoke();
        }
    }
}
