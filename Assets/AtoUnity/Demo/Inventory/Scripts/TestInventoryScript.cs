using UnityEngine;

namespace AtoGame.OtherModules.Inventory.Demo
{
    public class TestInventoryScript : MonoBehaviour
    {
        [SerializeField, ItemField()] private int id1;
        [SerializeField, ItemField("collector 1")] private int id2;
        [SerializeField, ItemField("collector 1", "collector 2")] private int id3;
        [SerializeField, ItemField(true, "collector 1", "collector 2")] private int id4;
    }
}
