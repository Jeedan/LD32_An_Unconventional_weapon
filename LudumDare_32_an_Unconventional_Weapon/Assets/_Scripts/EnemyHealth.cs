using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public Resource healthScript;
    public GameObject healthGlobePrefab;

    SpawnEnemies spawnScript;

    public bool canFlash = true;
    public float flashSpeed = 5.0f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    private bool damaged;

    private Color prev;
    private Material enemyMaterial;

    public void FlashOnDamage()
    {
        if (damaged)
        {
            enemyMaterial.color = flashColour;
            //damageImage.color = flashColour;
        }
        else
        {
            enemyMaterial.color = Color.Lerp(enemyMaterial.color, prev, flashSpeed * Time.deltaTime);
            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    public void FlashInitialize()
    {
        // color flashing
        enemyMaterial = GetComponentInChildren<Renderer>().material;
        prev = enemyMaterial.color;
    }

    // Use this for initialization
    void Start()
    {
        spawnScript = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnEnemies>();
        if (canFlash)
        {
            FlashInitialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthScript.isDead())
        {
            // Spawn death explosion
            // destroy this GameObject
            spawnScript.killedEnemies++;
            spawnScript.currentlySpawned--;
            float score = spawnScript.killedEnemies * 15.0f;
            UIManager.instance.setPlayerKills(score);
            UIManager.instance.setKills(spawnScript.killedEnemies);
            
            float randomNumber = Random.Range(1, 50);
            if (randomNumber > 25 && randomNumber < 40)
            {
                Instantiate(healthGlobePrefab, transform.position, Quaternion.identity);
            }
            spawnScript.RemoveEnemy(gameObject);
            damaged = false;

            Destroy(gameObject);

        }

        if (canFlash)
        {
            FlashOnDamage();
        }
        damaged = false;
    }

    public void TakeDamage(float amount)
    {
        damaged = true;
        healthScript.currentResource = healthScript.SubtractResource(healthScript.currentResource, amount);
    }
}
