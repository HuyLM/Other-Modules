using System;
using UnityEngine;

namespace OtherModules.Inventory.Editor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ItemFieldAttribute : PropertyAttribute
    {

    }
}