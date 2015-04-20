using UnityEngine;
using System.Collections;

public class SpinAround : MonoBehaviour
{
    public float despawnTime = 2.0f;
    public float rotateSpeed = 60.0f;
    BoxCollider boxCol;
    PlayerController playerScript;
    Resource playerHealth;


    // Use this for initialization
    void Start()
    {
        
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerHealth = playerScript.healthScript;

        boxCol = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed* Time.deltaTime, Space.World);
        Destroy(gameObject, despawnTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHealth.currentResource += 10.0f;

            if (playerHealth.currentResource >= playerHealth.MaxResource)
            {
                playerHealth.currentResource = playerHealth.MaxResource;
            }
            UIManager.instance.setPlayerHealth(playerHealth.currentResource);
            Destroy(gameObject);
            boxCol.enabled = false;
        }
    }
}
