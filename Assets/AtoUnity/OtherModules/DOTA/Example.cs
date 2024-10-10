using Sirenix.OdinInspector;
using UnityEngine;

public class Example : MonoBehaviour {
    // Define an enum
    public enum WeaponType {
        Sword,
        Bow,
        Staff,
        Dagger
    }

    // Apply the EnumToggleButtons attribute
    [EnumToggleButtons]
    public WeaponType selectedWeapon;

    // Show a property or method based on the selected enum
    [ShowIf("selectedWeapon", WeaponType.Sword)]
    public int swordDamage = 100;

    [ShowIf("selectedWeapon", WeaponType.Bow)]
    public int bowRange = 50;

    [ShowIf("selectedWeapon", WeaponType.Staff)]
    public int staffMagicPower = 120;

    [ShowIf("selectedWeapon", WeaponType.Dagger)]
    public int daggerSpeed = 80;

    [Button]
    public void Attack()
    {
        Debug.Log("Attacking with: " + selectedWeapon);
    }
}

 // bo Values
 // gan Tween
 // OnCompleteEvent
 // bo UnityEvent ?
 // test play runtime
 // them Paralle/Sequence
 // 
