using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace AtoGame.Base.UI
{
    [RequireComponent(typeof(CanvasScaler))]
    public class AutoCanvasScaler : MonoBehaviour
    {
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField, Range(0f, 10f)] private float updateRate = 1f;
        [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(1.33f, 0f, 2f, 1f);

        private WaitForSeconds waitUpdate;
        private float currentAspect;

        private void Reset()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            canvasScaler = GetComponent<CanvasScaler>();
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        }

        private void Awake()
        {
            if (updateRate > 0)
            {
                waitUpdate = new WaitForSeconds(updateRate);
            }
            else
            {
                waitUpdate = null;
            }
        }

        private void OnEnable()
        {
            StartCoroutine(AutoUpdate());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        [ContextMenu("CalculatorScale")]
#if ODIN_INSPECTOR
        [Button("CalculatorScale")]
#endif
        private void CalculatorScale()
        {
            Camera camera = Camera.main;

            if (camera == null) return;
            if (canvasScaler == null) return;
            if (camera.aspect == currentAspect) return;

            currentAspect = camera.aspect;
            canvasScaler.matchWidthOrHeight = curve.Evaluate(currentAspect);
        }

        private IEnumerator AutoUpdate()
        {
            while (true)
            {
                CalculatorScale();
                if (waitUpdate == null)
                {
                    yield break;
                }
                yield return waitUpdate;
            }
        }
    }
}