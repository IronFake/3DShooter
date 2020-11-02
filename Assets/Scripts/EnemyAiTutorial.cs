using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public float fieldOfView;
    public float health;

    //Patroling
    public float walkPointRange;


    //Attacking
    public float timeBetweenAttacks;
    public GameObject projectile;
    

    //States
    public float attackRange;


    private Transform target;
    private bool alreadyAttacked;
    private bool playerInSightRange, targetInAttackRange;
    private bool hasTarget;
    private List<Transform> targetsInAttackRange = new List<Transform>();
    private Vector3 walkPoint;
    private bool walkPointSet;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);
        targetsInAttackRange.Clear();
        foreach (var c in colliders)
        {
            if (c.transform.root != transform)
            {
                CheckCollider(c.transform);
            }
        }

        if(targetsInAttackRange.Count > 0)
        {
            if(targetsInAttackRange.Count == 1)
            {
                target = targetsInAttackRange[0];
            }
            else
            {
                target = targetsInAttackRange[0];
                foreach (var t in targetsInAttackRange)
                {
                    if (Vector3.Magnitude(t.position - transform.position) < Vector3.Magnitude(target.position - transform.position))
                    {
                        target = t;
                    }
                }
            }
        }
        else
        {
            target = null;
        }

        if (target != null) AttackPlayer();
        else Patroling();
    }

    private void CheckCollider(Transform t)
    {
        Vector3 direction = t.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < fieldOfView * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit, attackRange))
            {
                if (hit.collider.gameObject == t.gameObject)
                {
                    targetsInAttackRange.Add(t);
                }
            }
        }
    }

    private void Patroling()
    {
        
        if (!walkPointSet)
        {
            do SearchWalkPoint();
            while (!CheckWhatNewPathInArea(walkPoint));
        }
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

/*
        float angle = Random.Range(-fieldOfView/2.5f, fieldOfView/2.5f);
        float distanceRange = Random.Range(1, 2);
        Vector3 t = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

        walkPoint = transform.position + t * distanceRange;*/

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    //
    bool CheckWhatNewPathInArea(Vector3 endPoint)
    {
        float angle = Vector3.Angle(endPoint, transform.position);
        if (angle > fieldOfView * 0.5f)
            return false;

        NavMeshPath navMeshPath = new NavMeshPath();
        agent.CalculatePath(endPoint, navMeshPath);
        print("New path calculated");
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
        //agent.SetDestination(transform.position);

        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            var dir = target.position - transform.position;
            rb.AddForce(dir * 5, ForceMode.Impulse);

            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
