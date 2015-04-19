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
    public float ignoreRange = 10.0f;

    #region attack values
    public float attackRange = 3.5f;
    public float damage = 2.0f;
    public float attackRate = 1.2f;
    private float attackTimer = -1.0f;
    #endregion

    public LayerMask playerLayer;
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
    }

    private void MoveToTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 dirNormalized = direction.normalized;
        float distance = direction.magnitude;
        if (distance > ignoreRange) return;
        if (distance < ignoreRange && distance > attackRange)
        {
            RotateToTarget(dirNormalized);
            movement = dirNormalized * speed * Time.deltaTime;
            rig.MovePosition(transform.position + movement);
        }
        else if (distance <= attackRange)
        {
            movement = Vector3.zero;
            rig.velocity = movement;
            RotateToTarget(dirNormalized);
            AttackPlayer(dirNormalized);
        }
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
                    // TODO play attack animation
                    playerScript.TakeDamage(damage);
                    attackTimer = Time.time;
                }
            }
        }
    }

    private void RotateToTarget(Vector3 direction)
    {
        Quaternion targetLookRotation = Quaternion.LookRotation(direction);

        rig.MoveRotation(targetLookRotation);
    }

    public void OnCollisionEnter(Collision collision)
    {
    }
}
