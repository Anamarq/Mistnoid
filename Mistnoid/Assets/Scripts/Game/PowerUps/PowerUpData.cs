using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "Mistnoid/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName;
    public Sprite sprite;
    public float fallSpeed = 2f;
    public float duration = 10f;
    [Header("Drop Weight")]
    public int weight = 10;
    public PowerUpType type;
    public int value; //Value of the souls

    public float speedMultiplier; //power up fast ball/ slow ball
}

public enum PowerUpType
{
    ExpandPaddle,
    ShrinkPaddle,
    ExtraLife,
    MultiBall,
    SlowBall,
    FastBall,
    BarShield,
    Shot,
    Soul,
    InvincibleBall
}