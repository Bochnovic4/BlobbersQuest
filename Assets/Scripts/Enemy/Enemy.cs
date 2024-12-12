using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    [SerializeField]
    private EnemyData enemyData;
    [SerializeField]
    private Player player;
    public Vector3 spawnPosition;
    public bool isBoss = false;

    void Start()
    {
        currentHealth = enemyData.Health;
        spawnPosition = transform.position;
    }

    void Update()
    {
        if(currentHealth < 0) 
        {
            if (isBoss)
            {
                GameManager.Instance.OnBossDeath();
            }
            player.exp += enemyData.exp;
            Destroy(gameObject);
        }
    }
}
