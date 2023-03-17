using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    public class ItemData
    {
        public static ItemData Empty = new ItemData(ItemDatabase.NoneId, 0);
        [SerializeField, ItemField] protected int i;

    }
}
