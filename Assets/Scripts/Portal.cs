using UnityEngine;

public class Portal2D : MonoBehaviour
{
    public Transform linkedPortal; // The target portal to teleport to
    private static bool isTeleporting = false; // Global cooldown shared across all portals

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure the portal is linked, the collision is with the player, and teleport isn't already happening
        if (isTeleporting || linkedPortal == null) return;

        if (other.CompareTag("Player")) // Make sure the player is tagged "Player"
        {
            Teleport(other.gameObject);
        }
    }

    private void Teleport(GameObject player)
    {
        isTeleporting = true;

        // Teleport the player to the linked portal's position
        player.transform.position = linkedPortal.position;

        // Start a global cooldown to prevent immediate re-teleportation
        StartCoroutine(ResetTeleportCooldown());
    }

    private System.Collections.IEnumerator ResetTeleportCooldown()
    {
        yield return new WaitForSeconds(0.2f); // Cooldown duration
        isTeleporting = false;
    }
}
