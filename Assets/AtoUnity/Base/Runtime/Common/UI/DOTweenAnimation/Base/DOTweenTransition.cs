﻿using UnityEngine;
using System;
using DG.Tweening;

namespace AtoGame.Base.UI
{
    public abstract class DOTweenTransition : MonoBehaviour
    {
        [SerializeField] private bool stopOnDisable = true;
        [SerializeField, Range(0f, 10f)] private float delay = 0f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private bool isSpeedBase;
        [SerializeField] private bool ignoreTimeScale = false;
        [SerializeField] private int loopNumber = 1;
        [SerializeField] private LoopType loopType = LoopType.Restart;
        [SerializeField] private Ease ease = Ease.Linear;
        [SerializeField] private AnimationCurve curve;

        private bool isCompleted;

        public bool StopOnDisable { get => stopOnDisable; set => stopOnDisable = value; }
        public bool IsSpeedBase { get => isSpeedBase; set => isSpeedBase = value; }
        public float Duration { get => duration; set => duration = value; }
        public float Delay { get => delay; set => delay = value; }
        public float TotalDuration { get => Duration * loopNumber + Delay; }
        public Ease Ease { get => ease; set => ease = value; }
        public bool IgnoreTimeScale { get => ignoreTimeScale; set => ignoreTimeScale = value; }

        public int LoopNumber { get => loopNumber; set => loopNumber = value; }

        public LoopType LoopType { get => loopType; set => loopType = value; }

        public AnimationCurve Curve { get => curve; set => curve = value; }
        public Tween Tween { get; protected set; }

        public bool IsCompleted { get => isCompleted; }

        private void OnDisable()
        {
            if(stopOnDisable == true)
            {
                Stop();
            }
        }

        public void OnDestroy()
        {
            if(stopOnDisable == false)
            {
                Stop();
            }
        }

        public void Stop(bool onComplete = false)
        {
            Tween?.Kill(onComplete);
            if (onComplete && loopNumber < 0)
            {
                ToEndState();
            }
        }
        public abstract void ResetState();
        public abstract void ToEndState();
        public virtual void DoTransition(Action onCompleted, bool restart)
        {
            isCompleted = false;
            if (restart)
            {
                ResetState();
            }
            CreateTween(onCompleted);
        }

        public virtual void CreateTween(Action onCompleted)
        {
            if (Tween != null)
            {
                Tween.SetSpeedBased(isSpeedBase).SetUpdate(IgnoreTimeScale)
                            .SetDelay(Delay).OnComplete(() =>
                            {
                                isCompleted = true;
                                onCompleted?.Invoke();
                            });
                if (loopNumber != 0 && loopNumber != 1)
                {
                    Tween.SetLoops(loopNumber, loopType);
                }
                if (ease == Ease.INTERNAL_Custom)
                {
                    Tween.SetEase(curve);
                }
                else
                {
                    Tween.SetEase(ease);
                }
            }
        }

#if UNITY_EDITOR

        public abstract void Save();
        public abstract void Load();

        public void PlayPreview()
        {
            Save();
            ResetState();
            CreateTween(null);
        }


        public void StopPreview()
        {
            Tween?.Rewind();
            Tween?.Kill(true);
            Load();
        }
#endif
    }
}