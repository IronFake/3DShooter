using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponPosition;

    [Header("Starting weapons")]
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    private Weapon[] availableWeapons = new Weapon[2];

    public int currentWeaponIndex { get; private set; }

    private bool ownerIsPlayer;

    private void Start()
    {
        ownerIsPlayer = gameObject.tag == "Player" ? true : false;

        PickupWeapon(primaryWeapon);
        PickupWeapon(secondaryWeapon);

        currentWeaponIndex = -1;
        ChangeWeapon(0);
    }

    public void PickupWeapon(Weapon prefab)
    {
        Weapon w = Instantiate(prefab, weaponPosition, false);
        w.name = prefab.name;
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
        w.gameObject.SetActive(false);
        w.PickedUp(gameObject.GetComponent<Actor>());

        if (!ownerIsPlayer)
            w.gameObject.layer = LayerMask.NameToLayer("Enemy");

        switch (prefab.weaponClass)
        {
            case Weapon.WeaponClass.Primary:
                availableWeapons[0] = w;
                break;
            case Weapon.WeaponClass.Secondary:
                availableWeapons[1] = w;
                break;
            case Weapon.WeaponClass.Grenade:
                availableWeapons[2] = w;
                break;
        }
    }

    public void ChangeWeapon(int number)
    {
        if (number > availableWeapons.Length)
            return;

        if (currentWeaponIndex != -1)
        {
            availableWeapons[currentWeaponIndex].PutAway();
            availableWeapons[currentWeaponIndex].gameObject.SetActive(false);
        }

        currentWeaponIndex = number;

        if (currentWeaponIndex < 0)
            currentWeaponIndex = availableWeapons.Length - 1;
        else if (currentWeaponIndex >= availableWeapons.Length)
            currentWeaponIndex = 0;

        availableWeapons[currentWeaponIndex].gameObject.SetActive(true);
        availableWeapons[currentWeaponIndex].Selected();
    }

    public void Shoot(bool state)
    {
        Weapon weapon = availableWeapons[currentWeaponIndex];
        if (weapon != null)
        {
            if(weapon.clipContent == 0)
            {
                weapon.Reload();
            }
            availableWeapons[currentWeaponIndex].triggerDown = state;
        }
    }

    public void Reload()
    {
        availableWeapons[currentWeaponIndex].Reload();
    }


}
