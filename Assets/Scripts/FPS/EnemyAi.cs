using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAi : MonoBehaviour
{
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public float fieldOfView;
    public float walkPointRange;
    public float attackRange;

    private NavMeshAgent m_agent;
    private WeaponManager m_weaponManager;
    private Transform m_target;
    private Vector3 m_walkPoint;
    private bool m_walkPointSet;

    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        m_target = FindTarget();

        if (m_target != null) AttackPlayer();
        else 
        {
            m_weaponManager.Shoot(false);
            Patroling();
        }            
    }

    private Transform FindTarget()
    {
        Transform currentTarget = null;

        var targetColliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (var tc in targetColliders)
        {
            Vector3 targetPos = tc.transform.position;
            if (tc.transform.root != transform)
            {
                Vector3 direction = targetPos - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < fieldOfView * 0.5f)
                {
                    //Check what enemy see target
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, direction.normalized, out hit, attackRange))
                    {
                        if(hit.collider.gameObject == tc.gameObject)
                        {
                            if (currentTarget == null)
                                currentTarget = tc.transform;
                            else
                            {
                                // Find nearest enemy 
                                if (Vector3.Magnitude(targetPos - transform.position) <
                                    Vector3.Magnitude(currentTarget.position - transform.position))
                                {
                                    currentTarget = tc.transform;
                                }
                            }
                        }
                    }
                }
            }
        }

        return currentTarget;
    }

    private void Patroling()
    {
        
        if (!m_walkPointSet)
        {
            do SearchWalkPoint();
            while (!CheckWhatNewPathInArea(m_walkPoint));
        }
        else
        {
            m_agent.SetDestination(m_walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - m_walkPoint;
        distanceToWalkPoint.y = 0;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            m_walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        var point = SpawnPointManager.Instance.GetRandomSpawnPoint(false);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        m_walkPoint = new Vector3(point.x + randomX, point.y, point.z + randomZ);


        if (Physics.Raycast(m_walkPoint, -transform.up, 2f, whatIsGround))
            m_walkPointSet = true;
    }

    bool CheckWhatNewPathInArea(Vector3 endPoint)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        m_agent.CalculatePath(endPoint, navMeshPath);
   
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        m_agent.SetDestination(transform.position);
        transform.LookAt(m_target);

        m_weaponManager.Shoot(true);
    }

    public void ResetAttack()
    {
        m_weaponManager.Shoot(false);
        m_walkPointSet = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
