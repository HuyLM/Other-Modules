using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    public interface IItemInventorySaver
    {
        void Init(Action onCompleted);
        void Load(Action<bool, ItemInventory> onLoaded);
        void PushSave(Action<bool> onResult);
    }
}
