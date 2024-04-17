using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AtoGame.Base
{
    public class OrConditionMono : ConditionMono
    {
        [SerializeField] protected ConditionMono[] subConditions;

        public override bool CheckCondition()
        {
            for (int i = 0; i < subConditions.Length; ++i)
            {
                if (subConditions[i].CheckCondition() == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}