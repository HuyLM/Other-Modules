using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    public class CanvasGroupDoTween: BaseDoTween
    {
        public override bool CheckShowFloatValues()
        {
            return true;
        }

        public override bool CheckShowCanvasGroupTarget()
        {
            return true;
        }
    }
}
