using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class PunchRotationDoTween : BaseDoTween {

        private Vector3 preValue;

        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Vector3 endValue = dota.Vector3To;
            Tween = dota.TransformTarget.DOPunchRotation(endValue, dota.BaseOptions.Duration, dota.IntValue_1, dota.FloatValue_1);
            base.CreateTween(dota, onCompleted);
        }
        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if (dota.FromCurrent == false)
            {
                dota.TransformTarget.localRotation = Quaternion.Euler(dota.Vector3From);
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            preValue = dota.TransformTarget.localRotation.eulerAngles;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota.TransformTarget.localRotation = Quaternion.Euler(preValue);
        }


        public override bool CheckShowVector3Values()
        {
            return true;
        }

        public override bool CheckShowFloat1()
        {
            return true;
        }

        public override bool CheckShowInt1()
        {
            return true;
        }

        public override string GetFloat1Lable()
        {
            return "Elasticity";
        }

        public override string GetInt1Lable()
        {
            return "Vibrato";
        }
        public override string GetToLable()
        {
            return "Punch";
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }
    }
}
