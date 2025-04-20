using System;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [Header(" Elements ")]
    public Weapon Weapon { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignWeapon(Weapon weapon, int weaponLevel)
    {
        // Set the location of the weapon at 6 positions
        Weapon = Instantiate(weapon, transform);
        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;

        Weapon.UpgradeTo(weaponLevel);
    }

    internal void RecycleWeapon()
    {
        Destroy(Weapon.gameObject);
        Weapon = null;
    }
}
