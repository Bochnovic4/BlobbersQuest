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
        }
        else
        {
            swordCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.currentHealth -= damage + (damage * playerData.strenght/10);
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
