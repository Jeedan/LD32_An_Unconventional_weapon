using UnityEngine;
using System.Collections;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    public string[] killTags;

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
