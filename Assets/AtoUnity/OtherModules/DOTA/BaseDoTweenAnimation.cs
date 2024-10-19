using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AtoGame.OtherModules.DOTA
{
    public abstract class BaseDoTweenAnimation : MonoBehaviour
    {
        [SerializeField, LabelText("Play Type"), TabGroup("Tab1", "Animation Setting")]
        private EPlayType _playType = EPlayType.ManualPlay;
        [SerializeField, HideLabel, EnumToggleButtons, TabGroup("Tab1", "Callback Setting")]
        protected ECallbackType callbackType;
        [SerializeField, LabelText("On Start"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnStart), Space]
        protected BaseDoTweenAnimation startDota;
        [SerializeField, LabelText("On Start"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnStart), Space]
        protected UnityEvent startCallback;
        [SerializeField, LabelText("On Complete"), TabGroup("Tab1", "Callback Setting"), ShowIf(nameof(callbackType), ECallbackType.OnComplete), Space]
        protected UnityEvent completeCallback;


        protected Action onCompleted;
        protected int dotaCallingCounter;
        protected bool isInitialized;

        private void Awake()
        {
            if(_playType == EPlayType.AutoPlay || _playType == EPlayType.AutoInitialize)
            {
                Initialize();
            }
        }

        private void OnEnable()
        {
            if (_playType == EPlayType.AutoPlay)
            {
                Play();
            }
        }

        public void Initialize()
        {
            if(isInitialized)
            {
                return;
            }
            isInitialized = true;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {

        }

        public virtual void Play()
        {
            Play(null);
        }

        public virtual void Play(Action onCompleted)
        {
            dotaCallingCounter = 1;
            this.onCompleted = onCompleted;
            startCallback?.Invoke();
            if (startDota != null)
            {
                dotaCallingCounter++;
                startDota.Play(() => {
                    CheckOnCompleted();
                });
            }
        }

        public void Stop()
        {
            Stop(false);
        }

        public virtual void Stop(bool complete)
        {
            if(startDota != null)
            {
                startDota.Stop(complete);
            }
        }

        protected void CheckOnCompleted()
        {
            dotaCallingCounter--;
            if (dotaCallingCounter == 0)
            {
                OnComplete();
            }
        }

        protected virtual void OnComplete()
        {
            completeCallback?.Invoke();
            onCompleted?.Invoke();
        }
    }
}
