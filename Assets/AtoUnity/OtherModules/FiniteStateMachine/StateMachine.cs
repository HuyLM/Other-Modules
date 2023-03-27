using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.FSM
{
    public abstract class StateMachine<T> where T : IContext
    {
        protected T context;
        State<T> currentState;
        protected List<Transition<T>> anyStateTransitions;


        public T Context { get => context; }

        public virtual void Initialize(T context)
        {
            this.context = context;
            // s1 create states
            // s2 create trasitions
        }

        public void Updating()
        {
            // Do Always Actions
            DoAlwaysActions();
            // Transition From Any States
            CheckTransitionFromAnyStates();
            // Update Current State
            currentState?.UpdateState(this);
        }

        protected abstract void DoAlwaysActions();
        private void CheckTransitionFromAnyStates()
        {
            if (anyStateTransitions == null)
            {
                return;
            }
            for (int i = 0; i < anyStateTransitions.Count; ++i)
            {
                if (anyStateTransitions[i].CheckTransition(this))
                {
                    return;
                }
            }
        }

        public void TransitionToState(State<T> nextState, Transition<T> transition)
        {
            if (nextState != null && nextState != currentState && currentState != null)
            {
                transition.DoBeforeTransitionActions(this);
                currentState.EndState(this);
                transition.DoWhileTransitionActions(this);
                SetCurrentState(nextState);
                currentState.StartState(this);
                transition.DoAfterTransitionActions(this);
            }
        }

        protected void SetCurrentState(State<T> currentState)
        {
            this.currentState = currentState;
        }

        void OnDrawGizmos(Transform transform)
        {
            if (currentState != null)
            {
                Gizmos.color = currentState.SceneGizmoColor;
                Gizmos.DrawWireSphere(transform.position, 0.5f);
            }
        }
    }
}
