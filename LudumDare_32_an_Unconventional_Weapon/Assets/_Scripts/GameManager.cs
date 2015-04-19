using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    /// <summary>
    /// Unity singleton
    /// </summary>
    #region SingleTon Unity Style
    public static GameManager instance = null;
    void SingletonInitialize()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public GameObject playerPrefab;

    public GameObject player;
    public Transform playerSpawnPoint;
    public GameObject baseEnvironment;

    public int score = 0;
    public int deathCount = 0;
    public void Awake()
    {
        SingletonInitialize();

        baseEnvironment = GameObject.Find("Base_Level");
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawnPoint = GameObject.Find("Base_Player_Spawn").transform;
        // keep track of score
        // TODO save score in playerprefs
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //TODO respawn button resets scene
    }

    public void Reset()
    {
        UIManager.instance.playAliveAnimation();
        SmoothFollowCamera cam = Camera.main.GetComponent<SmoothFollowCamera>();
        PlayerController playerScript = player.GetComponent<PlayerController>();
        playerScript.ResetValues();
        player.SetActive(true);
        player.transform.position = playerSpawnPoint.position;
        Vector3 camRespawnPos = Camera.main.transform.position - player.transform.position;
        Camera.main.transform.position = camRespawnPos;

        cam.enabled = true;
        //Camera.main.transform.position = new Vector3(playerSpawnPoint.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        // deactivate base
        UIManager.instance.hideNPCDialogue();
        baseEnvironment.SetActive(true);
    }
}
