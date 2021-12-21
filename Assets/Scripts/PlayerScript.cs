using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerScript : MonoBehaviour
{
    [Header("Components")]
     private CharacterController controller;

    [Header("Physics")]
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpHeight = 4;
    [SerializeField] private float timeToJumpApex = .4f;
    [SerializeField] private float accelerationTimeGrounded = .1f;
    [SerializeField] private float accelerationTimeAirborne = .2f;
    [SerializeField] private float MaxVelocity_y = 20f;

    private Vector3 velocity,
                    oldVelocity,
                    deltaPos;
    private Vector2 input;

    private float targetVelocityX;
    private float velocityXSmoothing;

    private float gravity => -2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2);
    private float jumpForce => Mathf.Abs(gravity) * timeToJumpApex;

    public bool IsGrounded { get;  set; }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        IsGrounded = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            Jump();
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetVelocityX = input.x * moveSpeed;
        if (velocity.y < -MaxVelocity_y)
            velocity.y = -MaxVelocity_y;
    }

    private void FixedUpdate()
    {

        oldVelocity = velocity;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, 
                     IsGrounded ? accelerationTimeGrounded : accelerationTimeAirborne);
        Flip();
        velocity.y += gravity * Time.fixedDeltaTime;
        deltaPos = (oldVelocity + velocity) * 0.5f * Time.fixedDeltaTime;
        controller.Move(deltaPos);
    
        IsGrounded = controller.collisions.down;
        if (IsGrounded || controller.collisions.up)
            velocity.y = 0;
    }

    private void Flip()
    {
        transform.localScale = new Vector3(1 * Mathf.Sign(velocity.x), 1, 1);
    }

    private void Jump()
    {
        velocity.y = jumpForce;
    }
}
