using UnityEngine;
using System.Collections;

public class DestroyOnTriggerEnter : MonoBehaviour
{
    public string[] killTags;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        foreach (string tag in killTags)
        {
            if (other.tag == tag)
            {
                // TODO add timer
                // spawn death animation
                // add score
                // drop item?
                // drop health
                Destroy(other.gameObject);
            }
        }
    }

}
