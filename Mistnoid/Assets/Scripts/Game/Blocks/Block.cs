using UnityEngine;
using static ShopPanel;

/// <summary>
/// Controls Block data
/// </summary>
public class Block : MonoBehaviour
{
    [SerializeField] private BlockData data;

    private int currentHealth;
    private SpriteRenderer sr;
    private float currentPowerUpChance;
    Collider2D col;
    private bool isDestroy = false;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("<color=red>SpriteRenderer es nulo</color>");
        currentHealth = data.maxHealth;

        if (data.sprite != null && data.sprite.Length > 0)
            sr.sprite = data.sprite[data.sprite.Length - 1];


        currentPowerUpChance = data.powerUpChance;
        currentPowerUpChance += UpgradeManager.Instance.GetLevel(UpgradeType.PowerUpChance) * 0.05f;
    }

    //Destroy block and add points to the score manager
    void DestroyBlock(BallController ball)
    {
        if (!isDestroy)
        {
            isDestroy = true;
            if (ball != null)
            {
                int pointsEarned = data.points * ball.GetMultiplier();
                Debug.Log("Multiplier: " + ball.GetMultiplier());
                ScoreManager.Instance.AddScore(pointsEarned);
                ball.IncreaseMultiplier();
            }
            else
            {
                ScoreManager.Instance.AddScore(data.points);
            }
            if (Random.value < currentPowerUpChance)
            {
                Debug.Log("currentPowerUpChance " + currentPowerUpChance);
                PowerUpManager.Instance.TrySpawnPowerUp(transform.position);
            }
            LevelController.Instance.BlockDestroyed();
            Destroy(gameObject);
        }
    }
    //----------------------------------------- external calls
    public BlockData GetBlockData()
    {
        return data;
    }
    public bool IsIndestructible()
    {
        return data.indestructible;
    }
    public void TakeHit(BallController ball = null)
    {
        
        if (data.indestructible)
            return;
        currentHealth--;

        if (currentHealth <= 0)
        {
            DestroyBlock(ball);
            AudioManager.Instance.PlayBlockBreak();
        }
        else
        {
            sr.sprite = data.sprite[currentHealth - 1];
            AudioManager.Instance.PlayBlock2();
        }

    }
    public void TakeHitInvincible(BallController ball = null)
    {
        AudioManager.Instance.PlayBlockMetal();
        if (data.indestructible)
            return;
        currentHealth = 0;
        DestroyBlock(ball);

    }

    public void SetBlockData(BlockData newData)
    {
        data = newData;
    }

    //Power up invencible
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!col.isTrigger) return;
        if (collision.CompareTag("Ball"))
        {
            BallController ball = collision.GetComponent<BallController>();
            if (ball != null)
            {
                TakeHitInvincible(ball);
            }
        }
    }
    public void SetTriggerMode(bool state)
    {
        col.isTrigger = state;
    }
}