using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class DelayDoTween : BaseDoTween {
        public override void CreateTween(DoTweenAnimation dota, Action onCompleted)
        {
            Tween = DOVirtual.Float(0, 1, dota.BaseOptions.Duration, null);
            base.CreateTween(dota, onCompleted);
        }
    }
}
