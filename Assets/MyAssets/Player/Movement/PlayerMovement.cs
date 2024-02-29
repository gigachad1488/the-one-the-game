using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    private InputManager inputManager;

    private Rigidbody2D rb;

    [Space(5)]
    [Header("Player Sprite")]
    public GameObject sprite;

    [Space(5)]
    [Header("Ground Check")]
    [SerializeField]
    private Collider2D groundCollider;
    [SerializeField]
    private LayerMask groundLayers;

    private bool isGrounded;

    [Space(5)]
    [Header("Movement")]
    public float maxXSpeed = 8f;
    public float maxYSpeed = 16f;
    public float moveSpeed = 8f;

    [Space(5)]
    [Header("Jump")]
    public float jumpForce = 10f;
    public float jumpCD;
    private float jumpCDTimer;

    public float glideSpeed = 4f;

    public float flyingForce = 1f;
    public float flyingTime = 3f;
    private float flyingTimer;

    [Space(5)]
    [Header("Dash")]
    public float dashForce = 10f;
    public float dashCD = 1;
    private float dashCDTimer;

    private void Start()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        Jump();        
        VelocityLimit();
        Dash();
    }

    public void GroundCheck()
    {
        isGrounded = groundCollider.IsTouchingLayers(groundLayers);
    }

    private void Move()
    {
        if (Mathf.Abs(inputManager.move.x) > 0)
        {
            rb.AddForceX(inputManager.move.x * moveSpeed, ForceMode2D.Force);

            if (inputManager.move.x > 0) 
            {
                sprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                sprite.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void Jump()
    {
        if (inputManager.jump)
        {
            if (isGrounded && jumpCDTimer < 0)
            {
                rb.AddForceY(jumpForce, ForceMode2D.Impulse);
                flyingTimer = flyingTime;
                jumpCDTimer = jumpCD;
            }
            else
            {
                if (flyingTimer > 0)
                {
                    rb.AddForceY(flyingForce, ForceMode2D.Force);
                    flyingTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    if (rb.velocity.y < -glideSpeed)
                    {
                        float brake = rb.velocity.y + glideSpeed;
                        rb.AddForceY(-brake, ForceMode2D.Force);
                    }
                }
            }
        }

        jumpCDTimer -= Time.fixedDeltaTime;
    }

    public void Dash()
    {
        if (inputManager.dash && dashCDTimer < 0)
        {
            rb.AddForceX(dashForce * sprite.transform.localScale.x, ForceMode2D.Impulse);
            dashCDTimer = dashCD;
        }

        dashCDTimer -= Time.fixedDeltaTime;
    }

    private void VelocityLimit()
    {
        Vector2 velocityNormalized = rb.velocity.normalized;

        float brakeXSpeed = Mathf.Abs(rb.velocity.x) - maxXSpeed;

        if (brakeXSpeed > 0)
        {
            float brakeVelocity = velocityNormalized.x * brakeXSpeed;
            rb.AddForceX(-brakeVelocity * 5, ForceMode2D.Force);
        }

        float brakeYSpeed = rb.velocity.y - maxYSpeed;

        if (brakeYSpeed > 0)
        {
            float brakeVelocity = brakeYSpeed;
            rb.AddForceY(-brakeVelocity * 5, ForceMode2D.Force);
        }
    }
}
