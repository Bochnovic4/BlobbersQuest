using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerData", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public float health;
    public float stamina;
    public float strenght;
    public float defence;
    public float exp;
    public float lvl;
}
