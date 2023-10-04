using UnityEngine;

namespace AtoGame.TheKiwiCoder.BT.Demo
{
    [CreateAssetMenu(fileName = "BasicRoamingBlackboard", menuName = "")]
    public class BasicRoamingBlackboard : Blackboard
    {
        [SerializeField] public Vector3 moveToPosition;
      
	}
}
