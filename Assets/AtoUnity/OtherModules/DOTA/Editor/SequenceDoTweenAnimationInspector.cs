using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    [CustomEditor(typeof(SequenceDoTweenAnimation)), CanEditMultipleObjects]
    public class SequenceDoTweenAnimationInspector : BaseDoTweenAnimationInspector {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
