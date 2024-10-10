using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class PunchAnchorPositionDoTween : BaseDoTween {
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

        public override bool CheckShowRectTransformTarget()
        {
            return true;
        }
    }
}
