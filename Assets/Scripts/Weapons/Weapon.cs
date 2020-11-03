using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Structures
    public enum TriggerType
    {
        Auto,
        Manual
    }

    public enum WeaponClass
    {
        Primary,
        Secondary,
        Grenade
    }

    public enum WeaponType
    {
        Raycast,
        Projectile
    }

    public enum WeaponState
    {
        Idle,
        Firing,
        Reloading
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float spreadAngle = 0.0f;
        public int projectilePerShot = 1;
        public float screenShakeMultiplier = 1.0f;
    }

    class ActiveTrail
    {
        public LineRenderer renderer;
        public Vector3 direction;
        public float remainingTime;
    }
    #endregion

    #region Public fields
    public WeaponStats weaponStats;
    public TriggerType triggerType = TriggerType.Manual;
    public WeaponClass weaponClass = WeaponClass.Primary;
    public WeaponType weaponType = WeaponType.Raycast;
    
    public Projectile projectilePrefab;
    public float projectileLaunchForce = 200.0f;

    public Transform endPoint;
    public bool disabledOnEmpty;
    public AdvancedSettings advancedSettings;

    [Header("Animation Clips")]
    public AnimationClip fireAnimationClip;
    public AnimationClip reloadAnimationClip;

    [Header("Audio Clips")]
    public AudioClip fireAudioClip;
    public AudioClip reloadAudioClip;

    [Header("Visual Settings")]
    public LineRenderer prefabRayTrail;
    #endregion

    #region Public properties
    public bool triggerDown
    {
        get { return m_TriggerDown; }
        set
        {
            m_TriggerDown = value;
            if (!m_TriggerDown) m_ShotDone = false;
        }
    }
    public WeaponState currentState { get; private set; }
    public int clipContent { get; private set; }
    public Actor owner { get; private set; }
    public bool ownerIsPlayer { get; private set; }
    #endregion

    #region Private field
    private bool m_TriggerDown;
    private bool m_ShotDone;
    private float m_ShotTimer = -1.0f;
    private int m_MimimumClipSize = 4;
    private int m_AmmoRemaining = 0;

    private int m_TrailPoolSize = 16;
    private List<ActiveTrail> m_ActiveTrails = new List<ActiveTrail>();
    private Queue<Projectile> m_ProjectilePool = new Queue<Projectile>();

    private Animator m_Animator;
    private int fireNameHash = Animator.StringToHash("fire");
    private int reloadNameHash = Animator.StringToHash("reload");

    private AudioSource m_Source;

    private WeaponRecoil m_WeaponRecoil;
    #endregion

    private void Awake()
    {
        clipContent = weaponStats.clipSize;
        m_AmmoRemaining = clipContent * (weaponStats.numberOfClips - 1);

        if (projectilePrefab != null)
        {
            int size = Mathf.Max(m_MimimumClipSize, weaponStats.clipSize) * advancedSettings.projectilePerShot;
            for (int i = 0; i < size; i++)
            {
                Projectile p = Instantiate(projectilePrefab);
                p.gameObject.SetActive(false);
                m_ProjectilePool.Enqueue(p);
            }
        }

        m_Animator = GetComponent<Animator>();
        m_Source = GetComponent<AudioSource>();
        m_WeaponRecoil = GetComponent<WeaponRecoil>();
    }

    private void Start()
    {
        if (prefabRayTrail != null)
            PoolSystem.Instance.InitPool(prefabRayTrail, m_TrailPoolSize);


    }

    private void Update()
    {
        UpdateControllerState();

        if (m_ShotTimer > 0)
            m_ShotTimer -= Time.deltaTime;

        //Animate trails 
        Vector3[] pos = new Vector3[2];
        for (int i = 0; i < m_ActiveTrails.Count; i++)
        {
            var activeTrail = m_ActiveTrails[i];

            activeTrail.renderer.GetPositions(pos);
            activeTrail.remainingTime -= Time.deltaTime;

            pos[0] += activeTrail.direction * 50.0f * Time.deltaTime;
            pos[1] += activeTrail.direction * 50.0f * Time.deltaTime;

            m_ActiveTrails[i].renderer.SetPositions(pos);

            if (m_ActiveTrails[i].remainingTime <= 0.0f)
            {
                m_ActiveTrails[i].renderer.gameObject.SetActive(false);
                m_ActiveTrails.RemoveAt(i);
                i--;
            }
        }
    }
    private void UpdateControllerState()
    {
        //This will control state of controller
        var info = m_Animator.GetCurrentAnimatorStateInfo(0);

        WeaponState newState;
        if (info.shortNameHash == fireNameHash)
            newState = WeaponState.Firing;
        else if (info.shortNameHash == reloadNameHash)
            newState = WeaponState.Reloading;
        else
            newState = WeaponState.Idle;

        if (newState != currentState)
        {
            var oldState = currentState;
            currentState = newState;

            if(oldState == WeaponState.Firing)
            {
                //we just finished firing, so check if we need to auto reload
                if (clipContent == 0)
                    Reload();
            }
        }

        if (triggerDown)
        {
            if (triggerType == TriggerType.Manual)
            {
                if (!m_ShotDone)
                {
                    m_ShotDone = true;
                    Fire();
                }
            }
            else
                Fire();
        }
    }

    public void PickedUp(Actor c)
    {
        owner = c;
        if (owner.gameObject.tag == "Player")
        {
            ownerIsPlayer = true;
        }
    }

    public void PutAway()
    {
        m_Animator.WriteDefaultValues();

        for (int i = 0; i < m_ActiveTrails.Count; i++)
        {
             m_ActiveTrails[i].renderer.gameObject.SetActive(false);
        }

        m_ActiveTrails.Clear();
    }

    public void Selected()
    {
        if (disabledOnEmpty)
            gameObject.SetActive(m_AmmoRemaining != 0 || clipContent != 0);

        //Set animation speed
        if (fireAnimationClip != null)
            m_Animator.SetFloat("fireSpeed", fireAnimationClip.length / weaponStats.fireRate);

        if (reloadAnimationClip != null)
            m_Animator.SetFloat("reloadSpeed", reloadAnimationClip.length / weaponStats.reloadTime);

        currentState = WeaponState.Idle;

        triggerDown = false;
        m_ShotDone = false;

        if (clipContent == 0 && m_AmmoRemaining != 0)
        {
            int chargeInClip = Mathf.Min(m_AmmoRemaining, weaponStats.clipSize);
            clipContent += chargeInClip;
            m_AmmoRemaining -= chargeInClip;

            
        }
        m_Animator.SetTrigger("selected");

        UpdateWeaponUI(true);

    }

    public void Fire()
    {
        if (currentState != WeaponState.Idle || m_ShotTimer > 0 || clipContent == 0)
            return;

        clipContent -= 1;
        m_ShotTimer = weaponStats.fireRate;

        UpdateWeaponUI(false);

        //the state will only change next frame, so we set it right now.
        currentState = WeaponState.Firing;
        m_Animator.SetTrigger("fire");
        m_Source.pitch = Random.Range(0.7f, 1.0f);
        m_Source.PlayOneShot(fireAudioClip);

        if (weaponType == WeaponType.Raycast)
        {
            for (int i = 0; i < advancedSettings.projectilePerShot; i++)
            {
                RaycastShot();
            }
        }
        else
        {
            ProjectileShot();
        }
    }

    private void RaycastShot()
    {
        RaycastHit hit;
        
        Ray r = GetRay();
        Vector3 hitPosition = r.origin + r.direction * 200.0f;

        if(Physics.Raycast(r, out hit, 1000.0f, ~(1 << 9), QueryTriggerInteraction.Ignore))
        {
            Renderer renderer = hit.collider.GetComponentInChildren<Renderer>();
            ImpactManager.Instance.PlayImpact(hit.point, hit.normal, renderer == null ? null : renderer.sharedMaterial);

            //if too close, the trail effect would look weird if it arced to hit the wall, so only correct it if far
            if (hit.distance > 5.0f)
                hitPosition = hit.point;

            Damageable target = hit.collider.gameObject.GetComponent<Damageable>();
            if (target != null)
                target.GotDamage(weaponStats.damage);
        }

        if (prefabRayTrail != null)
        {
            if (ownerIsPlayer)
                CreateRayTrail(GetCorrectedMuzzlePlace(), hitPosition);
            else
            {
                CreateRayTrail(endPoint.position, hitPosition);
            }            
        }

        if(m_WeaponRecoil != null)
            m_WeaponRecoil.GenerateRecoil();

    }

    //return ray from muzzle of weapon to forward
    private Ray GetRay()
    {
        if (ownerIsPlayer)
        {
            //compute the ratio of our spread angle over the fov to know in viewport space what is the possible offset from center
            float spreadRatio = advancedSettings.spreadAngle / FPSController.Instance.MainCamera.fieldOfView;
            Vector2 spread = spreadRatio * Random.insideUnitCircle;

            return FPSController.Instance.MainCamera.ViewportPointToRay(Vector3.one * 0.5f + (Vector3)spread);
        }
        else
        {
            return new Ray(endPoint.position, endPoint.transform.forward);
        }
    }

    /// <summary>
    /// This will compute the corrected position of the muzzle flash in world space. Since the weapon camera use a
    /// different FOV than the main camera, using the muzzle spot to spawn thing rendered by the main camera will appear
    /// disconnected from the muzzle flash. So this convert the muzzle post from
    /// world -> view weapon -> clip weapon -> inverse clip main cam -> inverse view cam -> corrected world pos
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCorrectedMuzzlePlace()
    {
        Vector3 position = endPoint.position;

        position = FPSController.Instance.WeaponCamera.WorldToScreenPoint(position);
        position = FPSController.Instance.MainCamera.ScreenToWorldPoint(position);

        return position;
    }

    private void CreateRayTrail(Vector3 begin, Vector3 end)
    {
        var trail = PoolSystem.Instance.GetInstance<LineRenderer>(prefabRayTrail);
        trail.gameObject.SetActive(true);
        trail.SetPosition(0, begin);
        trail.SetPosition(1, end);

        m_ActiveTrails.Add(new ActiveTrail()
        {
            remainingTime = 0.3f,
            direction = (end - begin).normalized,
            renderer = trail
        });
    }

    private void ProjectileShot()
    {
        for (int i = 0; i < advancedSettings.projectilePerShot; i++)
        {
            float angle = Random.Range(0.0f, advancedSettings.spreadAngle * 0.5f);
            Vector2 angleDir = Random.insideUnitCircle * Mathf.Tan(angle * Mathf.Deg2Rad);

            Vector3 dir = endPoint.transform.forward + (Vector3)angleDir;
            dir.Normalize();

            var p = m_ProjectilePool.Dequeue();

            p.gameObject.SetActive(true);
            p.Launch(this, dir, projectileLaunchForce);
        }
    }

    //For optimization, when a projectile is "destroyed" it is instead disabled and return to the weapon for reuse.
    public void ReturnProjecticle(Projectile p)
    {
        m_ProjectilePool.Enqueue(p);
    }


    public void Reload()
    {
        if (currentState != WeaponState.Idle || clipContent == weaponStats.clipSize)
            return;

        if (m_AmmoRemaining == 0)
        {
            //No more bullet, so we disable the gun so it's displayed on empty (useful e.g. for grenade)
            if (disabledOnEmpty)
                gameObject.SetActive(false);
            return;
        }

        int chargeInClip = Mathf.Min(m_AmmoRemaining, weaponStats.clipSize - clipContent);

        //the state will only change next frame, so we set it right now.
        currentState = WeaponState.Reloading;
        m_Animator.SetTrigger("reload");

        if (reloadAudioClip != null)
        {
            m_Source.pitch = Random.Range(0.7f, 1.0f);
            m_Source.PlayOneShot(reloadAudioClip);
        }

        clipContent += chargeInClip;
        m_AmmoRemaining -= chargeInClip;

        UpdateWeaponUI(true);
    }

    private void UpdateWeaponUI(bool reload)
    {
        if (ownerIsPlayer)
        {
            WeaponInfoUI.Instance.UpdateClipInfo(clipContent);
            if (reload)
            {
                WeaponInfoUI.Instance.UpdateAmmoRemaining(m_AmmoRemaining);
            }
        } 
    }  
}
