using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class ParallelDoTweenAnimation : BaseDoTweenAnimation {
        [SerializeField, TabGroup("Tab1", "Animation Setting")]
        private BaseDoTweenAnimation[] dotas;

        public override void Play(Action onCompleted)
        {
            base.Play(onCompleted);
            if (dotas.Length == 0)
            {

            }
            else
            {
                for (int i = 0; i < dotas.Length; ++i)
                {
                    dotaCallingCounter++;
                    dotas[i].Play(()=>{
                        CheckOnCompleted();
                    });
                }
            }
            CheckOnCompleted();
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
