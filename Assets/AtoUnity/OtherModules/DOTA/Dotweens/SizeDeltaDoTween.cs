using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class SizeDeltaDoTween : BaseDoTween {
        public override bool CheckShowRelativeVector2Values()
        {
            return true;
        }

        public override bool CheckShowRectTransformTarget()
        {
            return true;
        }
    }
}
