using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Tutorial
{
    public interface ITutorialSave
    {
        void Save(int[] keys, Action<bool> onResult);
        void ForceSave(Action<bool> onResult);
        bool IsTutorialCompleted();
        bool SetTutorialCompleted();
        bool CheckHasKey(int key);
    }

}
