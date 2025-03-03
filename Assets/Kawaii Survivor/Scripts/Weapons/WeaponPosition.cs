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

    public void AssignWeapon(Weapon weapon)
    {
        weapon = Instantiate(weapon, transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }
}
