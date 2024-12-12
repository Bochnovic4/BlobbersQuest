using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.currentHealth = -1;
        }
        else
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.currentHealth = -1;
        }
    }
}
