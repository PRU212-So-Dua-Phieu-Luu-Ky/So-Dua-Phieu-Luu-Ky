using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instance;
    [Header("Elements")]
    [SerializeField] private PlayerWeapons playerWeapons;
    [Header("Settings")]
    [SerializeField] private List<Weapon> weaponsToMerge = new List<Weapon>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Actions")]
    public static Action<Weapon> onMerge;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public bool CanMerge(Weapon weapon)
    {
        if (weapon.Level >= 3) return false;

        weaponsToMerge.Clear();
        weaponsToMerge.Add(weapon);

        Weapon[] weapons = playerWeapons.GetWeapons();
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null) continue;

            if (weapon == weapons[i]) continue;

            if (weapons[i].WeaponData.Name != weapon.WeaponData.Name) continue;

            if (weapons[i].Level != weapon.Level) continue;

            weaponsToMerge.Add(weapons[i]);
            return true;
        }
        return false;
    }

    public void Merge()
    {
        if (weaponsToMerge.Count < 2)
        {
            Debug.Log("Not enough weapons to merge");
            return;
        }
        DestroyImmediate(weaponsToMerge[1].gameObject);
        weaponsToMerge[0].Upgrade();

        Weapon weapon = weaponsToMerge[0];
        weaponsToMerge.Clear();

        onMerge?.Invoke(weapon);

        foreach (Weapon w in weaponsToMerge)
        {
            Debug.Log("Merge: " + w.WeaponData.Name);
        }
    }

}
