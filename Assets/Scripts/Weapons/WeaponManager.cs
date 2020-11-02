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

    private int m_CurrentWeapon;

    private void Start()
    {

        PickupWeapon(primaryWeapon);
        PickupWeapon(secondaryWeapon);

        m_CurrentWeapon = -1;
        ChangeWeapon(0);
    }

    private void Update()
    {
        if (availableWeapons[m_CurrentWeapon] != null)
        {
            availableWeapons[m_CurrentWeapon].triggerDown = Input.GetMouseButton(0);
        }
        
        if (Input.GetButton("Reload"))
            availableWeapons[m_CurrentWeapon].Reload();

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ChangeWeapon(m_CurrentWeapon - 1);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ChangeWeapon(m_CurrentWeapon + 1);
        }

        //Key input to change weapon
        for (int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int num;
                if (i == 0)
                    num = 10;
                else
                    num = i - 1;

                if (num < availableWeapons.Length)
                {
                    ChangeWeapon(num);
                }
            }
        }
    }

    private void PickupWeapon(Weapon prefab)
    {
        Weapon w = Instantiate(prefab, weaponPosition, false);
        w.name = prefab.name;
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
        w.gameObject.SetActive(false);
        w.PickedUp(gameObject.GetComponent<FPSController>());

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
        if(m_CurrentWeapon != -1)
        {
            availableWeapons[m_CurrentWeapon].PutAway();
            availableWeapons[m_CurrentWeapon].gameObject.SetActive(false);
        }

        m_CurrentWeapon = number;

        if (m_CurrentWeapon < 0)
            m_CurrentWeapon = availableWeapons.Length - 1;
        else if (m_CurrentWeapon >= availableWeapons.Length)
            m_CurrentWeapon = 0;

        availableWeapons[m_CurrentWeapon].gameObject.SetActive(true);
        availableWeapons[m_CurrentWeapon].Selected();
    }

}
