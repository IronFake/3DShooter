using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public LayerMask layerMask;
    public float overlapRaduis = 3;

    private float m_Radius = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        for (float i = -m_Radius; i <= m_Radius; i += m_Radius)
        {
            for (float j = -m_Radius; j <= m_Radius ; j += m_Radius)
            {
                Vector3 newPos = transform.position;
                newPos.x += i;
                newPos.z += j;
                
                Gizmos.DrawRay(newPos, transform.up * 10);
                Gizmos.DrawLine(transform.position, newPos);
            }
        }   
    }


    public bool IsFree()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, overlapRaduis, layerMask);
        return colliders.Length == 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, overlapRaduis);
    }

}
