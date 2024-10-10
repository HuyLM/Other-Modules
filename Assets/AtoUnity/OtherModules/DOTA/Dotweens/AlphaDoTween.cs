using System;
using DG.Tweening;
using AtoGame.Base.Helper;
using static UnityEngine.GraphicsBuffer;

namespace AtoGame.OtherModules.DOTA
{
    public class AlphaDoTween : BaseDoTween
    {
        private float preAlpha;

        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Tween = dota._graphicTarget.DOFade(dota._floatValues.To, dota._baseOptions.Duration);
            base.CreateTween(dota, onCompleted);
        }
        public override void ResetState(DoTweenAnimation dota)
        {
            base.ResetState(dota);
            if (dota._floatValues.FromCurrent == false)
            {
                dota._graphicTarget.ChangeAlpha(dota._floatValues.From);
            }
        }

        public override void Save(DoTweenAnimation dota)
        {
            base.Save(dota);
            preAlpha = dota._graphicTarget.color.a;
        }

        public override void Load(DoTweenAnimation dota)
        {
            base.Load(dota);
            dota._graphicTarget.ChangeAlpha(preAlpha);
        }

        public override bool CheckShowFloatValues()
        {
            return true;
        }

        public override bool CheckShowGraphicTarget()
        {
            return true;
        }

    }
}
