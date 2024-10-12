using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    [CustomEditor(typeof(DoTweenAnimation)), CanEditMultipleObjects]
    public class DoTweenAnimationInspector : BaseDoTweenAnimationInspector {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
