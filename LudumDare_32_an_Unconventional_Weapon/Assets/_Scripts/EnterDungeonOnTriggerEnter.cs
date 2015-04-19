using UnityEngine;
using System.Collections;

public class EnterDungeonOnTriggerEnter : MonoBehaviour
{

    public GameObject player;
    public Transform spawn;


    public GameObject baseEnvironment;

    void Start()
    {
        spawn = GameObject.Find("Dungeon_Player_Spawn").transform;
        baseEnvironment = GameObject.Find("Base_Level");
        player = GameObject.FindGameObjectWithTag("Player");

    }

    public void OnTriggerEnter(Collider other)
    {
        // TODO Screen Fade
        // dungeon position will be somewhere like 1000 units to the right or something
        player.transform.position = spawn.position;
        Camera.main.transform.position = new Vector3(spawn.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.GetComponent<SmoothFollowCamera>().enabled = true;
        // deactivate base
        UIManager.instance.hideNPCDialogue();
        baseEnvironment.SetActive(false);
    }
}
