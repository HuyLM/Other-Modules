using AtoGame.Base.Helper;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private MainLobbyPanel mainLobbyPanel;
        [SerializeField] private ResultPopup resultPopup;

        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnKeo;
        [SerializeField] private Button btnBua;
        [SerializeField] private Button btnBao;

        [SerializeField] private Text txtAiChoose;
        [SerializeField] private Text txtPlayerChoose;

        [Header("For Tutorial")]
        [SerializeField] private GameObject goBattleInfo;
        [SerializeField] private GameObject goButtons;

        [Header("Configs")]
        [SerializeField] private int randomNumber = 5;
        [SerializeField] private float randomWait = 0.5f;
        [SerializeField] private float delayShowResult = 2f;

        private Choose playerChoose;
        private Choose aiChoose;
        private Choose[] results = new Choose[] { Choose.Keo, Choose.Bua, Choose.Bao };

        private void Start()
        {
            btnBack.onClick.AddListener(OnBackButtonClicked);
            btnKeo.onClick.AddListener(OnKeoButtonClicked);
            btnBua.onClick.AddListener(OnBuaButtonClicked);
            btnBao.onClick.AddListener(OnBaoButtonClicked);
        }

        public void Show()
        {
            txtAiChoose.text = string.Empty;
            txtPlayerChoose.text = string.Empty;

            DOVirtual.DelayedCall(0.25f, () => {
                CheckShowTutorial();
            });
        }

        private void CheckShowTutorial()
        {
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 1, goBattleInfo);
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 2, goButtons);
            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 3, btnKeo.gameObject);

            TutorialController.Instance.AssignCallBackStep(TutorialKey.GameTutorial_Keo, 1, onEnd: NextStepTutorial);
            TutorialController.Instance.AssignCallBackStep(TutorialKey.GameTutorial_Keo, 2, onEnd: NextStepTutorial);

            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao, 0, btnBao.gameObject);

            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao_Full, 1, btnBao.gameObject);

            TutorialController.Instance.ShowCurrentStep(); // GameTutorial_Keo 1 & GameTutorial_Bao_Full 1
            TutorialController.Instance.ShowTutorial(TutorialKey.GameTutorial_Bao);
        }

        private void NextStepTutorial()
        {
            TutorialController.Instance.ShowCurrentStep(); // GameTutorial_Keo 2 & GameTutorial_Keo 3
        }

        private void OnBackButtonClicked()
        {
            this.gameObject.SetActive(false);
            resultPopup.gameObject.SetActive(false);
            mainLobbyPanel.gameObject.SetActive(true);
            mainLobbyPanel.Show();
        }

        private void OnKeoButtonClicked()
        {
            playerChoose = Choose.Keo;
            txtPlayerChoose.text = playerChoose.ToString();
            StartAiChoose();
        }

        private void OnBuaButtonClicked()
        {
            playerChoose = Choose.Bua;
            txtPlayerChoose.text = playerChoose.ToString();
            StartAiChoose();
        }

        private void OnBaoButtonClicked()
        {
            playerChoose = Choose.Bao;
            txtPlayerChoose.text = playerChoose.ToString();
            StartAiChoose();
        }

        private void StartAiChoose()
        {
            SetStateButton(btnBack, false, true);
            SetStateButton(btnKeo, false, true);
            SetStateButton(btnBua, false, true);
            SetStateButton(btnBao, false, true);

            StartCoroutine(IChooseAiResult());
        }

        private IEnumerator IChooseAiResult()
        {
            if (TutorialController.Instance.CheckTutorialCompleted(TutorialKey.GameTutorial_Keo) == false)
            {
                aiChoose = Choose.Bao;
            }
            else if (TutorialController.Instance.CheckTutorialCompleted(TutorialKey.GameTutorial_Bao) == false && TutorialController.Instance.CheckTutorialCompleted(TutorialKey.GameTutorial_Bao_Full) == false)
            {
                aiChoose = Choose.Bua;
            }
            else
            {
                aiChoose = RandomHelper.RandomInCollection(results);
            }

            int randomIndex = Random.Range(0, results.Length);
            for (int i = 0; i < randomNumber; ++i)
            {
                txtAiChoose.text = results[(i + randomIndex) % results.Length].ToString();
                yield return new WaitForSeconds(randomWait);
            }


            txtAiChoose.text = aiChoose.ToString();

            yield return new WaitForSeconds(delayShowResult);

            // popup result

            Result result = Result.Draw;
            if (playerChoose == aiChoose)
            {
                result = Result.Draw;
            }
            else if ((int)playerChoose == (int)aiChoose + 1)
            {
                result = Result.Win;
            }
            else if ((int)playerChoose == (int)aiChoose - 2)
            {
                result = Result.Win;
            }
            else
            {
                result = Result.Lose;
            }
            resultPopup.gameObject.SetActive(true);
            resultPopup.Show(result);

            SetStateButton(btnBack, true, true);
            SetStateButton(btnKeo, true, true);
            SetStateButton(btnBua, true, true);
            SetStateButton(btnBao, true, true);

            TutorialController.Instance.ShowCurrentStep(); // GameTutorial_Keo 4 & GameTutorial_Bao 1 & GameTutorial_Bao_Full 2
        }

        private void SetStateButton(Button button, bool interaction, bool show)
        {
            if (button)
            {
                button.gameObject.SetActive(show);
                if (show)
                {
                    button.interactable = interaction;
                }
            }
        }
    }

    public enum Choose { Keo, Bua, Bao }
    public enum Result { Win, Lose, Draw }
}