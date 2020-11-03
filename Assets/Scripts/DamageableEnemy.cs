using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEnemy : Damageable
{
    public override void GotDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Debug.Log("Enemy dies");
        }
    }
}
