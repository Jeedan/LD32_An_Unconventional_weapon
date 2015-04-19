using UnityEngine;
using System.Collections;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    public string[] killTags;
    public BoxCollider hitBox;

    public void Start()
    {
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
                Destroy(other.gameObject);
            }
        }
    }

}
