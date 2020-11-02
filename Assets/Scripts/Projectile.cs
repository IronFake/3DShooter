using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    static Collider[] s_SphereCastPool = new Collider[32];

    public bool destroyedOnHit = true;
    public float timeToDestroyed = 4.0f;
    public float reachRadius = 5.0f;
    public float damage = 10.0f;
    public AudioClip destroyedSound;

    public GameObject prefabOnDestruction;

    private Weapon m_Owner;
    private Rigidbody m_Rigidbody;
    private float m_TimeSinceLaunch;

    private void Awake()
    {
        PoolSystem.Instance.InitPool(prefabOnDestruction, 4);
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Weapon launcher, Vector3 direction, float force)
    {
        m_Owner = launcher;

        //transform.position = launcher.GetCorrectMuzzlePlace();
        //transform.forward = launcher.EndPoint.forward;
    }
}
