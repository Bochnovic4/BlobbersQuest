using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (player != null)
        {
            player.currentHealth = -1;
        }
        if (enemy != null)
        {
            enemy.currentHealth = -1;
        }
    }
}
