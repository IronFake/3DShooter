using UnityEngine;

[CreateAssetMenu(menuName = "Weapon menu")]
public class WeaponStats : ScriptableObject
{
    public int weaponId;
    public int damage;
    public float fireRate;
    public float reloadTime;
    public int clipSize;
    public int numberOfClips;
}
