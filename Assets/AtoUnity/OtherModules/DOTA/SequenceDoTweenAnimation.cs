using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class SequenceDoTweenAnimation : BaseDoTweenAnimation {
        [SerializeField, TabGroup("Tab1", "Animation Setting")]
        private BaseDoTweenAnimation[] dotas;

        private int curIndex;

        public override void Play(Action onCompleted)
        {
            base.Play(onCompleted);
            if (dotas.Length == 0)
            {
                CheckOnCompleted();
            }
            else
            {
                curIndex = 0;
                dotas[curIndex].Play(() => {
                    PlayNext();
                });
            }
        }

        private void PlayNext()
        {
            curIndex++;
            if (curIndex < dotas.Length)
            {
                dotas[curIndex].Play(() => {
                    PlayNext();
                });
            }
            else
            {
                CheckOnCompleted();
            }
        }

        public override void Stop(bool complete)
        {
            for (int i = 0; i < dotas.Length; ++i)
            {
                dotas[i].Stop(complete);
            }
            base.Stop(complete);
        }
    }
}
