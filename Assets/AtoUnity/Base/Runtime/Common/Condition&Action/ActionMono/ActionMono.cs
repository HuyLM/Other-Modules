using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.Base
{
    public abstract class ActionMono : MonoBehaviour
    {
        public abstract void Execute(Action onCompleted = null);
    }
}