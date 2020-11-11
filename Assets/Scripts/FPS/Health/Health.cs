using System.Collections;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public int health = 100;
    public Collider hitCollider;

    [Header("Invincibility settings")]
    public float respawnTime = 3f;
    public float invincibilityDurationSeconds;
    public float invincibilityDeltaTIme;
    public GameObject rendererPart;

    protected int CurrentHealth { get; set; }
    
    protected bool inInvincible = false;
    protected WeaponManager m_weaponManager;

    private void Awake()
    {
        CurrentHealth = health;
        m_weaponManager = GetComponent<WeaponManager>();
    }

    public abstract void GotDamage(GameObject from, int damage);
    protected abstract void Death();

    protected virtual void Respawn()
    {
        CurrentHealth = health;

        var newPos = SpawnPointManager.Instance.GetRandomSpawnPoint(true);
        transform.position = newPos;
        hitCollider.enabled = true;

        StartCoroutine(BecomeInvincible());
    }

    protected IEnumerator BecomeInvincible()
    {
        inInvincible = true;

        for (float i = 0; i < invincibilityDurationSeconds; i += invincibilityDeltaTIme)
        {
            // Alternate between 0 and 1 scale to simulate flashing
            if (rendererPart.transform.localScale == Vector3.one)
            {
                ScaleModelTo(Vector3.zero);
            }
            else
            {
                ScaleModelTo(Vector3.one);
            }
            yield return new WaitForSeconds(invincibilityDeltaTIme);
        }

        ScaleModelTo(Vector3.one);
        inInvincible = false;
    }

    protected void ScaleModelTo(Vector3 scale)
    {
        rendererPart.transform.localScale = scale;
    }
}
