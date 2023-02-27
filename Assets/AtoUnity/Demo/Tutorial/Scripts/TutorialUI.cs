using AtoGame.Base.UI;
using Ftech.Lib.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AtoGame.OtherModules.Tutorial.Demo
{
    public class TutorialUI : MonoBehaviour, ITutorialUI
    {
        [SerializeField] private Button btnSkip;
        [SerializeField] private Text txtDescription;
        [SerializeField] private Button btnTapContinue;
        [SerializeField] private Canvas tutorialCanvas;
        [SerializeField] private Image imgBG;
        [SerializeField] private Button btnSkipAll;

        [Header("Descrition")]
        [SerializeField] private CanvasGroup cgTextBox;
        [SerializeField] private CanvasGroup cgDescription;
        [SerializeField] private RectTransform rtDescriptionContainer;

        [Header("Character")]
        [SerializeField] private Text txtCharacterName;
        [SerializeField] private Image imgCharacter;
        [SerializeField] private RectTransform rtCharacterLeft;
        [SerializeField] private RectTransform rtCharacterRight;

        [Header("Animation")]
        [SerializeField] private DOTweenAnimation showBoxAnim;
        [SerializeField] private DOTweenAnimation hideBoxAnim;


        private Action onTapContinue;
        private Action onDescriptionCompleted;
        private Action onAnyButtonClicked;


        private EventSystem eventSystem;
        protected EventSystem EventSystem
        {
            get
            {
                if (eventSystem == null)
                {
                    eventSystem = EventSystem.current;

                }
                return eventSystem; ;
            }
        }

        //private bool enableCheckAnyButtonClicked = false;

        private void Start()
        {
            btnSkip.onClick.AddListener(OnSkipButtonClicked);
            btnTapContinue.onClick.AddListener(OnTapContinueButtonClicked);
            btnSkipAll.onClick.AddListener(OnSkipAllButtonClicked);
            showBoxAnim.Initialize();
            hideBoxAnim.Initialize();
        }

        public void Init(Action onCompleted)
        {

            onCompleted?.Invoke();
        }

        public string GetTranslate(string key)
        {
            return key;
        }

        public void IgnoreInput(bool ignore)
        {
            if (EventSystem != null)
            {
                EventSystem.enabled = !ignore;
            }
        }

        public void SetDescriptionText(string description)
        {
            txtDescription.text = description;
        }

        public void SetDescriptionCustomPosition(Vector2 minAnchor, Vector2 maxAnchor, Vector2 position)
        {
            rtDescriptionContainer.anchorMin = minAnchor;
            rtDescriptionContainer.anchorMax = maxAnchor;
            rtDescriptionContainer.anchoredPosition = position;
        }

        public void SetDescriptionAutoPosition()
        {

        }

        public void ShowDescription()
        {
            hideBoxAnim.Stop();
            cgTextBox.alpha = 1;
            rtDescriptionContainer.gameObject.SetActive(true);
            showBoxAnim.Play(OnDescriptionCompleted, true);
            StartCoroutine(IDelay());
        }

        public void HideDescription(bool hideImmediate)
        {
            showBoxAnim.Stop();
            if (hideImmediate)
            {
                OnHideCompleted();
            }
            else
            {
                hideBoxAnim.Play(OnHideCompleted, true);
            }
        }

        public void SetShowSkipButton(bool show)
        {
            btnSkip.gameObject.SetActive(show);
        }

        public void SetShowSkipAllButton(bool show)
        {
            btnSkipAll.gameObject.SetActive(show);
        }

        public void SetShowBG(bool show)
        {
            imgBG.gameObject.SetActive(show);
        }

        public void HighlightObject(GameObject target)
        {
            Canvas canvas = target.AddComponent<Canvas>();
            target.AddComponent<GraphicRaycaster>();
            canvas.overridePixelPerfect = true;
            canvas.pixelPerfect = false;
            canvas.overrideSorting = true;
            canvas.sortingLayerName = tutorialCanvas.sortingLayerName;
            canvas.sortingOrder = tutorialCanvas.sortingOrder + 5;
        }

        public void LowlightObject(GameObject target)
        {
            Destroy(target.GetComponent<GraphicRaycaster>());
            Destroy(target.GetComponent<Canvas>());
        }

        public Image GetBgImage()
        {
            return imgBG;
        }

        public void SetBgColor(Color color)
        {
            imgBG.color = color;
        }

        public void SetOnShowTextBoxCompleted(Action onCompleted)
        {
            this.onDescriptionCompleted = onCompleted;
        }

        public void SetShowTapScreenButton(bool show)
        {
            btnTapContinue.gameObject.SetActive(show);
        }

        public void SetOnTapScreenButton(Action onTapContinue)
        {
            this.onTapContinue = onTapContinue;
        }

        private IEnumerator IDelay()
        {
            yield return new WaitForSecondsRealtime(1f);
            OnDescriptionCompleted();
        }

        public void OnDescriptionCompleted()
        {
            onDescriptionCompleted?.Invoke();
        }

        #region Events
        private void OnTapContinueButtonClicked()
        {
            onTapContinue?.Invoke();
        }

        private void OnSkipButtonClicked()
        {
            TutorialController.Instance.Skip();
        }

        private void OnSkipAllButtonClicked()
        {
            TutorialController.Instance.SkipAll();
        }
        #endregion

        #region Animation Events
        public void OnHideCompleted()
        {
            rtDescriptionContainer.gameObject.SetActive(false);
            cgTextBox.alpha = 0;
        }

        #endregion
    }
}
