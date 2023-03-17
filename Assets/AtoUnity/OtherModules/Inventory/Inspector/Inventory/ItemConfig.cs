using Ftech.Lib.Common.UnityInspector.Editor;
using Ftech.RacingCar2;
using Newtonsoft.Json.Linq;
using OtherModules.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using static OtherModules.Inventory.ItemDatabase;

namespace OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Data/Item/ItemConfig")]
    public class ItemConfig : ScriptableObject, IItemConfig
    {
        [Header("[Item]")]
        [SerializeField, SpriteField] private Sprite icon;
        [SerializeField, SpriteField] private Sprite sprite;
#if UNITY_EDITOR
        [SerializeField] public bool lockID;
#endif
        [SerializeField, Range(1, 999999), DisableIf("lockID")] private int id = 1;
        [SerializeField] private string displayName;
        [SerializeField] private string localizeKey;
        [SerializeField, TextArea(3, 5)] private string description;
        [SerializeField] private ItemData sellPrice;

        public int Id
        {
            get => id;
#if UNITY_EDITOR
            set
            {
                if (lockID)
                {
                    Debug.LogError("Can't change id. " + name + " is locked");
                    return;
                }
                id = value;
            }
#endif
        }
        public virtual string Name
        {
            get => displayName;
            set => displayName = value;
        }
        public virtual string LocalizeKey
        {
            get => localizeKey;
            set => localizeKey = value;
        }
        public virtual Sprite Icon
        {
            get => icon;
            set => icon = value;
        }
        public virtual Sprite Sprite => sprite;
        public string Description
        {
            get => description;
            set => description = value;
        }
        public ItemData SellPrice
        {
            get => sellPrice;
            set => sellPrice = value;
        }

        public virtual long ItemType { get; set; }

        public long GetItemType()
        {
            return ItemType;
        }
        public virtual IItemData[] Claim(long amount, bool withNotify, string position)
        {
            //Debug.Log($"Add {amount} item {Id} in inventory");
            //SaveDataManager.Instance.Inventory.Add(Id, amount, withNotify ? Common.NotifyType.InventoryChanged : Common.NotifyType.Nothing, false);
            SaveDataManager.Instance.Inventory.AddWSCart(Id, amount);
            return new ItemData[] { new ItemData(Id, amount) };
        }

        public virtual void Remove(long amount, bool withNotify)
        {
            SaveDataManager.Instance.Inventory.RemoveWSCart(Id, amount);
            //SaveDataManager.Instance.Inventory.Remove(Id, amount, withNotify ? Common.NotifyType.InventoryChanged : Common.NotifyType.Nothing, false);
        }

        public virtual long GetAvaliable()
        {
            return SaveDataManager.Instance.Inventory.GetItem(Id).Amount;
        }

#if UNITY_EDITOR
        public virtual JObject GetJsonConfig()
        {
            return null;
        }


#endif
        public virtual void GetConfig(JToken item, int itemType)
        {
            int id = item.Value<int>("id");
            string nameDisplay = item.Value<string>("nameDisplay");
            string keyLocalize = item.Value<string>("keyLocalize");
            long sellCash = item.Value<long>("sellCash");
            long sellDiamond = item.Value<long>("sellDiamond");

            Name = nameDisplay;
            LocalizeKey = keyLocalize;
            if (sellCash > 0)
            {
                SellPrice = new ItemData(ConstantItemID.CASH_ITEM_ID, sellCash);
            }
            else
            {
                SellPrice = new ItemData(ConstantItemID.DIAMOND_ITEM_ID, sellDiamond);
            }
            ItemType = itemType;
        }
    }

}