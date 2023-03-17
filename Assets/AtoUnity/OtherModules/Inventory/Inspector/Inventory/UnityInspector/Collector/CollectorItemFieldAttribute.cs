using System;
using UnityEngine;

namespace OtherModules.Inventory.Editor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]

    public class CollectorItemFieldAttribute : PropertyAttribute
    {
        [NonSerialized]public string nameCollector;
        public CollectorItemFieldAttribute(string nameCollector)
        {
            this.nameCollector = nameCollector;
        }
    }
}
