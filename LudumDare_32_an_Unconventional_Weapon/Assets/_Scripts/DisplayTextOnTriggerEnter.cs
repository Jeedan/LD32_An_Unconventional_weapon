using UnityEngine;
using System.Collections;

public class DisplayTextOnTriggerEnter : MonoBehaviour
{


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIManager.instance.playerIsCloseEnough(true, gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {

        UIManager.instance.playerIsCloseEnough(false, gameObject);
    }
}
