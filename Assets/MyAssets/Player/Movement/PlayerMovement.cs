using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    public InputManager inputManager;

    public Rigidbody2D rb;

    [Space(5)]
    [Header("Player Sprite")]
    public GameObject sprite;

    [Space(5)]
    [Header("Ground Check")]
    [SerializeField]
    private Collider2D groundCollider;
    [SerializeField]
    private LayerMask groundLayers;

    private Vector2 groundColliderSize;

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

    [SerializeField]
    private ParticleSystem flyingParticles;
    private ParticleSystem.MainModule flyingParticlesMainModule;

    [Space(5)]
    [Header("Dash")]
    public float dashForce = 10f;
    public float dashCD = 1;
    public float dashIFrame = 0.1f;
    private float dashCDTimer;

    [SerializeField]
    private ParticleSystem dashParticles;

    private Health playerHealth;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<Health>();

        groundColliderSize = groundCollider.bounds.size;
        flyingParticlesMainModule = flyingParticles.main;

        dashParticles.Stop();

        inputManager.jumpAction.performed += delegate { Jump(); };
    }

    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        Flying();
        Down();
        VelocityLimit();
        Dash();

        jumpCDTimer -= Time.fixedDeltaTime;

        if (!inputManager.jump && isGrounded)
        {
            flyingTimer = flyingTime;
        }
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
        if (isGrounded && jumpCDTimer < 0)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
            flyingTimer = flyingTime;
            jumpCDTimer = jumpCD;
        }
    }

    public void Flying()
    {
        if (inputManager.jump)
        {
            if (flyingTimer > 0)
            {
                rb.AddForceY(flyingForce, ForceMode2D.Force);
                flyingTimer -= Time.fixedDeltaTime;

                flyingParticlesMainModule.simulationSpeed = 2;
                flyingParticles.Emit(3);
            }
            else
            {
                if (rb.velocity.y < -glideSpeed)
                {
                    float brake = rb.velocity.y + glideSpeed;
                    rb.AddForceY(-brake, ForceMode2D.Force);

                    flyingParticlesMainModule.simulationSpeed = 1;
                    flyingParticles.Emit(2);
                    return;
                }
            }
        }
    }

    public void Dash()
    {
        if (inputManager.dash && dashCDTimer < 0)
        {
            rb.AddForceX(dashForce * sprite.transform.localScale.x, ForceMode2D.Impulse);
            dashCDTimer = dashCD;
            playerHealth.ResetIFrame(dashIFrame);
            dashParticles.Emit(1);
        }

        dashCDTimer -= Time.fixedDeltaTime;
    }

    public void Down()
    {
        if (inputManager.down)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(groundCollider.transform.position, groundColliderSize, 0);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.CompareTag("Platform"))
                {
                    hits[i].enabled = false;
                    StartCoroutine(EnablePlatform(hits[i]));
                }
            }
        }
    }

    public IEnumerator EnablePlatform(Collider2D collider)
    {
        yield return new WaitForSeconds(0.25f);
        collider.enabled = true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundCollider.transform.position, groundColliderSize);
    }
}
