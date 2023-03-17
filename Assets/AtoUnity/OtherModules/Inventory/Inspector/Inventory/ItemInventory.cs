using Ftech.Lib;
using Ftech.Lib.Common;
using Ftech.Lib.Common.UnityInspector.Editor;
using Ftech.Lib.Helper;
using Ftech.RacingCar;
using Ftech.RacingCar2;
using Ftech.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OtherModules.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace OtherModules.Inventory
{
    // [CreateAssetMenu(fileName = "NewInventory", menuName = "Data/Item/ItemInventory")]
    public class ItemInventory : ISaveDatable, IInitUserData
    {
        [JsonIgnore] private readonly Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();

        [JsonIgnore] private List<ItemData> items = new List<ItemData>();
        [JsonIgnore] private List<int> iii = new List<int>();



        [JsonIgnore] private List<ItemData> cart = new List<ItemData>();
        [JsonIgnore] private bool usingCart;

        [JsonIgnore] private bool isDirty;
        [JsonIgnore] private BlueprintBuildData blueprintBuildData;

        [JsonIgnore] public List<int> InfiniteItemIds { get => iii; }
        [JsonIgnore] public BlueprintBuildData BlueprintBuildData { get => blueprintBuildData; }
        public IEnumerable<ItemData> GetAllItem()
        {
            return itemDictionary.Values;
        }

        public virtual void Add(NotifyType notifyType, bool mustSave, params ItemData[] items)
        {
            if (items == null)
            {
                return;
            }
            if (notifyType == NotifyType.InventoryChanged)
            {
                Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
            }
            if (mustSave)
            {
                Save();
            }
        }

        public virtual void Add(int id, long amount, NotifyType notifyType, bool mustSave)
        {
            if (ItemDatabase.Constains(id))
            {
                if (usingCart) // add to cart
                {
                    if (notifyType == NotifyType.Nothing)
                    {
                        AddCart(id, amount);
                    }
                }
                long preAmount = 0;
                if (itemDictionary.ContainsKey(id))
                {
                    preAmount = itemDictionary[id].Amount;
                    itemDictionary[id].Stack(amount);
                    isDirty = true;
                }
                else
                {
                    itemDictionary.Add(id, new ItemData(id, amount));
                    isDirty = true;
                }
                if (notifyType == NotifyType.InventoryChanged)
                {
                    Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
                }

                if (mustSave == true)
                {
                    Save();
                }
            }
        }
        public virtual void SetAmount(int id, long amount, NotifyType notifyType = NotifyType.Nothing)
        {
            if (ItemDatabase.Constains(id))
            {
                if (itemDictionary.ContainsKey(id))
                {
                    itemDictionary[id].Amount = amount;
                    isDirty = true;
                }
                else
                {
                    itemDictionary.Add(id, new ItemData(id, amount));
                    isDirty = true;
                }
                if (notifyType == NotifyType.InventoryChanged)
                {
                    Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
                }
            }
        }

        public virtual void Remove(NotifyType notifyType, bool mustSave, params ItemData[] items)
        {
            foreach (ItemData item in items)
            {
                Remove(item.Id, item.Amount, notifyType, false);
            }
            if (notifyType == NotifyType.InventoryChanged)
            {
                Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
            }
            if (mustSave)
            {
                Save();
            }
        }

        public virtual void Remove(int id, long amount, NotifyType notifyType, bool mustSave)
        {
            if (itemDictionary.ContainsKey(id))
            {

                if (usingCart) // remove to cart
                {
                    if (notifyType == NotifyType.Nothing)
                    {
                        RemoveCart(id, amount);
                    }
                }
                long preAmount = itemDictionary[id].Amount;
                itemDictionary[id].Destack(amount);
                isDirty = true;

                if (notifyType == NotifyType.InventoryChanged)
                {
                    Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
                }
            }
            if (mustSave)
            {
                Save();
            }
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
            if (id == ConstantItemID.CASH_ITEM_ID) return new ItemData(id, UserDataManager.Instance.Profile.Cash);
            if (id == ConstantItemID.DIAMOND_ITEM_ID) return new ItemData(id, UserDataManager.Instance.Profile.Diamond);
            return new ItemData(id, 0);
        }

        private bool IsInfinite(int id)
        {
            if (InfiniteItemIds != null)
            {
                return InfiniteItemIds.Contains(id);
            }
            return false;
        }

        public void AddInfiniteItem(int id)
        {
            if (itemDictionary.ContainsKey(id))
            {
                if (!InfiniteItemIds.Contains(id))
                {
                    InfiniteItemIds.Add(id);
                    isDirty = true;
                }
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
                Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
            }
        }

        #region Cart

        public List<ItemData> GetCart(bool isStackCart)
        {
            List<ItemData> result = new List<ItemData>();
            if (isStackCart)
            {
                for (int i = 0; i < cart.Count; ++i)
                {
                    ItemData itemInList = GetItemWithIdInList(result, cart[i].Id);
                    if (itemInList.IsEmpty)
                    {
                        result.Add(new ItemData(cart[i].Id, cart[i].Amount));
                    }
                    else
                    {
                        itemInList.Stack(cart[i].Amount);
                    }
                }
            }
            else
            {
                result = cart.ToList();
            }
            // defalt not stack
            return result;
        }
        public void StartUseCart()
        {
            usingCart = true;
            cart.Clear();
        }

        public void ClearCart()
        {
            if (cart != null)
            {
                cart.Clear();
            }
        }

        public void EndUseCart(NotifyType notifyType)
        {
            if (cart.Count == 0 || usingCart == false)
            {
                return;
            }
            usingCart = false;
            if (notifyType == NotifyType.InventoryChanged)
            {
                Ftech.Lib.Common.EventDispatcher.Instance.Dispatch<EventKey.OnItemInventoryChanged>();
            }
            Save();
        }

        public void AddCart(params ItemData[] items)
        {
            if (items != null)
            {
                for (int i = 0; i < items.Length; ++i)
                {
                    AddCart(items[i].Id, items[i].Amount);
                }
            }
        }

        public void AddCart(int id, long amount)
        {
            cart.Add(new ItemData(id, amount));
        }

        public void RemoveCart(params ItemData[] items)
        {
            if (items != null)
            {
                for (int i = 0; i < items.Length; ++i)
                {
                    RemoveCart(items[i].Id, items[i].Amount);
                }
            }
        }

        public void RemoveCart(int id, long amount)
        {
            cart.Add(new ItemData(id, -1 * amount));
        }

        private ItemData GetItemWithIdInList(List<ItemData> list, int id)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Id == id)
                {
                    return list[i];
                }
            }
            return ItemData.Empty;
        }
        #endregion

        #region OnlineCart

        //public List<ItemData> GetCart(bool isStackCart)
        //{
        //    List<ItemData> result = new List<ItemData>();
        //    if(isStackCart)
        //    {
        //        for(int i = 0; i < cart.Count; ++i)
        //        {
        //            ItemData itemInList = GetItemWithIdInList(result, cart[i].Id);
        //            if(itemInList.IsEmpty)
        //            {
        //                result.Add(new ItemData(cart[i].Id, cart[i].Amount));
        //            }
        //            else
        //            {
        //                itemInList.Stack(cart[i].Amount);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        result = cart.ToList();
        //    }
        //    // defalt not stack
        //    return result;
        //}

        [JsonIgnore] private List<ItemData> wsCart = new List<ItemData>();
        [JsonProperty] private List<ItemData> wsMissCart = new List<ItemData>();
        [JsonIgnore] public bool isSendingCart;

        public bool HaveMissCart()
        {
            return wsMissCart.Count > 0;
        }

        public void ReconnectWSCart(Action OnSuccess = null, Action OnFail = null)
        {
            Debug.Log("ReconnectWSCart");
            isSendingCart = false;
            PopupHUD.Instance.Hide<WaitingPopup>();
            SendWSCart(OnSuccess, OnFail, showErrorPopup: false);
        }
        public void SendWSCart(Action OnSuccess = null, Action OnFail = null, bool autoClaim = true, bool showErrorPopup = true)
        {
            if (isSendingCart)
            {
                if (autoClaim)
                {
                    Debug.Log("Add WS Item fail - Is Sending - Add to WS miss cart");
                    wsMissCart.AddRange(wsCart);
                    for (int i = 0; i < wsCart.Count; i++)
                    {
                        Debug.Log("wsMissCart " + i + " - itemID: " + wsMissCart[i].Id + " - amount: " + wsMissCart[i].Amount);
                    }
                }
                wsCart.Clear();
                return;
            }

            if (wsMissCart.Count > 0)
            {
                for (int i = 0; i < wsMissCart.Count; i++)
                {
                    UpdateWsCart(wsMissCart[i].Id, wsMissCart[i].Amount);
                }
                wsMissCart.Clear();
            }

            if (wsCart.Count == 0) return;

            isSendingCart = true;
            WSInventory.AddItems(wsCart.ToArray(), () =>
            {
                isSendingCart = false;
                OnSuccess?.Invoke();
                wsCart.Clear();
                PopupHUD.Instance.Hide<WaitingPopup>();
            }, (e) =>
            {
                if (autoClaim)
                {
                    Debug.Log("Add WS Item fail - Add to WS miss cart");
                    wsMissCart.AddRange(wsCart);
                    for (int i = 0; i < wsCart.Count; i++)
                    {
                        Debug.Log("wsMissCart " + i + " - itemID: " + wsMissCart[i].Id + " - amount: " + wsMissCart[i].Amount);
                    }
                    Save();
                }
                wsCart.Clear();
                isSendingCart = false;
                PopupHUD.Instance.Hide<WaitingPopup>();
                OnFail?.Invoke();
            }, showErrorPopup);
        }

        public void AddWSCart(int id, long amount)
        {
            if (isSendingCart) return;

            if (usingCart) // add to cart
            {
                AddCart(id, amount);
            }
            UpdateWsCart(id, amount);
        }
        public void RemoveWSCart(int id, long amount)
        {
            if (isSendingCart) return;

            if (usingCart) // add to cart
            {
                RemoveCart(id, amount);
            }
            UpdateWsCart(id, -1 * amount);
        }
        private void UpdateWsCart(int id, long amount)
        {
            var item = GetItemInWSCart(id);
            if (item != null) item.Amount += amount;
            else wsCart.Add(new ItemData(id, amount));
        }
        private ItemData GetItemInWSCart(int id)
        {
            for (int i = 0; i < wsCart.Count; ++i)
            {
                if (wsCart[i].Id == id)
                {
                    return wsCart[i];
                }
            }
            return null;
        }
        #endregion


        #region Save/Load Data
        public void Save()
        {
            if (isDirty == false)
            {
                return;
            }
            if (items == null)
            {
                items = new List<ItemData>();
            }
            items.Clear();
            foreach (var item in itemDictionary.Values)
            {
                items.Add(item);
            }
            string userItemsDataPath = Application.persistentDataPath + Ftech.RacingCar2.UserDataPathConfig.ITEM_INVENTORY_FILE;
            JsonWrapper.SaveJson(this, userItemsDataPath);
            isDirty = false;
        }

        /// <summary>
        /// Call load data from save file
        /// </summary>
        public static ItemInventory Load()
        {
            ItemInventory data;
            string userItemsDataPath = Application.persistentDataPath + Ftech.RacingCar2.UserDataPathConfig.ITEM_INVENTORY_FILE;
            Debug.Log("ITEM_INVENTORY_FILE: " + userItemsDataPath);
            if (File.Exists(userItemsDataPath))
            {
                data = JsonWrapper.LoadJson<ItemInventory>(userItemsDataPath);
            }
            else
            {
                data = new ItemInventory();
                data.Create();
            }
            return data;
        }

        public void Init()
        {
            GetItem(ConstantItemID.CASH_ITEM_ID).Amount = UserDataManager.Instance.Profile.Cash;
            GetItem(ConstantItemID.DIAMOND_ITEM_ID).Amount = UserDataManager.Instance.Profile.Diamond;
        }

        public void Create()
        {

        }

        public void LoadData(Action OnDone, Action<int> OnFail)
        {
            WSInventory.GetAllInventoryConfigs(() =>
            {
                WSInventory.GetAllItemsData(OnDone, OnFail);
            });
        }
        public void InitData(JArray data)
        {
            blueprintBuildData = new BlueprintBuildData();
            blueprintBuildData.ListBlueprintProgress = new List<BlueprintProgress>();
            foreach (var item in data)
            {
                int itemId = item.Value<int>("itemId");
                long amount = item.Value<long>("amount");
                if (ItemDatabase.Constains(itemId))
                {
                    if (itemDictionary.ContainsKey(itemId))
                    {
                        itemDictionary[itemId].Stack(amount);
                        isDirty = true;
                    }
                    else
                    {
                        itemDictionary.Add(itemId, new ItemData(itemId, amount));
                        isDirty = true;
                    }

                    if (item["startTime"] != null)
                    {
                        long startTime = item.Value<long>("startTime");
                        long claimTime = item.Value<long>("claimTime");
                        long skipTime = item.Value<long>("skipTime");
                        int countUseReduce = item.Value<int>("countUseReduce");
                        BlueprintProgress newProgress = new BlueprintProgress();
                        newProgress.Id = itemId;
                        if (startTime > 0)
                        {
                            newProgress.StartTime = UnityHelper.ConvertToDateTime(startTime);
                            newProgress.ClaimTime = UnityHelper.ConvertToDateTime(claimTime);
                            newProgress.SkipTime = TimeSpan.FromSeconds(skipTime);
                            newProgress.UseReduceTimes = countUseReduce;
                            newProgress.IsBuilding = true;


                            DateTime now = GlobalTime.GetTime();
                            DateTime completeTime = newProgress.ClaimTime;
                            DateTime curTime = now.Add(newProgress.SkipTime);
                            newProgress.IsCompleted = curTime.CompareTo(completeTime) >= 0;
                        }
                        else if (startTime == -1)
                        {
                            newProgress.IsBuilding = false;
                            newProgress.IsCompleted = true;
                        }
                        else
                        {
                            newProgress.IsBuilding = false;
                            newProgress.IsCompleted = false;
                        }
                        BlueprintBuildData.ListBlueprintProgress.Add(newProgress);
                    }
                }
            }
        }
        #endregion
    }
}