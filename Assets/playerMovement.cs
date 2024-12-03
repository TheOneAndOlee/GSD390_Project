using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMove : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    public float dashForce;
    public float dashCooldown;
    bool readyToDash = true;
    bool isDashing = false;
    public float dashDuration = 0.2f;
    private Vector3 dashDirection;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.E;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask floor;
    bool grounded;

    public Transform orientation;

    float HorizontalInput;
    float VerticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, floor);

        MyInput();
        speedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0;
        }
    }
    
    private void FixedUpdate()
    {
        movePlayer();
    }

    private void MyInput()
    {
        // Get the input from the WASD keys
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }

        if (Input.GetKey(dashKey) && readyToDash && !isDashing)
        {
            readyToDash = false;
            isDashing = true;

            Dash();
        }
    }

    private void movePlayer()
    {
        moveDirection = orientation.forward * VerticalInput + orientation.right * HorizontalInput;

        if (grounded == true) {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        } else if (grounded == false)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f,  rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 controlledVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(controlledVel.x, rb.velocity.y, controlledVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Dash()
    {
        Vector3 inputDirection = new Vector3(HorizontalInput, 0f, VerticalInput).normalized;

        if (inputDirection.magnitude == 0)
        {
            dashDirection = orientation.forward;
        }
        else
        {
            dashDirection = orientation.right * inputDirection.x + orientation.forward * inputDirection.z;
        }

        StartCoroutine(ApplyDashForce());
    }

    private void resetDash()
    {
        readyToDash = true;
        isDashing = false;
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private IEnumerator ApplyDashForce()
    {
        float startTime = Time.time;


        while (Time.time < startTime + dashDuration)
        {
            rb.AddForce(dashDirection * dashForce, ForceMode.Force);
            yield return null;
        }
         
        rb.velocity = Vector3.zero;

        Invoke(nameof(resetDash), dashCooldown);
    }
}
