using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;
    private Character character;
    public Vector2 inputDirection;
    [Header("Basic Parameters")]
    public float speed;
    private float runSpeed;
    private float walkSpeed => speed / 2.5f;
    public float jumpForce;
    public float wallJumpForce;
    public float hurtForce;
    public float slideDistance;
    public float slideSpeed;
    public int slidePowerCost;

    [Header("Phyical Meterial")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    [Header("Statuses")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;
    public bool wallJump;
    public bool isSlide;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        inputControl = new PlayerInputControl();
        character = GetComponent<Character>();

        // Jump
        inputControl.Gameplay.Jump.started += Jump;

        #region Forced Walking
        runSpeed = speed;
            inputControl.Gameplay.WalkButton.performed += ctx => 
            {
                if(physicsCheck.isGround)
                    speed = walkSpeed;
            };

            inputControl.Gameplay.WalkButton.canceled += ctx =>
            {
                if(physicsCheck.isGround)
                    speed = runSpeed;
            };
        #endregion

        // Attack
        inputControl.Gameplay.Attack.started += PlayerAttack;
        
        // Slide
        inputControl.Gameplay.Slide.started += Slide;
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();

        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
            Move();
    }

    // // Test
    // private void OnTriggerStay2D(Collider2D other)
    // {
        // Debug.Log(other.name);
    // }

    public void Move()
    {
        if (!wallJump)
            rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);

        int faceDir = (int)transform.localScale.x;
        if (!isSlide)
        {
            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0)
                faceDir = -1;
        }

        // Flip Player
        transform.localScale = new Vector3(faceDir, 1, 1);
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        // Debug.Log("JUMP");
        if(physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

            // Stop slide
            isSlide = false;
            StopAllCoroutines();
        }
        else if (physicsCheck.onWall)
        {
            rb.AddForce(new Vector2(-inputDirection.x, 2.5f) * wallJumpForce, ForceMode2D.Impulse);
            wallJump = true;
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)

    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }

    private void Slide(InputAction.CallbackContext context)
    {
          int faceDir = (int)transform.localScale.x;
        if (!isSlide)
        {
            if (inputDirection.x > 0)
                faceDir = 1;
            if (inputDirection.x < 0)
                faceDir = -1;
        }
        if (!isSlide && physicsCheck.isGround && character.currentPower >= slidePowerCost && !(faceDir < 0 && physicsCheck.touchLeftWall) && !(faceDir > 0 && physicsCheck.touchRightWall))
        {
            isSlide = true;
            var targetPos = new Vector3(transform.position.x + slideDistance * transform.localScale.x, transform.position.y);
            StartCoroutine(TriggerSlide(targetPos));

            character.OnSlide(slidePowerCost);
        }
    }

    private IEnumerator TriggerSlide(Vector3 target)
    {
        do
        {
            yield return null;
            if (!physicsCheck.isGround)
                break;

            // Touching wall during slide
            if (physicsCheck.touchLeftWall && transform.lossyScale.x < 0 || physicsCheck.touchRightWall && transform.lossyScale.x > 0)
            {
                isSlide = false;
                break;
            }

            rb.MovePosition(new Vector2(transform.position.x + transform.localScale.x * slideSpeed, transform.position.y));
        } while (MathF.Abs(target.x - transform.position.x) > 0.1f);
        isSlide = false;
    }

    #region UnityEvent
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        inputControl.Gameplay.Disable();
    }
    #endregion

    private void CheckState()
    {
        coll.sharedMaterial = physicsCheck.isGround  ? normal : wall;

        if (physicsCheck.onWall)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 1.5f);
        else
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (wallJump && rb.velocity.y < 0f)
            wallJump = false;

         if (isDead || isSlide)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
