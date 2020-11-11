using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponPosition;

    public bool ownerIsPlayer;
    public Camera weaponCamera;
    public Camera mainCamera;

    [Header("Starting weapons")]
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

    public int CurrentWeaponIndex { get; private set; }
    public bool OwnerIsPlayer { get; private set; }

    private Weapon[] m_availableWeapons = new Weapon[2];
    private InputHandler m_input;
    private PlayerHealth m_health;

    private void Awake()
    {
        m_input = InputHandler.Instance;
        OwnerIsPlayer = ownerIsPlayer;

        if (OwnerIsPlayer)
            m_health = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        PickupWeapon(primaryWeapon);
        PickupWeapon(secondaryWeapon);

        CurrentWeaponIndex = -1;
        ChangeWeapon(0);
    }

    private void Update()
    {
        if (OwnerIsPlayer && m_health.IsAlive && !GameSystem.Instance.IsPause)
        {
            Shoot(m_input.ToggleFire);

            if (m_input.ToggleReload)
            {
                Reload();
            }

            if (m_input.ScrollWheelMovement < 0)
            {
                ChangeWeapon(CurrentWeaponIndex - 1);
            }

            if (m_input.ScrollWheelMovement > 0)
            {
                ChangeWeapon(CurrentWeaponIndex + 1);
            }

            //Key input to change weapon
            if (m_input.ToggleChangeWeapon)
            {
                ChangeWeapon(m_input.NumberOfKey);
            }
        } 
    }

    public void PickupWeapon(Weapon prefab)
    {
        Weapon w = Instantiate(prefab, weaponPosition, false);
        w.name = prefab.name;
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
        w.gameObject.SetActive(false);
        w.PickedUp(this);

        //Need for correct display weapon in enemy prefab
        if (!OwnerIsPlayer)
            w.gameObject.layer = LayerMask.NameToLayer("Enemy");

        switch (prefab.weaponClass)
        {
            case Weapon.WeaponClass.Primary:
                m_availableWeapons[0] = w;
                break;
            case Weapon.WeaponClass.Secondary:
                m_availableWeapons[1] = w;
                break;
            case Weapon.WeaponClass.Grenade:
                m_availableWeapons[2] = w;
                break;
        }
    }

    public void ChangeWeapon(int number)
    {
        if (number > m_availableWeapons.Length - 1)
            return;

        if (CurrentWeaponIndex != -1)
        {
            m_availableWeapons[CurrentWeaponIndex].PutAway();
            m_availableWeapons[CurrentWeaponIndex].gameObject.SetActive(false);
        }

        CurrentWeaponIndex = number;

        if (CurrentWeaponIndex < 0)
            CurrentWeaponIndex = m_availableWeapons.Length - 1;
        else if (CurrentWeaponIndex >= m_availableWeapons.Length)
            CurrentWeaponIndex = 0;

        m_availableWeapons[CurrentWeaponIndex].gameObject.SetActive(true);
        m_availableWeapons[CurrentWeaponIndex].Selected();
    }

    public void Shoot(bool state)
    {
        Weapon weapon = m_availableWeapons[CurrentWeaponIndex];
        if (weapon != null)
        {
            m_availableWeapons[CurrentWeaponIndex].triggerDown = state;
        }
    }

    public void Reload()
    {
        m_availableWeapons[CurrentWeaponIndex].Reload();
    }

    public void DisableAllWeapons()
    {
        foreach (var weapon in m_availableWeapons)
        {
            weapon.ResetWeapon();
            weapon.PutAway();
            weapon.gameObject.SetActive(false);
        }
    }

    public void EnableWeapon()
    {
        m_availableWeapons[CurrentWeaponIndex].gameObject.SetActive(true);
        m_availableWeapons[CurrentWeaponIndex].Selected();
    }
}
