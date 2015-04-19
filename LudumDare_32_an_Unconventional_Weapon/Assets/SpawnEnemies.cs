using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnEnemies : MonoBehaviour
{
    public int count = 2;

    int currentlySpawned = 0;

    public GameObject[] enemyPrefab;
    public GameObject[] enemySpawns;
    private List<GameObject> enemies;
    private int enemyIndex = 0;

    // Use this for initialization
    void Start()
    {
        enemies = new List<GameObject>();

        Spawn();
    }

    public void Spawn()
    {
        if (enemyPrefab != null && currentlySpawned == 0)
        {
            for (int i = 0; i <= count-1; i++)
            {
                int spawnIndex = Random.Range(1, enemySpawns.Length);
                Vector3 enemyPos = enemySpawns[spawnIndex].transform.position;

                GameObject enemy = Instantiate(enemyPrefab[enemyIndex], enemyPos, Quaternion.identity) as GameObject;

                //enemies[i] = enemy;
                enemies.Add(enemy);
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
        }
    }

}
