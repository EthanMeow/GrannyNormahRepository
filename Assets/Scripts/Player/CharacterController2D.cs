using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Health System")]
    public int maxHealth = 3;
    public GameObject[] heartUI;

    [Header("NPC Interaction")]
    public bool isSpeakingToNPC = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Animator anim; // Animator reference

    private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
    private static readonly int JumpKey = Animator.StringToHash("Jump");
    private static readonly int GroundedKey = Animator.StringToHash("Grounded");
    private static readonly int IsFallingKey = Animator.StringToHash("IsFalling");
    private static readonly int IsJumpingKey = Animator.StringToHash("IsJumping");

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (heartUI.Length != maxHealth)
        {
            Debug.LogError("The number of heart UI objects doesn't match maxHealth!");
        }

        if (GameManager.instance != null && GameManager.instance.GetPlayerHealth() == 0)
        {
            GameManager.instance.SavePlayerHealth(maxHealth);
        }

        UpdateHeartUI();
    }

    void Update()
    {
        if (isSpeakingToNPC) return;

        HandleMovement();
        HandleJump();
        UpdateAnimator();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y); // FIXED

        anim.SetFloat(IdleSpeedKey, Mathf.Abs(moveInput));

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // FIXED
            anim.SetTrigger(JumpKey); // Jump animation trigger
            Debug.Log("Jump Triggered!"); // Debugging
        }
    }

    private void UpdateAnimator()
    {
        anim.SetBool(GroundedKey, isGrounded);

        // Check if the player is jumping
        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            anim.SetBool(IsJumpingKey, true);
        }
        else
        {
            anim.SetBool(IsJumpingKey, false);
        }

        // If the player is falling (velocity.y < 0) but not grounded, it's a jump/fall state
        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            anim.SetBool(IsFallingKey, true);
        }
        else
        {
            anim.SetBool(IsFallingKey, false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        int currentHealth = GameManager.instance.GetPlayerHealth() - damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        GameManager.instance.SavePlayerHealth(currentHealth);

        UpdateHeartUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHeartUI()
    {
        int currentHealth = GameManager.instance.GetPlayerHealth();
        for (int i = 0; i < heartUI.Length; i++)
        {
            if (heartUI[i] != null)
            {
                heartUI[i].SetActive(i < currentHealth);
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
