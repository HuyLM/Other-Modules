using UnityEngine;
using UnityEngine.Serialization;
using Newtonsoft.Json;
using OtherModules.Inventory.Editor;
using System.Collections.Generic;

namespace OtherModules.Inventory
{
    [System.Serializable]
    public class ItemData : IItemData
    {
        public static ItemData Empty = new ItemData(ItemDatabase.NoneId, 0);

        [FormerlySerializedAs("item")]
        [SerializeField, ItemField, JsonProperty] protected int i;
        [FormerlySerializedAs("amount")]
        [SerializeField, JsonProperty] protected long a;

        [JsonIgnore] private IItemConfig item;

        [JsonIgnore]
        public IItemConfig ItemConfig
        {
            get
            {
                if (item == null)
                {
                    ItemDatabase.TryGetItem(Id, out item);
                }
                return item;
            }
        }

        [JsonIgnore]
        public long Amount
        {
            set
            {
                a = value;
            }

            get
            {
                return a;
            }
        }

        [JsonIgnore] public int Id => i;

        [JsonIgnore]
        public string Name
        {
            get
            {
                return ItemConfig?.Name;
            }
        }

        [JsonIgnore]
        public string Description
        {
            get
            {
                return ItemConfig?.Description;
            }
        }

        [JsonIgnore]
        public Sprite Icon
        {
            get
            {
                return ItemConfig?.Icon;
            }
        }

        [JsonIgnore] public bool IsEmpty => Id == ItemDatabase.NoneId || a <= 0;

        public ItemData(int itemId, long amount)
        {
            this.i = itemId;
            this.a = amount;
        }

        public override string ToString()
        {
            return $"{Name} - {Amount}";
        }

        public string ToShortString()
        {
            return $"{Id}{Amount}";
        }

        public void Stack(long amount)
        {
            this.a += amount;
        }

        public void Destack(long amount)
        {
            this.a -= amount;
            if (this.a < 0)
            {
                this.a = 0;
            }
        }

        public IItemData[] Claim(bool withNotify, string position)
        {
            IItemConfig item = ItemConfig;
            if (item != null)
            {
                return item.Claim(Amount, withNotify, position);
            }
            return null;
        }

        public void Claim(float multi, bool withNotify, string position)
        {
            IItemConfig item = ItemConfig;
            if (item != null)
            {
                item.Claim(Mathf.RoundToInt(Amount * multi), withNotify, position);
            }
        }

        public void Remove(bool withNotify)
        {
            IItemConfig item = ItemConfig;
            if (item != null)
            {
                item.Remove(Amount, withNotify);
            }
        }

    }

    public static class ItemDataExtension
    {
        public static List<ItemData> AddItem(this List<ItemData> LstItems, ItemData item)
        {
            for (int i = 0; i < LstItems.Count; i++)
            {
                if (LstItems[i].Id == item.Id)
                {
                    LstItems[i].Amount += item.Amount;
                    return LstItems;
                }
            }
            LstItems.Add(item);
            return LstItems;
        }
        public static List<ItemData> AddRangeItem(this List<ItemData> LstItems, List<ItemData> LstNewItems)
        {
            for (int i = 0; i < LstNewItems.Count; i++)
            {
                LstItems = LstItems.AddItem(LstNewItems[i]);
            }
            return LstItems;
        }
    }
}