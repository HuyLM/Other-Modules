
using UnityEngine;

namespace AtoGame.TheKiwiCoder.BT.Demo
{
    public class RandomPosition : ActionNode
    {
        public Vector2 min = Vector2.one * -10;
        public Vector2 max = Vector2.one * 10;

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            BasicRoamingBlackboard basicRoamingBlackboard = tree.blackboard as BasicRoamingBlackboard;
            basicRoamingBlackboard.moveToPosition.x = Random.Range(min.x, max.x);
            basicRoamingBlackboard.moveToPosition.z = Random.Range(min.y, max.y);
            return State.Success;
        }
    }
}