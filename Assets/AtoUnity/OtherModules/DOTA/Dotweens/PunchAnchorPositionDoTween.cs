using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class PunchAnchorPositionDoTween : BaseDoTween {
        private Vector2 preValue;

        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Vector2 endValue = dota.Vector2To;
            Tween = dota.RectTransformTarget.DOPunchAnchorPos(endValue, dota.BaseOptions.Duration, dota.IntValue_1, dota.FloatValue_1);
            base.CreateTween(dota, onCompleted);
        }
        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if (dota.FromCurrent == false)
            {
                dota.RectTransformTarget.anchoredPosition = (dota.Vector2From);
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            preValue = dota.RectTransformTarget.anchoredPosition;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota.RectTransformTarget.anchoredPosition = (preValue);
        }


        public override bool CheckShowVector2Values()
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

        public override bool CheckShowRectTransformTarget()
        {
            return true;
        }
    }
}
