using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    [System.Serializable]
    public class ImpactSetting
    {
        public ParticleSystem ParticlePrefab;
        public GameObject bulletHole;
        public AudioClip ImpactSound;
        public Material TargetMaterial;
    }

    static public ImpactManager Instance { get; protected set; }

    public ImpactSetting defaultSettings;
    public ImpactSetting[] impactSettings;

    Dictionary<Material, ImpactSetting> m_SettingLookup = new Dictionary<Material, ImpactSetting>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        PoolSystem.Instance.InitPool(defaultSettings.ParticlePrefab, 32);
        foreach (var impactSettings in impactSettings)
        {
            PoolSystem.Instance.InitPool(impactSettings.bulletHole, 20);
            PoolSystem.Instance.InitPool(impactSettings.ParticlePrefab, 32);
            m_SettingLookup.Add(impactSettings.TargetMaterial, impactSettings);
        }
    }

    public void PlayImpact(Vector3 position, Vector3 normal, Material material = null)
    {
        ImpactSetting setting = null;
        if (material == null || !m_SettingLookup.TryGetValue(material, out setting))
        {
            setting = defaultSettings;
        }

        var sys = PoolSystem.Instance.GetInstance<ParticleSystem>(setting.ParticlePrefab);
        sys.gameObject.transform.position = position;
        sys.gameObject.transform.forward = normal;

        sys.gameObject.SetActive(true);
        sys.Play();

        if(setting.bulletHole != null)
        {
            var bulletHole = PoolSystem.Instance.GetInstance<GameObject>(setting.bulletHole);
            bulletHole.transform.position = position;
            bulletHole.transform.forward = normal;
            bulletHole.SetActive(true);
        }
    }
}
