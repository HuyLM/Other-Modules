using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class ShakePositionDoTween : BaseDoTween {
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


        public override string GetFloat1Lable()
        {
            return "Randomness";
        }

        public override string GetInt1Lable()
        {
            return "Vibrato";
        }

        public override string GetBool1Lable()
        {
            return "FadeOut";
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }
    }
}
