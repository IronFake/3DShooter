using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    private GameObject m_deathScreen;

    public bool IsAlive { get; private set; }

    private void Start()
    {
        m_deathScreen = GameObject.Find("DeathScreen");
        m_deathScreen.SetActive(false);
        HealthInfoUI.Instance.UpdateHealthValue(CurrentHealth);
        IsAlive = true;
    }

    public override void GotDamage(GameObject from, int damage)
    {
        if (inInvincible)
            return;

        CurrentHealth -= damage;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            GameSystem.Instance.UpdatePoints(from, gameObject);
            Death(); 
        }

        HealthInfoUI.Instance.UpdateHealthValue(CurrentHealth);
    }

    protected override void Respawn()
    {
        base.Respawn();
        m_weaponManager.EnableWeapon();
        m_deathScreen.SetActive(false);
        IsAlive = true;
        HealthInfoUI.Instance.UpdateHealthValue(CurrentHealth);
    }

    protected override void Death()
    {
        m_weaponManager.DisableAllWeapons();
        IsAlive = false;
        hitCollider.enabled = false;
        m_deathScreen.SetActive(true);
        //m_weaponManager.ResetWeaponAmmo();
        StartCoroutine(RespawnCountdown());
    }

    private IEnumerator RespawnCountdown()
    {
        yield return new WaitForSeconds(3);
        Respawn();
    }
}
