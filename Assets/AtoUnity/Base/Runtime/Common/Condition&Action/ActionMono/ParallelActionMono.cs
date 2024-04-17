using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public class ParallelActionMono : ActionMono
    {
        [SerializeField] ActionMono[] actions;

        private int completedActionCount;
        private Action onCompleted;
        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            completedActionCount = 0;
            for(int i = 0; i < actions.Length; ++i)
            {
                actions[i].Execute(CompleteAction);
            }
        }


        private void CompleteAction()
        {
            completedActionCount++;
            if(completedActionCount == actions.Length)
            {
                this.onCompleted?.Invoke();
            }
        }
    }
}