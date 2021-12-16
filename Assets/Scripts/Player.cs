using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float XmaxVelocity = 10f;
    [SerializeField] private float YmaxVelocity = 10f;
    [SerializeField] private float jumpDelay = 0.25f;


    [Header("Components")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;

    [Header("Physics")]
    [SerializeField] private float linearDrag = 4f;
    [SerializeField] private float gravity = 1f;
    [SerializeField] private float FallMultiplier = 1f;

    private float horizontalMovement;
    private float jumpTimer;
    const float groundedRadius = 0.2f;

    private bool JumpPressed { get;  set; }
    public bool IsGrounded { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }
    private void FixedUpdate()
    {
        Move();
        CheckForGround();
        ModifyPhysics();
        JumpLogic();
    }

    private void Move()
    {
        //rb.AddForce(Vector2.right * horizontalMovement * speed);
        rb.velocity = new Vector2(horizontalMovement * speed, rb.velocity.y); 
        if (Mathf.Abs(rb.velocity.x) > XmaxVelocity)
            rb.velocity = new Vector2(horizontalMovement * XmaxVelocity, rb.velocity.y);

        if (rb.velocity.y < 0 &&  Mathf.Abs(rb.velocity.y) > YmaxVelocity)
            rb.velocity = new Vector2(rb.velocity.x,  YmaxVelocity * Mathf.Sign(rb.velocity.y));

    }

    private void CheckForGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IsGrounded = true;
                return;
            }
        }
        IsGrounded = false;
    }

    private void ModifyPhysics()
    {
        bool changingDirections = (horizontalMovement > 0 && rb.velocity.x < 0) || (horizontalMovement < 0 && rb.velocity.x > 0);
        if (IsGrounded)
        {
            if (horizontalMovement == 0 || changingDirections)
                rb.drag = linearDrag;
            else
                rb.drag = 0;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * FallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                    rb.gravityScale = gravity * (FallMultiplier / 2);
            }
        }
    }

    private void JumpLogic()
    {
        if (jumpTimer > Time.time && IsGrounded)
        {
            jumpTimer = 0;
            IsGrounded = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundedRadius);
    }
}
