#if ODIN_INSPECTOR
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
#if ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onShowing;
#if ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onHidding;
#if ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onPausing;
#if ODIN_INSPECTOR
        [FoldoutGroup("Frame")]
#endif
        [SerializeField] private FrameEvent onResuming;

        protected FrameState state = FrameState.NotInitialized;
        protected HUD hud;

        protected Action onCompleted;
        protected bool instant;

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

        public Frame ShowByHUD(Action onCompleted = null, bool instant = false) // warning: Awake -> ShowByHUD -> ActiveFrame -> Start -> OnShowedFrame
        {
            if(IsHidding)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Showing);
                onShowing?.Invoke(this);
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

        public Frame HideByHUD(Action onCompleted = null, bool instant = false)
        {
            if(Initialized && !IsHidding)
            {
                this.onCompleted = onCompleted;
                this.instant = instant;
                SetState(FrameState.Hidding);
                onHidding?.Invoke(this);
                DeactiveFrame();
            }
            return this;
        }

        protected virtual void DeactiveFrame()
        {
            this.DelayFrame(2, OnHiddenFrame);
        }

        protected virtual void OnHiddenFrame()
        {
            gameObject.SetActive(false);
            this.onCompleted?.Invoke();
        }

        public Frame PauseByHUD(Action onCompleted = null, bool instant = false)
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

        public Frame ResumeByHUD(Action onCompleted = null, bool instant = false)
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
            Hud.BackToPrevious(null);
            return this;
        }

        public void Hide()
        {
            hud.Hide(this);
        }

        public void Show()
        {
            hud.Show(this);
        }

        public void Pause()
        {
            hud.Pause(this);
        }

        public void Resume()
        {
            hud.Resume(this);
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
