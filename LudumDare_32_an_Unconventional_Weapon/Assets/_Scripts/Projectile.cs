using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    Rigidbody rig;
    public float speed = 10.0f;
    public float damage = 2.0f;
    PlayerController player;
    // Use this for initialization
    void Start()
    {
        GameObject playerGO =GameObject.FindGameObjectWithTag("Player");
        if (playerGO)
        {
            player = playerGO.GetComponent<PlayerController>();
        }
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = transform.forward * speed * Time.deltaTime;

        Destroy(gameObject, 5.0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
