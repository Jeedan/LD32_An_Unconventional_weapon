using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text playerHealthText;
    public Text playerStaminaText;


    public void Awake()
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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setPlayerHealth(float hp)
    {
        string playerHealth = "Health: " + hp.ToString("0");
        playerHealthText.text = playerHealth;
    }

    public void setPlayerStamina(float stamina)
    {
        string playerStamina = "Stamina: " + stamina.ToString("0");
        playerStaminaText.text = playerStamina;
    }
}
