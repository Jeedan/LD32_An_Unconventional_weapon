using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpawnEnemies : MonoBehaviour
{

    private bool spawnedHoover = false;
    [SerializeField]
    private List<GameObject> enemies;
    private int enemyIndex = 0;
    public int count = 2;

    public int currentlySpawned = 0;

    public GameObject hooverPrefab;
    public Transform weaponSpawn;

    public GameObject[] enemyPrefab;
    public GameObject[] enemySpawns;

    public int killedEnemies = 0;


    public float brainDelay = 3.0f;
    private float brainTimer = -3.0f;

    public int Wave = 1;

    // Use this for initialization
    void Start()
    {
        enemies = new List<GameObject>();

        Spawn();
    }

    void Update()
    {
        if (enemies.Count <= 0)
        {
            StartCoroutine(DisplayMessage());
            if (Time.time > brainDelay + brainTimer)
            {
                Spawn();
                Wave++;
                count += 2;
                count = Mathf.Clamp(count, 1, 20);
                UIManager.instance.wavesText.text = Wave + " :Wave";
            }
        }

        if (Time.time > brainDelay + brainTimer)
        {

            UIManager.instance.setNewWaveMessage("Wave " + Wave);

            if (enemies != null)
            {
                foreach (GameObject en in enemies)
                {
                    if (en != null)
                    {

                        en.GetComponent<EnemyAI>().enabled = true;


                        brainTimer = Time.time;

                    }
                }
            }
        }

        if (killedEnemies >= 15)
        {
            SpawnWeapon();
        }
    }
    IEnumerator DisplayMessage()
    {

        UIManager.instance.showNewWaveText(true);

        UIManager.instance.setNewWaveMessage("Wave Incoming");
        yield return new WaitForSeconds(2.0f);

        UIManager.instance.setNewWaveMessage("Wave " + Wave);
        yield return new WaitForSeconds(1.0f);
        UIManager.instance.showNewWaveText(false);
    }

    public void Spawn()
    {
        if (enemyPrefab != null)
        {
            enemyIndex = 0;
            int randomEnemies = Random.Range(0, 8);

            for (int i = 0; i <= count - 1; i++)
            {
                Vector3 enemyPos = enemySpawns[i].transform.position;


                GameObject enemy = Instantiate(enemyPrefab[enemyIndex], enemyPos, Quaternion.identity) as GameObject;
                enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
                enemy.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                enemy.GetComponent<EnemyAI>().enabled = false;
                //enemies[i] = enemy;
                enemies.Add(enemy);
                if (currentlySpawned >= randomEnemies)
                {
                    enemyIndex++;
                    if (enemyIndex >= enemyPrefab.Length - 1)
                    {
                        enemyIndex = enemyPrefab.Length - 1;
                    }
                }

                currentlySpawned++;
            }


        }
    }

    public void SpawnWeapon()
    {
        if (spawnedHoover) return;
        Instantiate(hooverPrefab, weaponSpawn.position, hooverPrefab.transform.rotation);
        spawnedHoover = true;
    }

    public void RemoveEnemy(GameObject go)
    {
        enemies.Remove(go);
    }

}
