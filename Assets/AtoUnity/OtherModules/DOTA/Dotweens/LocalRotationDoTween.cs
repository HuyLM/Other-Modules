using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class LocalRotationDoTween : BaseDoTween {
        public override bool CheckShowVector3Values()
        {
            return true;
        }

        public override bool CheckShowRotateMode()
        {
            return true;
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }
    }
}
