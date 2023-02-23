using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtoGame.OtherModules.Tutorial
{
    [CreateAssetMenu(fileName = "FocusButtonStep", menuName = "Data/OtherModules/Tutorial/Step/FocusButton")]
    public class FocusButtonStep : TutorialStep
    {
        [SerializeField] private bool useManualEndStep;

        private Button btnTarget;

        public override void AssignTarget(GameObject target)
        {
            if (target == null)
            {
                TutorialController.Instance.Log($"Target assgin is null");
                btnTarget = null;
                return;
            }
            Button btn = target.GetComponent<Button>();
            if (btn == null)
            {
                TutorialController.Instance.Log($"Target assgin is not Button");
                return;
            }
            btnTarget = btn;
        }

        public override void RemoveReferences()
        {
            btnTarget = null;
        }

        protected override void OnShow()
        {
            base.OnShow();
            Show();
        }

        public override void EndStep()
        {
            if(useManualEndStep)
            {
                Hide();
            }
            base.EndStep();
        }

        public void Show()
        {
            if (btnTarget == null)
            {
                TutorialController.Instance.Log("Target is null in show step");
                return;
            }
            tutorialUI.HighlightObject(btnTarget.gameObject);
            btnTarget.onClick.AddListener(OnTargetButtonClicked); // FIFO
            IHighlightComponent highlightComponent = btnTarget.GetComponent<IHighlightComponent>();
            if (highlightComponent != null)
            {
                highlightComponent.Show();
            }
        }

        private void Hide()
        {
            if (btnTarget == null)
            {
                TutorialController.Instance.Log("Target is null in end step");
                return;
            }
            tutorialUI.LowlightObject(btnTarget.gameObject);
            btnTarget.onClick.RemoveListener(OnTargetButtonClicked);
            IHighlightComponent highlightComponent = btnTarget.GetComponent<IHighlightComponent>();
            if (highlightComponent != null)
            {
                highlightComponent.Hide();
            }
        }
        private void OnTargetButtonClicked()
        {
            if(useManualEndStep)
            {
                Hide();
            }
            else
            {
                EndStep();
            }
        }
    }
}
