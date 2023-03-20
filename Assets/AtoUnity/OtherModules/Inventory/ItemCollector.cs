using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "ItemCollector", menuName = "Data/OtherModules/Inventory/ItemCollector")]
    public class ItemCollector : ScriptableObject
    {
        [SerializeField] private string nameCollector = "other";
        [SerializeField] private List<ItemConfig> items;
        [SerializeField] private List<ItemCollector> collectors;

        public List<ItemConfig> Items { get => items; }
        public List<ItemCollector> Collectors { get => collectors; }
        public string NameCollector { get => nameCollector; }

#if UNITY_EDITOR
        public void LockAllItem()
        {
            foreach (var i in items)
            {
                i.lockID = true;
                UnityEditor.EditorUtility.SetDirty(i);
            }
            foreach (var c in collectors)
            {
                c.LockAllItem();
            }
        }
#endif
    }
}
