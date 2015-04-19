using UnityEngine;
using System.Collections;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject[] Weapons;

    private int currentIndex = 0;

    public GameObject currentWeapon;

    public DestroyOnTriggerEnter[] hitBoxScripts;
    // Use this for initialization
    void Start()
    {
        hitBoxScripts = new DestroyOnTriggerEnter[Weapons.Length];
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i] != null)
            {
                hitBoxScripts[i] = Weapons[i].GetComponentInChildren<DestroyOnTriggerEnter>();
            }
        }


        if (Weapons != null)
        {
            currentWeapon = Weapons[0];
        }
        for (int w = 1; w <= Weapons.Length - 1; w++)
        {
            if (Weapons[w] != null)
            {
                Weapons[w].SetActive(false);
            }
        }
    }

    void updateHitbox(int index)
    {
        // update our hitboxes

        hitBoxScripts[index] = Weapons[index].GetComponentInChildren<DestroyOnTriggerEnter>();

        // hide all other guns
        if (index != 0)
        {
            Weapons[index].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            currentIndex = 0;
            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);

        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            currentIndex = 1;

            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);

        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            currentIndex = 2;

            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);


        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            currentIndex = 3;
            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);

        }

        if (currentIndex >= Weapons.Length)
        {
            currentIndex = Weapons.Length - 1;
        }
    }

    void SwapWeapon(int index)
    {
        currentWeapon.SetActive(false);
        currentWeapon = Weapons[index];
        currentWeapon.SetActive(true);
    }

    public void pickUpAndEquipWeapon(GameObject weap)
    {
        if (currentWeapon == null)
        {
            currentIndex = 0;
            Weapons[currentIndex] = weap;
            currentWeapon = weap;
            updateHitbox(currentIndex);
        }
        else
        {
            currentIndex++;
            if (currentIndex < Weapons.Length )
            {
                Weapons[currentIndex] = weap;
                updateHitbox(currentIndex);
            }
            else
            {
                fullSlots = true;
            }
        }
        //currentIndex = 0;
        //currentWeapon = Weapons[currentIndex];
        //currentWeapon.SetActive(true);
    }

    public void toggleWeaponHitbox(bool value)
    {
        if (hitBoxScripts[currentIndex] != null)
            hitBoxScripts[currentIndex].toggleHitBox(value);
    }

    private bool fullSlots = false;
    public bool fullInventory()
    {
        return fullSlots;
    }
}
