using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Slider healthBarSlider;
    public Slider staminaBarSlider;
    
    public Text playerHealthText;
    public Text playerStaminaText;
    public Text playerCleanScoreText;
    public Text playerKillsText;
    public Text wavesText;
    public Text newWaveText;

    #region Dialog
    public GameObject DialogBox;
    public Text npcConvoText;
    public float letterDelay = 0.3f;

    private bool showText = false;
    private bool isShowing = false;
    public string[] npcText = { "HELLO INTERN", "GO CLEAN THE CELLAR?", "What are you waiting for?" };
    public GameObject npcTrigger;
    #endregion

    #region Death Animation
    public GameObject DeathDisplay;
    public Animator deathAnimator;
    #endregion
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
       // DontDestroyOnLoad(gameObject);

        healthBarSlider = GameObject.Find("HealthBar").GetComponent<Slider>();
        staminaBarSlider = GameObject.Find("StaminaBar").GetComponent<Slider>();
        playerHealthText = GameObject.Find("Health").GetComponent<Text>();
        playerStaminaText = GameObject.Find("Stamina").GetComponent<Text>();
        playerCleanScoreText = GameObject.Find("CleaningScore").GetComponent<Text>();
        playerKillsText = GameObject.Find("Kills").GetComponent<Text>();
        wavesText = GameObject.Find("Waves").GetComponent<Text>();
        newWaveText = GameObject.Find("NewWaveDisplay").GetComponent<Text>();
        newWaveText.gameObject.SetActive(false);


        DialogBox = GameObject.Find("ConversationPanel");
        npcConvoText = GameObject.Find("NPC_TEXT").GetComponent<Text>();

        DeathDisplay = GameObject.Find("DeathDisplay");
        deathAnimator = DeathDisplay.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        playerCleanScoreText.text = "Cleaning score: 0";

        DialogBox.gameObject.SetActive(false);
        playerStaminaText.gameObject.SetActive(false);
        playerHealthText.gameObject.SetActive(false);
       
    }

    public void showNewWaveText(bool value)
    {
        newWaveText.gameObject.SetActive(value);
    }

    public void setNewWaveMessage(string message)
    {
        string msg = message;
        newWaveText.text = msg;
    }


    public void playDeathAnimation()
    {
        deathAnimator.SetBool("Alive", false);
    }

    public void playAliveAnimation()
    {
        deathAnimator.SetBool("Alive", true);
    }

    public void setPlayerMaxHealth(float hp)
    {
        healthBarSlider.maxValue = hp;
    }
    
    public void setPlayerMaxStamina(float hp)
    {
        staminaBarSlider.maxValue = hp;
    }

    public void setPlayerHealth(float hp)
    {
        string playerHealth = "Health: " + hp.ToString("0");
        healthBarSlider.value = hp;
        playerHealthText.text = playerHealth;
    }

    public void setPlayerStamina(float stamina)
    {
        string playerStamina = "Stamina: " + stamina.ToString("0");
        playerStaminaText.text = playerStamina;
        staminaBarSlider.value = stamina;
    }

    public void setPlayerKills(float kills)
    {
        string playerKills = "Cleaning score: " + kills.ToString("0");
        playerCleanScoreText.text = playerKills;
    }

    public void setKills(float kills)
    {
        string playerKills =  kills.ToString("0") +" :Kills";
        playerKillsText.text = playerKills;
    }

    public void displayNPCDialogue()
    {
        DialogBox.SetActive(true);
        StartCoroutine(ScrollingText());
    }

    public void hideNPCDialogue()
    {

        if (isShowing) return;
        DialogBox.SetActive(false);
    }

    public void playerIsCloseEnough(bool displayText, GameObject trigger)
    {
        showText = displayText;

        if (showText)
        {
            npcTrigger = trigger;
            npcTrigger.SetActive(false);
            npcConvoText.text = "";
            displayNPCDialogue();
        }
        else
        {
            npcConvoText.text = "";
            hideNPCDialogue();
        }
    }

    IEnumerator ScrollingText()
    {
        npcConvoText.text = "";
        bool doneDisplaying = false;
        var lineIndex = 0;
        foreach (string word in npcText)
        {
            foreach (var letter in word.ToCharArray())
            {
                npcConvoText.text += letter;
                yield return new WaitForSeconds(letterDelay);
                if (npcConvoText.text == npcText[lineIndex])
                {
                    bool wordDone = true;
                    lineIndex++;
                    if (wordDone && lineIndex < npcText.Length)
                    {
                        yield return new WaitForSeconds(1.0f);

                        npcConvoText.text = "";
                        wordDone = false;

                    }
                }
            }
        }


        doneDisplaying = true;
        yield return new WaitForSeconds(2);
        string[] repeatThisText = { npcText[npcText.Length-1] };
        npcText = repeatThisText;
        Debug.Log("finished displaying");
        isShowing = !doneDisplaying;

        npcTrigger.SetActive(true);
        hideNPCDialogue();
    }
}
