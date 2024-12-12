using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public float damage;
    [SerializeField]
    private EnemyData enemyData;

    void Start()
    {
        currentHealth = enemyData.Health;
        damage = enemyData.Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth < 0) 
        {
            gameObject.SetActive(false);
        }
    }
}
