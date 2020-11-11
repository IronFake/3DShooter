using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{  
    public ParticleSystem deathParticle;
    public EnemyAi enemyAi;

    private void Start()
    {
        enemyAi = GetComponent<EnemyAi>();
    }

    public override void GotDamage(GameObject from, int damage)
    {
        if (inInvincible)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            GameSystem.Instance.UpdatePoints(from, gameObject);
            Death();
        }
    }

    protected override void Respawn()
    {
        base.Respawn();
        rendererPart.SetActive(true);
        m_weaponManager.EnableWeapon();
    }

    protected override void Death()
    {
        m_weaponManager.DisableAllWeapons();
        rendererPart.SetActive(false);
        hitCollider.enabled = false;      
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        StartCoroutine(RespawnCountdown());
    }

    private IEnumerator RespawnCountdown()
    {
        yield return new WaitForSeconds(3);
        Respawn();

    }
}

