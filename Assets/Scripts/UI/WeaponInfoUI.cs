using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance { get; private set; }

    public Text weaponClipContent;
    public Text weaponAmmoRemaining;

    private void Awake()
    {
        Instance = this;   
    }

    public void UpdateClipInfo(int value)
    {
        weaponClipContent.text = value.ToString();
    }

    public void UpdateAmmoRemaining(int value)
    {
        weaponAmmoRemaining.text = value.ToString();
    }
}
