using UnityEngine;
using System.Collections;

public class MeleeSwipe : MonoBehaviour
{
    public string[] killTags;
    public BoxCollider hitBox;

    private PlayerController playerScript;

    private float damage;

    public void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        damage = playerScript.damage;
        hitBox = GetComponent<BoxCollider>();
        hitBox.enabled = false;
    }

    public void toggleHitBox(bool value)
    {
        hitBox.enabled = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        foreach (string tag in killTags)
        {
            if (other.tag == tag)
            {
                // spawn death animation
                // add score
                // drop item?
                // drop health
                other.GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }

}
