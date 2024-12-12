using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private CapsuleCollider m_CapsuleCollider;
    [SerializeField]
    private int weaponDamage;

    private void Start()
    {
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        DisableCollider();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (!playerController.isBlocking)
            {
                player.TakeDamage(weaponDamage);
                DisableCollider();
            }
        }
    }

    public void EnableCollider()
    {
        m_CapsuleCollider.enabled = true;
    }

    public void DisableCollider()
    {
        m_CapsuleCollider.enabled = false;
    }
}
