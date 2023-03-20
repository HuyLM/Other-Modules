using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    public class ItemInventory
    {
        private readonly Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();
        private List<ItemData> items = new List<ItemData>();
        private List<int> iii = new List<int>();

        private bool isDirty;

        public List<ItemData> Items { get => items; }
        public List<int> InfiniteItemIds { get => iii; }

        public void OnInitialize()
        {
            if (items == null)
            {
                return;
            }
            foreach (ItemData item in items)
            {
                if (ItemInventoryController.Instance.ItemDatabase.Constains(item.Id))
                {
                    long amount = item.Amount;
                    if (itemDictionary.ContainsKey(item.Id))
                    {
                        itemDictionary[item.Id].Stack(amount);
                    }
                    else
                    {
                        itemDictionary.Add(item.Id, new ItemData(item.Id, amount));
                    }
                }
            }
        }

        public void Add(params ItemData[] items)
        {
            for(int i =0; i< items.Length; ++i)
            {
                Add(items[i].Id, items[i].Amount);
            }
        }

        public virtual void Add(int id, long amount)
        {
            if (ItemInventoryController.Instance.ItemDatabase.Constains(id))
            {
                if (itemDictionary.ContainsKey(id))
                {
                    itemDictionary[id].Stack(amount);
                    isDirty = true;
                }
                else
                {
                    itemDictionary.Add(id, new ItemData(id, amount));
                    isDirty = true;
                }
            }
        }

        public virtual void Remove(params ItemData[] items)
        {
            foreach (ItemData item in items)
            {
                Remove(item.Id, item.Amount);
            }
        }

        public virtual void Remove(int id, long amount)
        {
            if (itemDictionary.ContainsKey(id))
            {
                itemDictionary[id].Destack(amount);
                isDirty = true;
            }
        }

        private bool IsInfinite(int id)
        {
            if (InfiniteItemIds != null)
            {
                return InfiniteItemIds.Contains(id);
            }
            return false;
        }

        public virtual ItemData GetItem(int id)
        {
            if (IsInfinite(id))
            {
                return new ItemData(id, int.MaxValue);
            }
            if (itemDictionary.TryGetValue(id, out ItemData item))
            {
                return item;
            }
            return new ItemData(id, 0);
        }

        public void AddInfiniteItem(int id)
        {
            if (itemDictionary.ContainsKey(id))
            {
                itemDictionary.Remove(id);
                isDirty = true;
            }
            if (!InfiniteItemIds.Contains(id))
            {
                InfiniteItemIds.Add(id);
                isDirty = true;
            }
        }

        public void RemoveInfiniteItem(int id)
        {
            if (InfiniteItemIds != null)
            {
                if (InfiniteItemIds.Contains(id))
                {
                    InfiniteItemIds.Remove(id);
                    isDirty = true;
                }
            }
        }

        public void PushEvent(NotifyType notifyType)
        {
            if (notifyType == NotifyType.InventoryChanged)
            {
                AtoGame.Base.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
            }
        }

        public void Load(List<ItemData> items, List<int> infiniteItemIds)
        {
            this.items = items;
            this.iii = infiniteItemIds;
            isDirty = false;
        }


    }

    public enum NotifyType
    {
        Nothing, InventoryChanged
    }
}
