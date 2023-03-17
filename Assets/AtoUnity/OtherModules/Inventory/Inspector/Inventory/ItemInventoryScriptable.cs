using Ftech.RacingCar2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "ItemInventoryScriptable", menuName = "Data/Item/ItemInventoryScriptable")]
    public class ItemInventoryScriptable : ScriptableObject
    {
        public ItemInventory ItemInventory { get => SaveDataManager.Instance.Inventory; }
    }
}
