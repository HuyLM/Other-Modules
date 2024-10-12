using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace AtoGame.OtherModules.DOTA
{
    public class AnchorPositionDoTween : BaseDoTween {

        private Vector2 prePosition;

        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Vector2 endValue = dota.Vector2To;
            if(dota.IsRelative)
            {
                if (dota.FromCurrent)
                {
                    endValue = dota.RectTransformTarget.anchoredPosition + dota.Vector2To;
                }
                else
                {
                    endValue = dota.Vector2From + dota.Vector2To;
                }
            }
            Tween = dota.RectTransformTarget.DOAnchorPos(endValue, dota.BaseOptions.Duration);
            base.CreateTween(dota, onCompleted);
        }

        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if(dota.FromCurrent == false)
            {
                dota.RectTransformTarget.anchoredPosition = dota.Vector2From;
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            prePosition = dota.RectTransformTarget.anchoredPosition;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota.RectTransformTarget.anchoredPosition = prePosition;
        }


        public override bool CheckShowVector2Values()
        {
            return true;
        }

        public override bool CheckShowRectTransformTarget()
        {
            return true;
        }

        public override bool CheckShowIsRelative()
        {
            return true;
        }
    }
}
