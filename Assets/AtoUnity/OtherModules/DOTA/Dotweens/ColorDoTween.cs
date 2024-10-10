using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class ColorDoTween : BaseDoTween {
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
