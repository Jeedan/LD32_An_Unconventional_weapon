using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public LayerMask walkableLayerMask;
    public float strafeSpeed = 5.0f;
    public float forwardSpeed = 10.0f;

    private Rigidbody rig;
    private Animator anim;
    private Vector3 movement;


    public float attackRate = 2.0f;
    [SerializeField]
    private float attackCoolDownTimer = 0.0f;

    public bool canMove = true;
    public bool canInput = true;

    public bool attacking = false;

    // TODO MOVE THIS TO ITS OWN CLASS
    public float health = 100.0f;
    private float maxhealth;
    public float stamina = 100.0f;
    [SerializeField]
    private float maxStamina;
    public float staminaRegen = 1.0f;

    private UIManager uiManager;

    // Use this for initialization
    void Start()
    {
        anim = transform.FindChild("_PlayerModel").GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiManager.setPlayerHealth(health);
        uiManager.setPlayerStamina(stamina);
        maxStamina = stamina;
        maxhealth = health;
    }

    void Update()
    {
        Attack();
    }

    public float regenDelay = 1.0f;
    public float regenTimer = -2.0f;

    // Update is called once per frame
    void FixedUpdate()
    {

        if (canInput)
        {
            float strafe = Input.GetAxisRaw("Horizontal"); // side to side movement
            float forward = Input.GetAxisRaw("Vertical"); // forward movement

            Move(strafe, forward);
            AnimateMovement(strafe, forward);
            Turn(movement);
        }

        if (!attacking && stamina < maxStamina && Time.time > regenDelay + regenTimer)
        {
            stamina += staminaRegen;
            uiManager.setPlayerStamina(stamina);
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
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
            float attackStaminaCost = 10.0f;
            if ((stamina > 0.0f && stamina >= attackStaminaCost) && Time.time > attackRate + attackCoolDownTimer)
            {
                attacking = true;
                StopMoving();

                StartCoroutine(LockMovement(attackRate));
                // subtract "stamina" from player
                stamina = SubtractResource(stamina, 10);
                uiManager.setPlayerStamina(stamina);

                // deal damage
                attackCoolDownTimer = Time.time;
            }
        }
    }

    IEnumerator LockMovement(float t)
    {
        canMove = false;
        // animate the attack
        AnimateAttack();
        yield return new WaitForSeconds(t);
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

    public bool isDead()
    {
        if (health > 0.0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ResetHealth()
    {
        health = maxhealth;
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
