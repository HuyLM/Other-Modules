using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    [CustomEditor(typeof(ParallelDoTweenAnimation)), CanEditMultipleObjects]
    public class ParallelDoTweenAnimationInspector : BaseDoTweenAnimationInspector {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
