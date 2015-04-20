using UnityEngine;
using System.Collections;

public class PickUpWeapon : MonoBehaviour
{
    private PlayerWeapon weaponScript;
    private GameObject weaponSlot;

    private SphereCollider pickUpCollider;

    public float spinSpeed = 60.0f;

    public string weaponType = "weapon";

    private bool pickedUp = false;
    // Use this for initialization
    void Start()
    {
        pickUpCollider = GetComponent<SphereCollider>();
        weaponSlot = GameObject.FindGameObjectWithTag("Player").transform.FindChild("_PlayerModel").transform.FindChild("arm.R").transform.FindChild("weapon_slot").gameObject;
        weaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
    }

    public void Update()
    {
        if (!pickedUp)
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            if (!weaponScript.fullInventory())
            {

                weaponScript.pickUpAndEquipWeapon(gameObject);
                positionAndRotateWeapon(weaponType);
                pickUpCollider.enabled = false;
                pickedUp = true;

                // todo check weapon type, adjust rotation according
                // 
            }
        }
    }

    public void positionAndRotateWeapon(string _type)
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

        if (weaponScript.currentWeapon != null && weaponScript.currentWeapon != gameObject)
        {
            gameObject.SetActive(false);
        }
        else
        {
            weaponSlot.transform.localRotation = pickupRot;
        }

        transform.position = weaponSlot.transform.position;
        transform.rotation = weaponSlot.transform.rotation;
        transform.SetParent(weaponSlot.transform);
    }
}
