using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AtoGame.OtherModules.Tutorial
{
    public interface ITutorialUI 
    {
        public string GetTranslate(string key);
        public void IgnoreInput(bool ignore);
        public void ShowDescriptionText(string description);
        public void ShowDescriptionCustomPosition(Vector2 minAnchor, Vector2 maxAnchor, Vector2 position);
        public void ShowDescriptionPosition();
        public void HideDescription(bool isHideImmediate);
        public void SetShowSkipButton(bool show);
        public void SetShowSkipAllButton(bool show);
        public void SetShowBG(bool show);
        public void HighlightObject(GameObject target);
        public void LowlightObject(GameObject target);
        public Image GetBgImage();
        public void SetBgColor(Color color);
        public void SetOnShowTextBoxCompleted(Action onCompleted);
        public void SetShowTapScreenButton(bool show);
        public void SetOnTapContinue(Action onTapped);
    }
}
