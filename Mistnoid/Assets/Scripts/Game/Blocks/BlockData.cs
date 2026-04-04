using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Mistnoid/Block")]
public class BlockData : ScriptableObject
{
    public string blockName;

    [Header("Stats")]
    public int maxHealth = 1;
    public bool indestructible = false;

    [Header("Rewards")]
    public int points;
    //public int soulsReward = 1;
    public float powerUpChance = 0.1f;

    [Header("Visual")]
    public Sprite[] sprite;
    public Color color = Color.white;
}