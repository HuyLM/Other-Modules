using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class GroupLayoutSpacingDoTween : BaseDoTween {
        public override bool CheckShowFloatValues()
        {
            return true;
        }

        public override bool CheckShowHorizontalOrVerticalLayoutGroupTarget()
        {
            return true;
        }
    }
}
