using Ftech.Lib.Common;
using UnityEngine;


namespace OtherModules.Inventory
{
    public interface IItemConfig : IEventParams
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        Sprite Sprite { get; }
        public ItemData SellPrice { get; set; }
        IItemData[] Claim(long amount, bool withNotify, string position);
        void Remove(long amount, bool withNotify);

        long GetAvaliable();
        void GetConfig(Newtonsoft.Json.Linq.JToken item, int itemType);

        long GetItemType();
    }

    public interface IItemData : IEventParams
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        Sprite Icon { get; }
        IItemConfig ItemConfig { get; }
        long Amount { get; }
        bool IsEmpty { get; }
    }
}