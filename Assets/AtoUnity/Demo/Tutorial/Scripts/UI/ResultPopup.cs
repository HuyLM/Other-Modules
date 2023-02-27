
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class ResultPopup : MonoBehaviour
    {
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private Text txtResult;
        [SerializeField] private Button btnOk;

        private void Start()
        {
            btnOk.onClick.AddListener(OnOkButtonClicked);
        }

        public void Show(Result result)
        {
            txtResult.text = result.ToString();
            DOVirtual.DelayedCall(0.25f, () => {
                CheckShowTutorial();
            });
        }

        private void CheckShowTutorial()
        {
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 4, this.gameObject);
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 5, btnOk.gameObject);
            TutorialController.Instance.AssignCallBackStep(TutorialKey.GameTutorial_Keo, 4, onEnd: NextStepTutorial);

            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao, 1, this.gameObject);
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao, 2, btnOk.gameObject);
            TutorialController.Instance.AssignCallBackStep(TutorialKey.GameTutorial_Bao, 1, onEnd: NextStepTutorial);

            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao_Full, 2, this.gameObject);
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao_Full, 3, btnOk.gameObject);
            TutorialController.Instance.AssignCallBackStep(TutorialKey.GameTutorial_Bao_Full, 2, onEnd: NextStepTutorial);
        }

        private void NextStepTutorial()
        {
            TutorialController.Instance.ShowCurrentStep(); //GameTutorial_Keo 5 & GameTutorial_Bao 2 & GameTutorial_Bao_Full 3
        }

        private void OnOkButtonClicked()
        {
            this.gameObject.SetActive(false);
            gamePanel.Show();
        }
    }
}