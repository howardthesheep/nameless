using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float groundDistance = 0.01f;
    public float speed = 6f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 3.0f;
    float turnSmoothVelocity;

    Vector3 velocity;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset velocity when grounded
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -0.01f;
            animator.SetTrigger("isGrounded");

            // Detect jump input
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("JumpClicked");
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

        }

        // Check if user is entering move commands
        if (Mathf.Abs(horizontal)+Mathf.Abs(vertical) >= 0.1f)
        {
            //TODO: Trigger Run animation, 
            animator.SetFloat("Speed", speed);


            // Get the angle the camera is facing, and rotate the player body to the same angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Update player position
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * speed * Time.deltaTime);

        } else {
            animator.SetFloat("Speed", 0f);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
