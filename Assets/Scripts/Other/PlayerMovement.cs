using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;

    Rigidbody rb;


    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    //public int jumpClickCounter;
    //public int frameCounter;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    //[Header("Flight")]
    //public bool inFlight;
    //public float flightTimer;
    //public float flightMaxTime;
    //public GameObject FlightParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        //jumpClickCounter = 0;
        //inFlight = false;

        //flightTimer = flightMaxTime * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }



        MyInput();

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);

        if(grounded)
        {
            rb.drag = groundDrag;
            //inFlight = false;
            //jumpClickCounter = 0;
        }
        else
        {
            rb.drag = 0;
        }

        SpeedControl();


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //calc mov dir
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
       

    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //jumpClickCounter++;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
