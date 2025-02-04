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
    public int maxHealth = 3; // Maximum health
    public GameObject[] heartUI; // Array for heart UI objects

    [Header("NPC Interaction")]
    public bool isSpeakingToNPC = false;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Ensure heartUI array matches maxHealth
        if (heartUI.Length != maxHealth)
        {
            Debug.LogError("The number of heart UI objects doesn't match maxHealth!");
        }

        // Initialize health in GameManager if not already set
        if (GameManager.instance != null && GameManager.instance.GetPlayerHealth() == 0)
        {
            GameManager.instance.SavePlayerHealth(maxHealth); // Set initial health
        }

        // Update heart UI based on the health from GameManager
        UpdateHeartUI();
    }

    void Update()
    {
        if (isSpeakingToNPC) return;

        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip character sprite based on movement direction
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        // Reduce health in GameManager
        int currentHealth = GameManager.instance.GetPlayerHealth() - damage;

        // Clamp current health to a minimum of 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Save the updated health to the GameManager
        GameManager.instance.SavePlayerHealth(currentHealth);

        // Update the heart UI based on the current health from GameManager
        UpdateHeartUI();

        // Handle death if health reaches 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHeartUI()
    {
        // Get the current health from GameManager
        int currentHealth = GameManager.instance.GetPlayerHealth();

        // Update the heart UI based on current health
        for (int i = 0; i < heartUI.Length; i++)
        {
            if (heartUI[i] != null)
            {
                heartUI[i].SetActive(i < currentHealth); // Show only hearts less than current health
            }
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Implement death behavior here (e.g., respawn, game over screen)
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
