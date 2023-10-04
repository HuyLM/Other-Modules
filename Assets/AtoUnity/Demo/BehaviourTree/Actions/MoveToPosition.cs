using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.TheKiwiCoder.BT.Demo
{
    public class MoveToPosition : ActionNode
    {
        public float speed = 5;
        public float stoppingDistance = 0.1f;
        public bool updateRotation = true;
        public float acceleration = 40.0f;
        public float tolerance = 1.0f;

        private BasicRoamingContext basicRoamingContext;

        protected override void OnStart()
        {
            basicRoamingContext = tree.context as BasicRoamingContext;
            BasicRoamingBlackboard basicRoamingBlackboard = tree.blackboard as BasicRoamingBlackboard;
            basicRoamingContext.agent.stoppingDistance = stoppingDistance;
            basicRoamingContext.agent.speed = speed;
            basicRoamingContext.agent.destination = basicRoamingBlackboard.moveToPosition;
            basicRoamingContext.agent.updateRotation = updateRotation;
            basicRoamingContext.agent.acceleration = acceleration;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            BasicRoamingContext basicRoamingContext = tree.context as BasicRoamingContext;
            if (basicRoamingContext.agent.pathPending)
            {
                return State.Running;
            }

            if (basicRoamingContext.agent.remainingDistance < tolerance)
            {
                return State.Success;
            }

            if (basicRoamingContext.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }
    }
}