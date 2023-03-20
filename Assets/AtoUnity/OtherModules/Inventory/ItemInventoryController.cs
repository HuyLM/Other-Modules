using AtoGame.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    public class ItemInventoryController : Singleton<ItemInventoryController>
    {
        private ItemDatabase itemDatabase;
        private ItemInventory itemInventory;
        private IItemInventorySaver inventorySaver;

        private bool isInitialized;
        private Dictionary<string, ItemCart> carts = new Dictionary<string, ItemCart>();

        public ItemDatabase ItemDatabase { get => itemDatabase; }
        public ItemInventory ItemInventory { get => itemInventory; }
        public IItemInventorySaver Saver { get => inventorySaver; }

        protected override void Initialize()
        {
            base.Initialize();
            isInitialized = false;
        }

        public void Init(ItemDatabase itemDatabase, IItemInventorySaver inventorySaver)
        {
            isInitialized = false;
            this.itemDatabase = itemDatabase;
            this.inventorySaver = inventorySaver;

            int doingTaskNumber = 1;

            itemDatabase.OnInitialize();
            if(inventorySaver != null)
            {
                inventorySaver.Init(()=> {
                    inventorySaver.Load((result, itemInventory)=>{
                        if(result == true)
                        {
                            this.itemInventory = itemInventory;
                            itemInventory.OnInitialize();
                        }
                        doingTaskNumber--;
                        if (doingTaskNumber == 0)
                        {
                            isInitialized = true;
                        }
                    });
                });
            }
        }

        public void Add(params ItemData[] items)
        {
            if(isInitialized == false)
            {
                Log($"Can't Add(params) Func, because it is not initialized");
                return;
            }
            ItemInventory.Add(items);
        }

        public void Add(int id, long amount)
        {
            if (isInitialized == false)
            {
                Log($"Can't Add Func, because it is not initialized");
                return;
            }
            ItemInventory.Add(id, amount);
        }

        public void Remove(params ItemData[] items)
        {
            if (isInitialized == false)
            {
                Log($"Can't Remove(params) Func, because it is not initialized");
                return;
            }
            ItemInventory.Remove(items);
        }

        public void Remove(int id, long amount)
        {
            if (isInitialized == false)
            {
                Log($"Can't Remove Func, because it is not initialized");
                return;
            }
            ItemInventory.Remove(id, amount);
        }


        public void Log(string message)
        {
            if(!isInitialized)
            {
                return;
            }
            if(ItemDatabase.EnableLog)
            {
                Debug.Log("[Inventory] " + message);
            }
        }
    }
}
