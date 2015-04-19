using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject[] Weapons;

    private int currentIndex = 0;

    private GameObject currentWeapon;
    // Use this for initialization
    void Start()
    {
        if (Weapons != null)
        {
            currentWeapon = Weapons[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            currentIndex = 0;
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            currentIndex = 1;
        }
    }
}
