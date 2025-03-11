using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    bool isSprinting = false;
    public float wallRunSpeed;
    public float climbSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    
    bool readyToJump;

    [Header("Sprint Jump Condition")]
    public float sprintJumpMultiplier = 1.5f;
    private float requiredSprintTime = 0.9f; // Time needed to enable sprint-jumping
    private float sprintTimer = 0f;
    private bool canSprintJump = false;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Gravity Control")]
    public float fallGravityMultiplier = 1.5f;


    

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Ref")]
    public Climbing climbingScript;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        wallrunning,
        climbing,
        air
    }
    public bool wallrunning;
    public bool climbing;
    public bool canAirJump = false;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
        //if (isServer && isClient) // Host
        //{
        //    walkSpeed = 15f; // Host speed
        //}
        //else if (isClient) // Client
        //{
        //    walkSpeed = 2f; // Client speed
        //}
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
        if (!grounded && rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.down * fallGravityMultiplier, ForceMode.Acceleration);
        }
    }

    private void FixedUpdate()
    {
       
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(sprintKey) && verticalInput > 0)
        {
            isSprinting = true;
        }

        if (verticalInput <= 0)
        {
            isSprinting = false;
        }

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        else if (Input.GetKeyDown(jumpKey) && canAirJump && !grounded)
        {
            Jump();
            canAirJump = false; // Use the air jump and reset the flag
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    private void StateHandler()
    {
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallRunSpeed;
        }
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (grounded)
        {
            if (isSprinting && verticalInput > 0)
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
                sprintTimer += Time.deltaTime;
                if (sprintTimer >= requiredSprintTime)
                {
                    canSprintJump = true;
                }
            }
            else
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
                sprintTimer = 0f;
                canSprintJump = false;
            }
        }
        else
        {
            state = MovementState.air;
        }
    }


    private void MovePlayer()
    {
        if (climbingScript.exitingWall) return;
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset vertical velocity before jumping
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Apply sprint jump multiplier if sprinting
        float currentJumpForce = (isSprinting && canSprintJump) ? jumpForce * sprintJumpMultiplier : jumpForce;

        rb.AddForce(transform.up * currentJumpForce, ForceMode.Impulse);

        canSprintJump = false;
        sprintTimer = 0f;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}