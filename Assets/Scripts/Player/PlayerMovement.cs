using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    //public float playerHeight;
    public LayerMask groundLayer;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;

    

    private float playerHeight;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        playerHeight = GetComponent<CapsuleCollider>().height;
    }

    private void Update()
    {       
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundLayer);

        MyInput();
        SpeedControl();

        // handle drag
        rb.drag = grounded ? groundDrag : 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
       
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // on ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * GetComponent<Rigidbody>().mass * 10f, ForceMode.Force);
        }
            
        // in air
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * GetComponent<Rigidbody>().mass * 10f * airMultiplier, ForceMode.Force);
        }          
    }

    private void SpeedControl()
    {
        
        Vector3 flatVel = new(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        
    }
    
    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * GetComponent<Rigidbody>().mass, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}