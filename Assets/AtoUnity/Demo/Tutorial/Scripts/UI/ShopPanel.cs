
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField] private MainLobbyPanel mainLobbyPanel;

        [SerializeField] private Button btnBuy1;
        [SerializeField] private Button btnBuy2;
        [SerializeField] private Button btnBack;
        [SerializeField] private GameObject goButtons;

        private void Start()
        {
            btnBuy1.onClick.AddListener(OnBuy1ButtonClicked);
            btnBuy2.onClick.AddListener(OnBuy2ButtonClicked);
            btnBack.onClick.AddListener(OnBackButtonClicked);
        }

        public void Show()
        {
            DOVirtual.DelayedCall(0.25f, ()=>{ 
                CheckShowTutorial();
            });
        }

        private void CheckShowTutorial()
        {
            TutorialController.Instance.AssignTarget(TutorialKey.ShopBuyTutorial, 1, goButtons);
            TutorialController.Instance.AssignTarget(TutorialKey.ShopBuyTutorial, 2, btnBuy2.gameObject);
            TutorialController.Instance.AssignTarget(TutorialKey.ShopBuyTutorial, 3, btnBack.gameObject);
        }

        private void OnBuy1ButtonClicked()
        {
            Debug.Log("Buy1");
        }

        private void OnBuy2ButtonClicked()
        {
            Debug.Log("Buy2");
        }


        private void OnBackButtonClicked()
        {
            this.gameObject.SetActive(false);
            mainLobbyPanel.gameObject.SetActive(true);
            mainLobbyPanel.Show();
        }
    }
}