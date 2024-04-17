using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AtoGame.Base
{
    public class UnityEventActionMono : ActionMono
    {
        [SerializeField] private UnityEvent unityEvent;
        public override void Execute(Action onCompleted = null)
        {
            unityEvent?.Invoke();
            onCompleted?.Invoke();
        }
    }
}