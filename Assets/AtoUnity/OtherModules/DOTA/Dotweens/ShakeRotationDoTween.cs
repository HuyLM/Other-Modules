using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class ShakeRotationDoTween : BaseDoTween {
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

        public override bool CheckShowBool1()
        {
            return true;
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }
    }
}