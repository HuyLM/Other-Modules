using AtoGame.Base.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.HUD
{
    public class DOTweenFrame : Frame
    {
#if USE_ODIN_INSPECTOR
       [FoldoutGroup("Frame")]
#endif
        [Header("[Animations]")]
#if USE_ODIN_INSPECTOR
       [FoldoutGroup("Frame")]
#endif
        [SerializeField] protected DOTweenAnimation showAnimation;
#if USE_ODIN_INSPECTOR
       [FoldoutGroup("Frame")]
#endif
        [SerializeField] protected DOTweenAnimation hideAnimation;
#if USE_ODIN_INSPECTOR
       [FoldoutGroup("Frame")]
#endif
        [SerializeField] protected DOTweenAnimation pauseAnimation;
#if USE_ODIN_INSPECTOR
       [FoldoutGroup("Frame")]
#endif
        [SerializeField] protected DOTweenAnimation resumeAnimation;

        protected override void OnInitialize(HUD hud)
        {
            base.OnInitialize(hud);
            InitializeAnimation();
        }

        private void InitializeAnimation()
        {
            showAnimation?.Initialize();
            hideAnimation?.Initialize();
            pauseAnimation?.Initialize();
            resumeAnimation?.Initialize();
        }

        protected override void ActiveFrame()
        {
            hideAnimation?.Stop();
            pauseAnimation?.Stop();
            if(instant || showAnimation == null)
            {
                base.ActiveFrame();
            }
            else
            {
                gameObject.SetActive(true);
                showAnimation.Play(OnShowedFrame, true);
            }
        }

        protected override void DeactiveFrame()
        {
            if (instant || hideAnimation == null)
            {
                base.DeactiveFrame();
            }
            else
            {
                gameObject.SetActive(false);
                hideAnimation.Play(OnHiddenFrame, true);
            }
        }

        protected override void ResumeFrame()
        {
            if (instant || resumeAnimation == null)
            {
                base.ResumeFrame();
            }
            else
            {
                resumeAnimation.Play(OnResumedFrame, true);
            }
        }
        protected override void PauseFrame()
        {
            if (instant || pauseAnimation == null)
            {
                base.PauseFrame();
            }
            else
            {
                pauseAnimation.Play(OnPausedFrame, true);
            }
        }

    }
}
