using DG.Tweening;
using System;

namespace AtoGame.OtherModules.DOTA
{
    public class DelayDoTween : BaseDoTween {
        public override void CreateTween(TweenAnimation dota, Action onCompleted)
        {
            Tween = DOVirtual.Float(0, 1, dota.BaseOptions.Duration, null);
            base.CreateTween(dota, onCompleted);
        }
    }
}
