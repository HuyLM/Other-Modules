using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class LocalScaleDoTween : BaseDoTween {
        private Vector3 preValue;


        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Vector3 endValue = dota.Vector3To;
            if (dota.IsRelative)
            {
                if (dota.FromCurrent)
                {
                    endValue = dota.TransformTarget.localScale + dota.Vector3To;
                }
                else
                {
                    endValue = dota.Vector3From + dota.Vector3To;
                }
            }
            Tween = dota.TransformTarget.DOScale(endValue, dota.BaseOptions.Duration);
            base.CreateTween(dota, onCompleted);
        }
        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if (dota.FromCurrent == false)
            {
                dota.TransformTarget.localScale = (dota.Vector3From);
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            preValue = dota.TransformTarget.localScale;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota.TransformTarget.localScale = (preValue);
        }


        public override bool CheckShowVector3Values()
        {
            return true;
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }

        public override bool CheckShowIsRelative()
        {
            return true;
        }
    }
}
