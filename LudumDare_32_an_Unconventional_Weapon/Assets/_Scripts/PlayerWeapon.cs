using UnityEngine;
using System.Collections;


public class PlayerWeapon : MonoBehaviour
{
    public GameObject[] Weapons;

    private int currentIndex = 0;
    private int previousIndex = 0;

    public GameObject currentWeapon;
    public GameObject weaponSlot;
    public MeleeSwipe[] hitBoxScripts;

    public string weaponType = "weapon";

    private PickUpWeapon pickUpScript;

    // Use this for initialization
    void Start()
    {

        weaponSlot = GameObject.FindGameObjectWithTag("Player").transform.FindChild("_PlayerModel").transform.FindChild("arm.R").transform.FindChild("weapon_slot").gameObject;
        hitBoxScripts = new MeleeSwipe[Weapons.Length];
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i] != null)
            {
                hitBoxScripts[i] = Weapons[i].GetComponentInChildren<MeleeSwipe>();
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

        hitBoxScripts[index] = Weapons[index].GetComponentInChildren<MeleeSwipe>();

        // hide all other guns
        if (index > 0)
        {
            Weapons[index].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            previousIndex = currentIndex;
            currentIndex = 0;
            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);

        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            previousIndex = currentIndex;

            currentIndex = 1;

            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);

        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            previousIndex = currentIndex;

            currentIndex = 2;

            SwapWeapon(currentIndex);
            Debug.Log("currentWeapon: " + currentIndex);


        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            previousIndex = currentIndex;

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

        if (Weapons[index] != null)
        {
            currentWeapon.SetActive(false);
            currentWeapon = Weapons[index];
            pickUpScript = currentWeapon.GetComponent<PickUpWeapon>();
            weaponType = pickUpScript.weaponType;
            positionAndRotateWeapon(weaponType);
            currentWeapon.SetActive(true);
        }
        else
        {
            currentIndex = previousIndex;
        }
    }

    public void positionAndRotateWeapon(string type)
    {
        Quaternion pickupRot;

        if (weaponType == "hoover")
        {
            pickupRot = Quaternion.Euler(0.0f, 90.0f, 357);


        }
        else
        {
            pickupRot = Quaternion.Euler(0.0f, 90.0f, 90.0f);

        }

        weaponSlot.transform.localRotation = pickupRot;
        currentWeapon.transform.position = weaponSlot.transform.position;
        currentWeapon.transform.rotation = weaponSlot.transform.rotation;
    }

    public void pickUpAndEquipWeapon(GameObject weap)
    {
        if (currentWeapon == null)
        {
            currentIndex = 0;
            Weapons[currentIndex] = weap;
            currentWeapon = weap;
            currentWeapon.SetActive(true);
            updateHitbox(currentIndex);
        }
        else
        {
            ++currentIndex;

            if (currentIndex > 0)
            {
                if (currentIndex < Weapons.Length)
                {
                    Weapons[currentIndex] = weap;

                    updateHitbox(currentIndex);
                }
                else
                {
                    fullSlots = true;
                }
            }            
        }
        //currentIndex = 0;
        //currentWeapon = Weapons[currentIndex];
        //currentWeapon.SetActive(true);
    }

    public void ActivateHoovingParticle()
    {
        ParticleSystem partic = currentWeapon.GetComponentInChildren<ParticleSystem>();
        if (partic != null)
        {
            partic.Play();
        }
    }

    public void StopHooving()
    {
        ParticleSystem partic = currentWeapon.GetComponentInChildren<ParticleSystem>();
        if (partic != null)
        {
            partic.Stop();
        }
    }

    public string getWeaponType()
    {
        pickUpScript = currentWeapon.GetComponent<PickUpWeapon>();
        weaponType = pickUpScript.weaponType;

        return weaponType;
    }

    public void toggleWeaponHitbox(bool value)
    {
        if(hitBoxScripts[currentIndex]!= null)
            hitBoxScripts[currentIndex].toggleHitBox(value);
    }

    private bool fullSlots = false;
    public bool fullInventory()
    {
        return fullSlots;
    }
}
