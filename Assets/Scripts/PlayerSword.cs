using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    public int damage;
    private CapsuleCollider swordCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        swordCollider = GetComponent<CapsuleCollider>();
        swordCollider.enabled = false;
    }

    // Update is called once per frame
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
        Debug.Log("Collision");
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
