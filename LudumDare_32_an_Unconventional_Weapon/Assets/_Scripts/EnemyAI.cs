using UnityEngine;
using System.Collections;
[RequireComponent(typeof(EnemyHealth))]
public class EnemyAI : MonoBehaviour
{

    private PlayerController playerScript;
    private Rigidbody rig;
    private Vector3 movement;
    public GameObject target;

    public float speed = 5.0f;
    public float wanderRadius = 10.0f;
    public float ignoreRange = 10.0f;

    #region attack values
    public float attackRange = 3.5f;
    public float damage = 2.0f;
    public float attackRate = 1.2f;
    private float attackTimer = -1.0f;
    #endregion

    public LayerMask playerLayer;
    public LayerMask wallLayer;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            MoveToTarget();
        }
        else
        {
            WanderAround();
        }
    }

    public float wanderDelay = 1.0f;
    public float wanderTimer = -1.0f;

    public Vector3 randomPos;
    Vector3 dirToRandomPoint;

    private void WanderAround()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;

        Debug.DrawRay(transform.position, transform.forward * 3.0f, Color.red);

        if (Time.time > wanderDelay + wanderTimer)
        {
            // dirToRandomPoint.y = 0.0f;

            randomPos = transform.position + (Random.insideUnitSphere * wanderRadius);
            dirToRandomPoint = randomPos - transform.position;
            dirToRandomPoint.y = 0.0f;

            wanderTimer = Time.time;

        }
        else if (Physics.Raycast(ray, out hitInfo, 2.5f, wallLayer))
        {
            if (hitInfo.transform.gameObject != gameObject)
            {
                randomPos = transform.position + (Random.insideUnitSphere * wanderRadius);

                dirToRandomPoint = randomPos - transform.position;
                dirToRandomPoint.y = 0.0f;
            }
        }


        RotateToTarget(dirToRandomPoint.normalized);
        Move(dirToRandomPoint.normalized);
    }

    private void MoveToTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 dirNormalized = direction.normalized;
        float distance = direction.magnitude;
        if (distance > ignoreRange)
        {
            WanderAround();
        }

        if (distance < ignoreRange && distance > attackRange)
        {
            RotateToTarget(dirNormalized);
            Move(dirNormalized);
        }
        else if (distance <= attackRange)
        {
            movement = Vector3.zero;
            rig.velocity = movement;
            RotateToTarget(dirNormalized);
            AttackPlayer(dirNormalized);
        }
    }

    private void Move(Vector3 dir)
    {
        movement = dir * speed * Time.deltaTime;
        movement.y = 0.0f;
        rig.MovePosition(transform.position + movement);
    }

    private void AttackPlayer(Vector3 direction)
    {
        if (Time.time > attackRate + attackTimer)
        {
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 5, playerLayer))
            {
                if (hitInfo.transform.gameObject == target)
                {
                    Debug.Log("attacking the player");
                    // TODO enemy attack animation
                    playerScript.TakeDamage(damage);
                    attackTimer = Time.time;
                }
            }
        }
    }

    private void RotateToTarget(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            direction.y = 0.0f;
            Quaternion targetLookRotation = Quaternion.LookRotation(direction);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetLookRotation, 150.0f * Time.deltaTime);
            rig.MoveRotation(rot);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.gameObject != gameObject)
        {
            Vector3 otherPos = collision.gameObject.transform.position;

            Vector3 offSet = otherPos - transform.position;
            movement += offSet * 100.0f;
        }
    }
}
