using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject[] Weapons;

    private int currentIndex = 0;

    private GameObject currentWeapon;

    public DestroyOnTriggerEnter[] hitBoxScripts;
    // Use this for initialization
    void Start()
    {
        hitBoxScripts = new DestroyOnTriggerEnter[Weapons.Length];
        for (int i = 0; i < Weapons.Length; i++)
        {
            hitBoxScripts[i] = Weapons[i].GetComponentInChildren<DestroyOnTriggerEnter>();
        }

        if (Weapons != null)
        {
            currentWeapon = Weapons[0];
        }
        for (int w = 1; w <= Weapons.Length-1; w++)
        {
            Weapons[w].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            currentIndex = 0;

            currentWeapon.SetActive(false);
            currentWeapon = Weapons[currentIndex];
            currentWeapon.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            currentIndex = 1;
            currentWeapon.SetActive(false);
            currentWeapon = Weapons[currentIndex];
            currentWeapon.SetActive(true);
        }

        if (currentIndex >= Weapons.Length)
        {
            currentIndex = Weapons.Length-1;
        }
    }

    public void toggleWeaponHitbox(bool value)
    {
        hitBoxScripts[currentIndex].toggleHitBox(value);
    }
}
