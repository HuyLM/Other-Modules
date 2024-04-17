using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public class SequenceActionMono : ActionMono
    {
        [SerializeField] ActionMono[] actions;

        private int curIndex;
        private Action onCompleted;
        public override void Execute(Action onCompleted = null)
        {
            this.onCompleted = onCompleted;
            curIndex = 0;
            actions[curIndex].Execute(ExecuteNext);
        }


        private void ExecuteNext()
        {
            curIndex++;
            if(curIndex < actions.Length)
            {
                actions[curIndex].Execute(ExecuteNext);
            }
            else
            {
                this.onCompleted.Invoke();
            }
        }
    }
}
