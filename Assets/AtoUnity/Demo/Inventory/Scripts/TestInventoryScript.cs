using UnityEngine;

namespace AtoGame.OtherModules.Inventory.Demo
{
    public class TestInventoryScript : MonoBehaviour
    {
        [SerializeField, CollectorItemField("test" , "test 2", "test 3")] private int id;
        [SerializeField, ItemField()] private int id1;
    }
}
