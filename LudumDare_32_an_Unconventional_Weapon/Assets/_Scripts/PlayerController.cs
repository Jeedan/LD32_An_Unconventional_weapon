using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private UIManager uiManager;
    private PlayerWeapon weaponScript;

    private Rigidbody rig;
    private Animator anim;
    private Vector3 movement;

    private bool attacking = false;
    private bool canMove = true;
    private bool canInput = true;

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

    // Use this for initialization
    void Start()
    {
        anim = transform.FindChild("_PlayerModel").GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        weaponScript = GetComponent<PlayerWeapon>();

        healthScript.MaxResource = healthScript.currentResource;
        staminaScript.MaxResource = staminaScript.currentResource;

        // HUD related stuff
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.setPlayerHealth(healthScript.currentResource);
        uiManager.setPlayerStamina(staminaScript.currentResource);
    }

   public void ResetValues()
    {
        healthScript.currentResource = healthScript.MaxResource;
        staminaScript.currentResource = healthScript.MaxResource;
        uiManager.setPlayerHealth(healthScript.currentResource);
        uiManager.setPlayerStamina(staminaScript.currentResource);
    }

    void Update()
    {
        if (canInput)
        {
            strafe = Input.GetAxisRaw("Horizontal"); // side to side movement
            forward = Input.GetAxisRaw("Vertical"); // forward movement
        }
        Attack();
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
            float attackStaminaCost = 10.0f;
            if ((staminaScript.currentResource > 0.0f && staminaScript.currentResource >= attackStaminaCost) && Time.time > attackRate + attackCoolDownTimer)
            {
                attacking = true;
                StopMoving();
                LockInput();
                StartCoroutine(LockMovement(attackRate));
                // subtract "stamina" from player
                staminaScript.currentResource = SubtractResource(staminaScript.currentResource, 10);
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
        float animDelay = t * 0.6f;
        float hitboxActiveTime = t * 0.2f;
        canMove = false;
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
    }

    IEnumerator FreeInput(float t)
    {
        yield return new WaitForSeconds(t);
        FreeInput();
    }

    void AnimateAttack()
    {
        anim.SetTrigger("Attack");
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


    public float SubtractResource(float value, float amount)
    {
        float result = value - amount;

        if (result < 0.0f)
        {
            result = 0.0f;
        }

        return (result);
    }

    //    public void KnockBack(Vector3 direction)
    //    {
    //        Vector3 startPos = transform.position;
    //        Vector3 endPos;

    //        LockInput();
    //        StopMoving();
    //        // movement = new Vector3(inputX, 0.0f, inputZ );
    //        movement = direction.normalized;

    //        float speedZ = 500.0f * Time.deltaTime;

    //        movement.z *= speedZ;

    //        endPos = transform.position + movement;
    //        rig.MovePosition(transform.position + movement);

    ////        rig.MovePosition(Vector3.Lerp(startPos, endPos, Vector3.Distance(transform.position, movement) / movement.magnitude));
    //        StartCoroutine(FreeInput(0.2f));

    //    }
}
