using OtherModules.Inventory;
using OtherModules.Inventory.Editor;
using UnityEditor;
using UnityEngine;

namespace Ftech.RacingCar2.Editor
{
    [CustomPropertyDrawer(typeof(CollectorItemFieldAttribute))]

    public class CollectorItemFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool isInteger = property.propertyType == SerializedPropertyType.Integer;

            if (isInteger)
            {
                label = EditorGUI.BeginProperty(position, label, property);
                EditorGUI.indentLevel = 0;
                Rect contentPosition = EditorGUI.PrefixLabel(position, label);


                EditorGUI.BeginChangeCheck();

                int selectedValue = property.intValue;

                CollectorItemFieldAttribute spriteFieldAttribute = (CollectorItemFieldAttribute)attribute;
                string nameCollector = spriteFieldAttribute.nameCollector;
                int numberItem = 0;
                foreach (var itemType in ItemDatabase.GetAllItem(nameCollector))
                {
                    numberItem++;
                }

                GUIContent[] contents = new GUIContent[numberItem + 1];
                contents[0] = new GUIContent("None", "None");
                int[] optionsValue = new int[numberItem + 1];
                optionsValue[0] = ItemDatabase.NoneId;

                int index = 1;
                foreach (var itemType in ItemDatabase.GetAllItem(nameCollector))
                {
                    int startIndex = itemType.NameType.IndexOf(nameCollector + "/");
                    string type = itemType.NameType.Remove(0, nameCollector.Length + 1 + startIndex);
                    string name = $"{itemType.Item.Name} (ID: {itemType.Item.Id})";
                    contents[index] = new GUIContent(type + name);
                    optionsValue[index] = itemType.Item.Id;
                    index++;
                }


                Rect popupRect = contentPosition;
                popupRect.Set(popupRect.x, popupRect.y, popupRect.width * 0.8f, popupRect.height);
                selectedValue = EditorGUI.IntPopup(popupRect, selectedValue, contents, optionsValue);

                Rect fieldRect = contentPosition;
                fieldRect.Set(fieldRect.x + fieldRect.width * 0.8f, fieldRect.y, fieldRect.width * 0.2f, fieldRect.height);
                selectedValue = EditorGUI.DelayedIntField(fieldRect, selectedValue);

                //EditorGUI.IntField(position, selectedValue);

                if (EditorGUI.EndChangeCheck())
                {
                    property.intValue = selectedValue;
                }
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
