using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Kawaii_Survivor.Scripts.Managers;
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

    public Weapon[] GetWeapons()
    {
        List<Weapon> weapons = new List<Weapon>();
        foreach (var weaponPosition in weaponPositions)
        {
            if (weaponPosition.Weapon == null)
            {
                weapons.Add(null);
                continue;
            }

            weapons.Add(weaponPosition.Weapon);
        }
        return weapons.ToArray();
    }

    internal void RecycleWeapon(int index)
    {
        int i = 0;
        foreach (var weaponPosition in weaponPositions)
        {
            if (i == index)
            {
                int recyclePrice = weaponPositions[i].Weapon.GetRecyclePrice();
                CurrencyManager.instance.AddCurrency(recyclePrice);
                weaponPosition.RecycleWeapon();
                break;
            }
            i++;
        }
    }
}