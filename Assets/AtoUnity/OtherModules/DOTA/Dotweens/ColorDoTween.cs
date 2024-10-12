using DG.Tweening;
using System;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class ColorDoTween : BaseDoTween {
        private Color preColor;

        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Tween = dota.GraphicTarget.DOColor(dota.ColorTo, dota.BaseOptions.Duration);
            base.CreateTween(dota, onCompleted);
        }
        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if (dota.FromCurrent == false)
            {
                dota.GraphicTarget.color = (dota.ColorFrom);
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            preColor = dota.GraphicTarget.color;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota.GraphicTarget.color = preColor;
        }

        public override bool CheckShowColorValues()
        {
            return true;
        }

        public override bool CheckShowGraphicTarget()
        {
            return true;
        }
    }
}
