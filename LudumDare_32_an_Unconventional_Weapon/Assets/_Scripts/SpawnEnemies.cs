using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnEnemies : MonoBehaviour
{
    public int count = 2;

    public static int currentlySpawned = 0;

    public GameObject[] enemyPrefab;
    public GameObject[] enemySpawns;
    private List<GameObject> enemies;
    private int enemyIndex = 0;

    public static int killedEnemies = 0;
    private bool isSpawning = false;
    // Use this for initialization
    void Start()
    {
        enemies = new List<GameObject>();

        Spawn();
    }

    void Update()
    {
        if (killedEnemies % count == 0 && currentlySpawned <= 0)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (enemyPrefab != null )
        {
            for (int i = 0; i <= count-1; i++)
            {
                Vector3 enemyPos = enemySpawns[i].transform.position;
                

                GameObject enemy = Instantiate(enemyPrefab[enemyIndex], enemyPos, Quaternion.identity) as GameObject;
                enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                enemy.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                enemy.GetComponent<EnemyAI>().enabled = false;
                //enemies[i] = enemy;
                enemies.Add(enemy);
                enemy.GetComponent<EnemyAI>().enabled = true;
                currentlySpawned++;
                if (currentlySpawned >= count / 2)
                {
                    enemyIndex++;
                    if (enemyIndex >= enemyPrefab.Length-1)
                    {
                        enemyIndex = enemyPrefab.Length-1;
                    }
                }

            }
            isSpawning = false;
        }
    }

}
