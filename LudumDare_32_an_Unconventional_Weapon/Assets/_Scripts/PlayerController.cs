using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private UIManager uiManager;
    private PlayerWeapon weaponScript;

    private Rigidbody rig;
    private Animator anim;
    private Vector3 movement;

    public bool attacking = false;
    public bool canMove = true;
    public bool canInput = true;

    private float strafe;
    private float forward; 

    public LayerMask walkableLayerMask;
    public float strafeSpeed = 5.0f;
    public float forwardSpeed = 10.0f;

#region attack timers
    // TODO move damage to weapon
    public float damage = 2.0f;
    public float attackRate = 2.0f;
    private float attackCoolDownTimer = 0.0f;
#endregion

    public Resource healthScript;
    public Resource staminaScript;

    #region regeneration
    public float staminaRegen = 1.0f;
    public float regenDelay = 1.0f;
    private float regenTimer = -2.0f;
    #endregion

    public Image damageImage; 
    public float flashSpeed = 5.0f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    public bool damaged;

    private float initialDamage;
    public Color prev;
    public Material playerMat;
    public void FlashOnDamage()
    {
        if (damaged)
        {
            playerMat.color = flashColour;
            //damageImage.color = flashColour;
        }
        else
        {
            playerMat.color = Color.Lerp(playerMat.color, prev, flashSpeed * Time.deltaTime);
            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    public void FlashInitialize()
    {
        // color flashing
        playerMat = GetComponentInChildren<Renderer>().material;
        prev = playerMat.color;
    }

    // Use this for initialization
    void Start()
    {
        // color flashing
        playerMat = GetComponentInChildren<Renderer>().material;
        prev = playerMat.color;
        initialDamage = damage;

        anim = transform.FindChild("_PlayerModel").GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        weaponScript = GetComponent<PlayerWeapon>();

        healthScript.MaxResource = healthScript.currentResource;
        staminaScript.MaxResource = staminaScript.currentResource;

        // HUD related stuff
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.setPlayerMaxHealth(healthScript.MaxResource);
        uiManager.setPlayerHealth(healthScript.currentResource);
        uiManager.setPlayerStamina(staminaScript.currentResource);
    }

   public void ResetValues()
    {
        healthScript.currentResource = healthScript.MaxResource;
        staminaScript.currentResource = healthScript.MaxResource;
        uiManager.setPlayerHealth(healthScript.currentResource);
        uiManager.setPlayerStamina(staminaScript.currentResource);

        attacking = false;
        canMove = true;
        canInput = true;
    }

    void Update()
    {
        if (canInput)
        {
            strafe = Input.GetAxisRaw("Horizontal"); // side to side movement
            forward = Input.GetAxisRaw("Vertical"); // forward movement
        }
        Attack();

        FlashOnDamage();
        damaged = false;
    
    }

    void FixedUpdate()
    {
        if (canInput)
        {
            Move(strafe, forward);
            AnimateMovement(strafe, forward);
            Turn(movement);
        }
        // when not attacking start regenerating stamina again
        RegenStamina();
    }

    void CheckIfPlayerIsDead()
    {
        if (healthScript.isDead())
        {
            uiManager.playDeathAnimation();
          //  transform.position = new Vector3(-100.0f, 1.0f, 0.0f);
            damaged = false;
            canMove = false;
            canInput = false;
            gameObject.SetActive(false);
            return;
        }
        else
        {
            uiManager.playAliveAnimation();
        }

    }

    public void TakeDamage(float amount)
    {
        damaged = true;
        healthScript.currentResource = healthScript.SubtractResource(healthScript.currentResource, amount);

        uiManager.setPlayerHealth(healthScript.currentResource);
        CheckIfPlayerIsDead();
    }

    void RegenStamina()
    {

        if (!attacking && staminaScript.currentResource < staminaScript.MaxResource && Time.time > regenDelay + regenTimer)
        {
            staminaScript.currentResource += staminaRegen;
            uiManager.setPlayerStamina(staminaScript.currentResource);
            if (staminaScript.currentResource > staminaScript.MaxResource)
            {
                staminaScript.currentResource = staminaScript.MaxResource;
            }
            regenTimer = Time.time;
        }
    }

    void Move(float inputX, float inputZ)
    {
        // bail out of the method if we cannot move
        if (!canMove) return;

        // movement = new Vector3(inputX, 0.0f, inputZ );
        movement = transform.forward * inputZ + transform.right * inputX;
        movement.y = 0.0f;
        float speedX = strafeSpeed * Time.deltaTime;
        float speedZ = forwardSpeed * Time.deltaTime;

        movement.Normalize();

        movement.x *= speedX;
        movement.z *= speedZ;
        movement.y = 0.0f;

        rig.MovePosition(transform.position + movement);
    }

    void AnimateMovement(float inputX, float inputZ)
    {
        // bail out of the method if we cannot move
        if (!canMove) return;

        bool walkForward = inputZ != 0.0f || inputX != 0.0f;
        anim.SetBool("isWalking", walkForward);
    }

    void Turn(Vector3 _direction)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorhit;

        if (Physics.Raycast(ray, out floorhit, Mathf.Infinity, walkableLayerMask))
        {
            Vector3 facing = floorhit.point - transform.position;
            float distance = facing.magnitude;

            facing.y = 0.0f;

            if (distance > 2.0f)
            {
                Quaternion targetLookRotation = Quaternion.LookRotation(facing);
                rig.MoveRotation(targetLookRotation);
            }
        }
    }

    void Attack()
    {
        // Todo take this out and make it a class
        // todo create "AbilityInfo" class for stats
        if (Input.GetMouseButtonDown(0) && canInput)
        {
            if (!weaponScript.currentWeapon) return;
            float attackStaminaCost;

            string weaponType = weaponScript.getWeaponType();
            
            if (weaponType == "hoover")
            {
                attackStaminaCost = 25.0f;
                damage *= 2;
            }
            else
            {
                attackStaminaCost = 10.0f;
                damage = initialDamage;
            }


            if ((staminaScript.currentResource > 0.0f && staminaScript.currentResource >= attackStaminaCost) && Time.time > attackRate + attackCoolDownTimer)
            {
                attacking = true;
                StopMoving();
                StartCoroutine(LockMovement(attackRate));
                // subtract "stamina" from player

                staminaScript.currentResource = SubtractResource(staminaScript.currentResource, attackStaminaCost);

                uiManager.setPlayerStamina(staminaScript.currentResource);

                // deal damage
                attackCoolDownTimer = Time.time;
            }
        }
    }

    /// <summary>
    /// Lock movement during attack animation
    /// when the attack animation is 60% complete
    /// we activate the hitbox
    /// we activate the hitbox for 10% of the duration
    /// then we disable it and wait another 10%
    /// then unlock movement
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    IEnumerator LockMovement(float t)
    {
        float animDelay;
        float hitboxActiveTime ;
        string weaponType = weaponScript.getWeaponType();

        if (weaponType == "hoover")
        {
            animDelay = t * 0.3f;
            hitboxActiveTime = t * 0.4f;
            canMove = true;

        }
        else
        {
            animDelay = t * 0.6f;
            hitboxActiveTime = t * 0.2f;
            canMove = false;
            LockInput();
        }

        // animate the attack
        AnimateAttack();
        yield return new WaitForSeconds(animDelay);

        weaponScript.toggleWeaponHitbox(true);
        yield return new WaitForSeconds(hitboxActiveTime);
        weaponScript.toggleWeaponHitbox(false);
        yield return new WaitForSeconds(hitboxActiveTime);
        FreeInput();
        canMove = true;
        attacking = false; 
        if (weaponType == "hoover")
        {
            weaponScript.StopHooving();
        }
    }

    IEnumerator FreeInput(float t)
    {
        yield return new WaitForSeconds(t);
        FreeInput();
    }

    void AnimateAttack()
    {
        string weaponType = weaponScript.getWeaponType();
        if (weaponType == "hoover")
        {
            weaponScript.ActivateHoovingParticle();
        }
        else
        {
            anim.SetTrigger("Attack");
        }
    }

    public void StopMoving()
    {
        // freeze movement        
        Move(0, 0);
        AnimateMovement(0, 0);
    }

    public void LockInput()
    {
        canInput = false;
    }

    public void FreeInput()
    {
        canInput = true;
    }

    public void FreeMovement()
    {
        canInput = true;
    }

    public float SubtractResource(float value, float amount)
    {
        float result = value - amount;

        if (result < 0.0f)
        {
            result = 0.0f;
        }

        return (result);
    }
}
