using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public abstract class ActionMono : MonoBehaviour
    {
        [SerializeField] protected ActionMono nextAction;
        public abstract void Execute(Action onCompleted = null);

        protected virtual void OnComplete(Action onComplete)
        {
            if(nextAction != null)
            {
                nextAction.Execute(onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
    }
}