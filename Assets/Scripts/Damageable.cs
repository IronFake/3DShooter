using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public int health = 100;

    public abstract void GotDamage(int damage);
}
