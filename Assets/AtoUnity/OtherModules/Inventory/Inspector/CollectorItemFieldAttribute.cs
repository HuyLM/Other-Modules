using System;
using UnityEngine;

namespace AtoGame.OtherModules.Inventory
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]

    public class CollectorItemFieldAttribute : PropertyAttribute
    {
        [NonSerialized]public string[] nameCollector;
        public CollectorItemFieldAttribute(params string[] nameCollector)
        {
            this.nameCollector = nameCollector;
        }
    }
}
