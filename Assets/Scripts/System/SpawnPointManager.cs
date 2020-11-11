using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

    private List<SpawnPoint> m_spawnPlaces;

    private int m_numberOfAttempts = 0;
    private int m_maxNumberOfAttempt = 30;

    private void Awake()
    {
        Instance = this;
        m_spawnPlaces = GetComponentsInChildren<SpawnPoint>().ToList();
    }

    public Vector3 GetRandomSpawnPoint(bool mustBeFree)
    {
        SpawnPoint sp;
        m_numberOfAttempts = 0;
        do
        {
            if (m_numberOfAttempts > m_maxNumberOfAttempt)
                throw new Exception("Don't find free spawn point. Add it more in scene");
                        
            int index = UnityEngine.Random.Range(0, m_spawnPlaces.Count);
            sp = m_spawnPlaces[index];

            if (!mustBeFree)
                return sp.transform.position + Vector3.up;

            m_numberOfAttempts++;
        } while (!sp.IsFree());


        return sp.transform.position + Vector3.up;
    }
}
