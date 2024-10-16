using DG.Tweening;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AtoGame.OtherModules.DOTA
{
    [CustomEditor(typeof(BaseDoTweenAnimation)), CanEditMultipleObjects]
    public abstract class BaseDoTweenAnimationInspector : OdinEditor {
        private static List<DoTweenAnimation> playingDOTAList = new List<DoTweenAnimation>();
        private bool isPlaying;
        protected BaseDoTweenAnimation dotweenAnimation;

        protected override void OnEnable()
        {
            base.OnEnable();
            isPlaying = false;
            dotweenAnimation = target as BaseDoTweenAnimation;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Stop();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(isPlaying);
            if (GUILayout.Button("Play", GUILayout.Height(50)))
            {
                Play();
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(!isPlaying);
            if (GUILayout.Button("Stop", GUILayout.Height(50)))
            {
                Stop();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            base.OnInspectorGUI();
        }

        public void PrepareTweenForPreview(DoTweenAnimation dota, Tween tween)
        {
            playingDOTAList.Insert(0, dota);
            DG.DOTweenEditor.DOTweenEditorPreview.PrepareTweenForPreview(tween, false);
        }

        private void Play()
        {
            playingDOTAList.Clear();
            isPlaying = true;
            DG.DOTweenEditor.DOTweenEditorPreview.Start();
            DoTweenAnimation.AddPrepareTweenForPreviewFunction(PrepareTweenForPreview);
            dotweenAnimation.Play();
        }


        private void Stop()
        {
            if (isPlaying)
            {
                isPlaying = false;
                for (int i = playingDOTAList.Count - 1; i >= 0; i--)
                {
                    playingDOTAList[i].Stop();
                }
                DG.DOTweenEditor.DOTweenEditorPreview.Stop();
            }
        }
    }
}
