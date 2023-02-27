using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class MainLobbyPanel : MonoBehaviour
    {
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private ShopPanel shopPanel;

        [SerializeField] private Button btnStartGame;
        [SerializeField] private Button btnShop;

        [SerializeField] private TutorialConfig tutorialConfig;
        [SerializeField] private TutorialUI tutorialUI;

        private void Start()
        {
            TutorialController.Instance.Init(tutorialConfig, new TutorialSave(), tutorialUI);

            btnStartGame.onClick.AddListener(OnStartGameButtonClicked);
            btnShop.onClick.AddListener(OnShopButtonClicked);
            Show();
        }

        public void Show()
        {
            DOVirtual.DelayedCall(0.25f, () => {
                CheckShowTutorial();
            });
        }

        private void CheckShowTutorial()
        {
            TutorialController.Instance.AssignTarget(TutorialKey.ShopBuyTutorial, 0, btnShop.gameObject);
            TutorialController.Instance.ShowTutorial(TutorialKey.ShopBuyTutorial);

            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Keo, 0, btnStartGame.gameObject);
            TutorialController.Instance.ShowTutorial(TutorialKey.GameTutorial_Keo);


            TutorialController.Instance.AssignTarget(TutorialKey.GameTutorial_Bao_Full, 0, btnStartGame.gameObject);
            TutorialController.Instance.ShowTutorial(TutorialKey.GameTutorial_Bao_Full);
        }

        private void OnStartGameButtonClicked()
        {
            this.gameObject.SetActive(false);
            gamePanel.gameObject.SetActive(true);
            gamePanel.Show();
        }

        private void OnShopButtonClicked()
        {
            this.gameObject.SetActive(false);
            shopPanel.gameObject.SetActive(true);
            shopPanel.Show();
        }
    }
}