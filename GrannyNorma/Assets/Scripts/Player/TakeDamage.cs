using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageAmount = 1; // The amount of damage dealt to the player
    public float damageDelay = 1f; // The delay between consecutive damage instances

    private bool isPlayerInContact = false; // To track if the player is in contact
    private Coroutine damageCoroutine; // To hold the coroutine reference

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the CharacterController2D component
        CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();

        if (player != null)
        {
            // Start the damage coroutine if the player isn't already taking damage
            if (!isPlayerInContact)
            {
                isPlayerInContact = true;
                damageCoroutine = StartCoroutine(DamagePlayer(player));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player exits the collision, stop the damage coroutine
        CharacterController2D player = collision.gameObject.GetComponent<CharacterController2D>();
        if (player != null && isPlayerInContact)
        {
            isPlayerInContact = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
        }
    }

    private IEnumerator DamagePlayer(CharacterController2D player)
    {
        while (isPlayerInContact)
        {
            // Deal damage to the player
            player.TakeDamage(damageAmount);
            Debug.Log($"Player took {damageAmount} damage from {gameObject.name}.");

            // Wait for the specified delay before dealing damage again
            yield return new WaitForSeconds(damageDelay);
        }
    }
}
