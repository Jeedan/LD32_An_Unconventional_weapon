using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    public Text playerHealthText;
    public Text playerStaminaText;

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
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        deathAnimator = DeathDisplay.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playDeathAnimation()
    {
        deathAnimator.SetBool("Alive", false);
    }

    public void playAliveAnimation()
    {
        deathAnimator.SetBool("Alive", true);
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
