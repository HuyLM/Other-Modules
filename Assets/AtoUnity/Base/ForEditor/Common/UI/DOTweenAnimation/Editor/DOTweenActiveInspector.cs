using UnityEditor;
using UnityEngine;

namespace AtoGame.Base.UI
{
    [CustomEditor(typeof(DOTweenActive)), CanEditMultipleObjects]
    public class DOTweenActiveInspector : DOTweenTransitionInspector
    {
        private DOTweenActive dOTween;
        private SerializedProperty targetProperty;
        private SerializedProperty activeCurrentProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            dOTween = transition as DOTweenActive;
            targetProperty = serializedObject.FindProperty("target");
            activeCurrentProperty = serializedObject.FindProperty("active");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(targetProperty);
            EditorGUILayout.PropertyField(activeCurrentProperty);
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
