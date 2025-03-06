using UnityEngine;


public class PlayerWeapons : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private WeaponPosition[] weaponPositions;

    public bool TryAddWeapon(WeaponDataSO weaponData, int weaponLevel)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null) continue;

            weaponPositions[i].AssignWeapon(weaponData.Prefab, weaponLevel);
            return true;
        }
        return false;
    }
}