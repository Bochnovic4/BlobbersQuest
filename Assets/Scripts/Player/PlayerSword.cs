using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    private CapsuleCollider swordCollider;
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private float damage;

    private bool hasHit;

    void Start()
    {
        swordCollider = GetComponent<CapsuleCollider>();
        swordCollider.enabled = false;
    }

    void Update()
    {
        if (player.isPlayingAttackAnimation)
        {
            swordCollider.enabled = true;

            if (!hasHit)
            {
                hasHit = false;
            }
        }
        else
        {
            swordCollider.enabled = false;
            hasHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            float finalDamage = damage * (1 + playerData.strenght / 100);
            enemy.currentHealth -= finalDamage;
            hasHit = true;
            DisableCollider();
        }
    }

    public void EnableCollider()
    {
        swordCollider.enabled = true;
    }

    public void DisableCollider()
    {
        swordCollider.enabled = false;
    }
}
