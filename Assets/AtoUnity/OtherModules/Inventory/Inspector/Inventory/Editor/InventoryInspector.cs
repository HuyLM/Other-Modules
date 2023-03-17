using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace OtherModules.Inventory
{

    [CustomEditor(typeof(ItemInventoryScriptable))]
    public class InventoryInspector : UnityEditor.Editor
    {
        private ItemInventoryScriptable inventory;
        private Vector2 scroll;
        private Vector2 scrollInfinite;
        private int id;
        private int amount;
        private int infId;
        GUIStyle myStyle;

        private void Awake()
        {
            inventory = target as ItemInventoryScriptable;
            myStyle = new GUIStyle();

            myStyle.fontSize = 16;
            myStyle.alignment = TextAnchor.MiddleCenter;
            myStyle.padding.top = 5;
            myStyle.padding.left = -3;
            myStyle.fontStyle = FontStyle.Bold;
        }

        public override void OnInspectorGUI()
        {
            if (!inventory)
            {
                return;
            }
            EditorGUILayout.LabelField("Items", myStyle);
            EditorGUILayout.Space(30);
            scroll = EditorGUILayout.BeginScrollView(scroll, true, true);
            foreach (ItemData item in inventory.ItemInventory.GetAllItem())
            {
                EditorGUILayout.BeginVertical("Box");
                GUIContent content = new GUIContent(item.Icon?.texture, $"{item.Name}\n{item.Description}");
                GUILayout.Box(content, GUILayout.Width(64), GUILayout.Height(64));
                GUILayout.Label($"x{item.Amount}", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(64));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            GUIContent[] contents = new GUIContent[ItemDatabase.GetCount() + 1];
            contents[0] = new GUIContent("None", "None");
            int[] optionsValue = new int[ItemDatabase.GetCount() + 1];
            optionsValue[0] = ItemDatabase.NoneId;

            int index = 1;
            foreach (var itemType in ItemDatabase.GetAllItem())
            {
                string type = itemType.NameType;
                string name = $"{itemType.Item.Name} (ID: {itemType.Item.Id})";
                contents[index] = new GUIContent(type + "/" + name);
                optionsValue[index] = itemType.Item.Id;
                index++;
            }

            id = EditorGUILayout.IntPopup(id, contents, optionsValue);
            amount = EditorGUILayout.IntField(amount);

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add"))
            {
                inventory.ItemInventory.Add(this.id, amount, Common.NotifyType.Nothing, false);
            }
            if (GUILayout.Button("Remove"))
            {
                inventory.ItemInventory.Remove(this.id, amount, Common.NotifyType.Nothing, false);
            }
            /////////////////////////////////
            EditorGUILayout.Space(50);
            EditorGUILayout.LabelField("Infinite Items", myStyle);
            EditorGUILayout.Space(30);

            scrollInfinite = EditorGUILayout.BeginScrollView(scrollInfinite, true, true);
            foreach (int item in inventory.ItemInventory.InfiniteItemIds)
            {
                EditorGUILayout.BeginVertical("Box");
                ItemData itemSlot = new ItemData(item, 0);
                GUIContent content = new GUIContent(itemSlot.Icon?.texture, $"{itemSlot.Name}\n{itemSlot.Description}");
                GUILayout.Box(content, GUILayout.Width(64), GUILayout.Height(64));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();

            GUIContent[] infContents = new GUIContent[ItemDatabase.GetCount() + 1];
            infContents[0] = new GUIContent("None", "None");
            int[] infOptionsValue = new int[ItemDatabase.GetCount() + 1];
            infOptionsValue[0] = ItemDatabase.NoneId;

            int infIndex = 1;
            foreach (var itemType in ItemDatabase.GetAllItem())
            {
                string type = itemType.NameType;
                string name = $"{itemType.Item.Name} (ID: {itemType.Item.Id})";
                infContents[infIndex] = new GUIContent(type + "/" + name);
                infOptionsValue[infIndex] = itemType.Item.Id;
                infIndex++;
            }

            infId = EditorGUILayout.IntPopup(infId, infContents, infOptionsValue);

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add Infinite"))
            {
                inventory.ItemInventory.AddInfiniteItem(this.infId);
            }
            if (GUILayout.Button("Remove Infinite"))
            {
                inventory.ItemInventory.RemoveInfiniteItem(this.infId);
            }
        }
    }
}