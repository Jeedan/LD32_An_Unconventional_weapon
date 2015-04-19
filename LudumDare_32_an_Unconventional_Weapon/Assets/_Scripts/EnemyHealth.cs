using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public Resource healthScript;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (healthScript.isDead())
        {
            // Spawn death explosion
            // destroy this GameObject
            SpawnEnemies.killedEnemies++;
            SpawnEnemies.currentlySpawned--;
            Debug.Log("killedEnemies: " + SpawnEnemies.killedEnemies, gameObject);
            Destroy(gameObject);
        }

    }

    public void TakeDamage(float amount)
    {
        healthScript.currentResource = healthScript.SubtractResource(healthScript.currentResource, amount);
    }
}
