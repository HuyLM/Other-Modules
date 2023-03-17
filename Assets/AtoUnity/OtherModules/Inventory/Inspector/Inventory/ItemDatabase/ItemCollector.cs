using Ftech.Utilities;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OtherModules.Inventory
{
    [CreateAssetMenu(fileName = "ItemCollector", menuName = "Data/Item/ItemDatabase/ItemCollector")]
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

        [FolderPath, BoxGroup("Export Json", centerLabel: true)] public string path;
        [BoxGroup("Export Json"), Button("Export Json Config", ButtonSizes.Large)]
        public void ExportJsonConfig()
        {
            JArray Jobj = new JArray();
            for (int i = 0; i < Items.Count; i++)
            {
                Jobj.Add(Items[i].GetJsonConfig());
                //Jobj[Items[i].Id.ToString()] = Items[i].GetJsonConfig();
            }
            SaveFile(Jobj.ToString());
        }

        private void SaveFile(string data)
        {
            string filePath = path + "/" + nameCollector + ".json";
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            File.WriteAllText(filePath, data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Export Done!");
        }

#endif
    }
}