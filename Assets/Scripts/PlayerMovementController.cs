using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")] 
    public float moveSpeed;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    [Header("Keybinds")] public KeyCode jumpKey = KeyCode.Space;
    [Header("Ground Check")] public float playerHeight;
    public float groundDrag;
    public LayerMask whatIsGround;
    public bool grounded; 

    public Transform orientation;

    private float horizontalInput;

    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
        Cursor.visible = false;

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //this.transform.rotation = orientation.rotation;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MoveInput();
    
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        SpeedControl();
        
    }

    void MoveInput()
    {
        horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");
        if(Input.GetKey(jumpKey)&&readyToJump&&grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.transform.forward* verticalInput + orientation.transform.right * horizontalInput;
        //rb.AddForce(moveDirection.normalized*moveSpeed*10f,ForceMode.Force);

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized*moveSpeed*10f,ForceMode.Force);
        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized*moveSpeed*10f*airMultiplier,ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up*jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void changeMoveSpeed(float speed)
    {
        this.moveSpeed *= speed;
    }
}
