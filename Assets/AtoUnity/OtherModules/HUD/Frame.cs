#if USE_ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using AtoGame.Base.Helper;
using System;
using UnityEngine;

namespace AtoGame.OtherModules.HUD
{
    [DisallowMultipleComponent]
    public abstract class Frame : MonoBehaviour
    {
        [Header("[Events]")]
#if USE_ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onShowing;
#if USE_ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onHidding;
#if USE_ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onPausing;
#if USE_ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onResuming;

        private FrameState state = FrameState.NotInitialized;
        private HUD hud;

        private Action onCompleted;
        private bool instant;

        public HUD Hud { get => hud; private set => hud = value; }
        public bool Initialized { get => state != FrameState.NotInitialized; }

        public bool IsShowing
        {
            get => state == FrameState.Showing;
        }

        public bool IsHidding
        {
            get => state == FrameState.Hidding;
        }
        public bool IsPausing
        {
            get => state == FrameState.Pausing;
        }

        public void SetState(FrameState state)
        {
            this.state = state;
        }

        public Frame Initialize(HUD hud)
        {
            if (!Initialized)
            {
                this.Hud = hud;
                OnInitialize(hud);
            }
            return this;
        }

        protected virtual void OnInitialize(HUD hud) 
        {
            state = FrameState.Hidding;
        }

        public Frame Show(Action onCompleted = null, bool instant = false)
        {
            if(IsHidding)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Showing);
                onShowing?.Invoke(this);
                Hud?.OnFrameShowed(this);
                ActiveFrame();
            }
            return this;
        }

        protected virtual void ActiveFrame()
        {
            gameObject.SetActive(true);
            this.DelayFrame(2, OnShowedFrame);
        }

        protected virtual void OnShowedFrame()
        {
            this.onCompleted?.Invoke();
        }

        public Frame Hide(Action onCompleted = null, bool instant = false)
        {
            if(Initialized && !IsHidding)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Hidding);
                onHidding?.Invoke(this);
                Hud?.OnFrameHidden(this);
                DeactiveFrame();
            }
            return this;
        }

        protected virtual void DeactiveFrame()
        {
            gameObject.SetActive(false);
            this.DelayFrame(2, OnHiddenFrame);
        }

        protected virtual void OnHiddenFrame()
        {
            this.onCompleted?.Invoke();
        }

        public Frame Pause(Action onCompleted = null, bool instant = false)
        {
            if(IsShowing)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Pausing);
                onPausing?.Invoke(this);
                PauseFrame();
            }
            return this;
        }

        protected virtual void PauseFrame()
        {
            this.DelayFrame(2, OnPausedFrame);
        }

        protected virtual void OnPausedFrame()
        {
            this.onCompleted?.Invoke();
        }

        public Frame Resume(Action onCompleted = null, bool instant = false)
        {
            if (IsPausing == true)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Showing);
                onResuming?.Invoke(this);
                ResumeFrame();
            }
            return this;
        }

        protected virtual void ResumeFrame()
        {
            this.DelayFrame(2, OnResumedFrame);
        }

        protected virtual void OnResumedFrame()
        {
            this.onCompleted?.Invoke();
        }

        public virtual Frame Back()
        {
            Hide();
            return this;
        }

        public enum FrameState
        {
            NotInitialized,
            Showing,
            Hidding,
            Pausing
        }
    }
}
