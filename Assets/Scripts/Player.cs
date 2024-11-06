using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private int health;
    [SerializeField]
    private int stamina;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Player is dead");
        }
    }
}
