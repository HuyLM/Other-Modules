using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class LocalPositionDoTween : BaseDoTween {
        public override bool CheckShowRelativeVector3Values()
        {
          return true;
        }

        public override bool CheckShowTransformTarget()
        {
            return true;
        }
    }
}
