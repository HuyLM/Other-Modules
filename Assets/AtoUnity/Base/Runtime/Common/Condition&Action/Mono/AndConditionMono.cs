using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AtoGame.Base
{
    public class AndConditionMono : ConditionMono
    {
        [SerializeField] protected ConditionMono[] subConditions;

        public override bool CheckCondition()
        {
            for(int i = 0; i < subConditions.Length; ++i)
            {
                if(subConditions[i].CheckCondition() == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}