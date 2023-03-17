using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Data/Item/ItemDatabase1")]
    public class ItemDatabase : ScriptableObject
    {
        private static ItemDatabase instance;

        public static bool HasInstance => instance != null;

        public static ItemDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    string path = typeof(ItemDatabase).Name;
                    ItemDatabase data = Resources.Load<ItemDatabase>(path);
                    if (data == null)
                    {
                        Debug.LogErrorFormat("[DATABASE] The asset {0} no found! Require asset in Resources folder", path);
                    }
                    else
                    {
                        Initialize(data);
                    }
                }
                return instance;
            }
            private set { instance = value; }
        }

        public static void Initialize(ItemDatabase data)
        {
            if (data == null)
            {
                Debug.LogErrorFormat($"[DATABASE] The {data.GetType().Name} is null!");
            }
            else
            {
                Instance = data;
                Instance.OnInitialize();
            }
        }

        public const int NoneId = 0;

        [SerializeField] private ItemCollector[] collectors;

        private Dictionary<int, ItemConfig> itemDictionary;
#if UNITY_EDITOR
        private Dictionary<int, ItemTypeName> itemTypeDictionary;
#endif

        [Button("Reload", ButtonSizes.Large)]
        public void OnInitialize()
        {
            itemDictionary = new Dictionary<int, ItemConfig>();
#if UNITY_EDITOR
            itemTypeDictionary = new Dictionary<int, ItemTypeName>();
#endif
            /*
            foreach (ItemCollector collector in collectors)
            {
                foreach (ItemConfig item in collector.Items)
                {
                    if (itemDictionary.ContainsKey(item.Id))
                    {
                        continue;
                    }

                    itemDictionary.Add(item.Id, item);
#if UNITY_EDITOR
                    itemTypeDictionary.Add(item.Id, new ItemTypeName() { Item = item, NameType = collector.NameCollector });
#endif
                }

            }
            */
            foreach (ItemCollector collector in collectors)
            {
                AddCollectorToDictionary(collector, string.Empty);
            }
        }

        private void AddCollectorToDictionary(ItemCollector collector, string path)
        {
            path += collector.NameCollector + "/";
            foreach (var c in collector.Collectors)
            {
                AddCollectorToDictionary(c, path);
            }
            foreach (var i in collector.Items)
            {
                AddItemToDictionary(i, path);
            }
        }

        private void AddItemToDictionary(ItemConfig item, string path)
        {
            if (itemDictionary.ContainsKey(item.Id))
            {
                return;
            }
            itemDictionary.Add(item.Id, item);
#if UNITY_EDITOR
            itemTypeDictionary.Add(item.Id, new ItemTypeName() { Item = item, NameType = path.ToString() });
#endif
        }

        public static int GetCount()
        {
#if UNITY_EDITOR
            return Instance.itemTypeDictionary.Count;
#endif
            return 0;
        }

#if UNITY_EDITOR

        public static IEnumerable<ItemTypeName> GetAllItem(string nameCollector)
        {
            foreach (ItemTypeName item in Instance.itemTypeDictionary.Values)
            {
                string path = item.NameType;
                string[] sub = path.Split('/');
                if (sub.Contains(nameCollector))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<ItemTypeName> GetAllItem()
        {
            foreach (ItemTypeName item in Instance.itemTypeDictionary.Values)
            {
                yield return item;
            }
        }

        [Button("Lock All Item", ButtonSizes.Large)]
        private void LockAllItem()
        {
            foreach (var c in collectors)
            {
                c.LockAllItem();
            }
        }
#endif
        public static IEnumerable<IItemConfig> GetAllItemConfig()
        {
            foreach (var item in Instance.itemDictionary.Values)
            {
                yield return item;
            }
        }

        public static bool TryGetItem(int id, out IItemConfig item)
        {
            if (Instance.itemDictionary.TryGetValue(id, out ItemConfig i))
            {
                item = i;
                return true;
            }

            item = null;
            return false;
        }

        public static bool Constains(int id)
        {
            return Instance.itemDictionary.ContainsKey(id);
        }


#if UNITY_EDITOR
        public class ItemTypeName
        {
            private ItemConfig item;
            private string nameType;

            public ItemConfig Item { get => item; set => item = value; }
            public string NameType { get => nameType; set => nameType = value; }
        }
#endif
    }
}