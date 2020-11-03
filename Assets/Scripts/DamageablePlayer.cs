using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePlayer : Damageable
{

    private int maxValue;

    private void Start()
    {
        maxValue = health;
        HealthInfoUI.Instance.UpdateHealthValue(health);
    }

    public override void GotDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Debug.Log("Player dies");
        }

        health = Mathf.Clamp(health, 0, maxValue);
        HealthInfoUI.Instance.UpdateHealthValue(health);
    }
}
