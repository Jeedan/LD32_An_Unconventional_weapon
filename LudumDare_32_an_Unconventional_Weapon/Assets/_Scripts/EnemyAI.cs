using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;

    public PlayerController playerScript;

    private Rigidbody rig;

    private Vector3 movement;
    public float speed = 5.0f;
    public float attackRange = 3.5f;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        playerScript = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        Vector3 dirNormalized = direction.normalized;
        float distance = direction.magnitude;

        if (distance > attackRange)
        {
            RotateToTarget(dirNormalized);
            movement = dirNormalized * speed * Time.deltaTime;
            rig.MovePosition(transform.position + movement);
        }
        else if (distance <= attackRange)
        {
            movement = Vector3.zero;
            RotateToTarget(dirNormalized);
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
