using UnityEngine;
using System.Collections;

public class PickUpWeapon : MonoBehaviour
{
    private PlayerWeapon weaponScript;
    public GameObject weaponSlot;

    private SphereCollider pickUpCollider;
    // Use this for initialization
    void Start()
    {
        pickUpCollider = GetComponent<SphereCollider>();
        weaponSlot = GameObject.FindGameObjectWithTag("Player").transform.FindChild("_PlayerModel").transform.FindChild("arm.R").transform.FindChild("weapon_slot").gameObject;
        weaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapon>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            weaponScript.pickUpAndEquipWeapon(gameObject);

            if (!weaponScript.fullInventory())
            {
                transform.position = weaponSlot.transform.position;
                transform.rotation = weaponSlot.transform.rotation;
                transform.SetParent(weaponSlot.transform);
                pickUpCollider.enabled = false;
            }
        }
    }
}
