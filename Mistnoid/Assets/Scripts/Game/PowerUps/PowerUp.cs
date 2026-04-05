using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private PowerUpData data;
    private SpriteRenderer sr;

    public void Init(PowerUpData powerUpData)
    {
        data = powerUpData;
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();
        sr.sprite = data.sprite;
    }

    void Update()
    {
        transform.Translate(Vector2.down * data.fallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Paddle"))
        {
            ApplyEffect();
            ReturnToPool();
        }
        if (collision.CompareTag("Limit"))
            ReturnToPool();
    }


    void ApplyEffect()
    {
        switch (data.type)
        {
            case PowerUpType.Soul:
                ScoreManager.Instance.AddSouls(data.value);
                break;
            case PowerUpType.ExpandPaddle:
                Debug.Log("Expand paddle");
                PlayerController.Instance.ExpandPaddle();
                break;

            case PowerUpType.ShrinkPaddle:
                Debug.Log("SHIRNK paddle");
                PlayerController.Instance.ShrinkPaddle();
                break;

            case PowerUpType.ExtraLife:
                Debug.Log("Extra life");
                PlayerController.Instance.AddLife();
                break;

            case PowerUpType.MultiBall:
                Debug.Log("MultiBall " + BallManager.Instance.GetNumBalls());
                if (BallManager.Instance.GetNumBalls() > 0)
                {
                    BallManager.Instance.SpawnExtraBalls(
                        BallManager.Instance.GetBallPosition(),
                        2
                    );
                }

                break;
            case PowerUpType.FastBall:
                Debug.Log("Fast");
                BallManager.Instance.ModifySpeed(data.speedMultiplier);
                break;

            case PowerUpType.SlowBall:
                Debug.Log("Slow");
                BallManager.Instance.ModifySpeed(data.speedMultiplier);
                break;

            case PowerUpType.BarShield:
                PlayerController.Instance.ActivateBottomShield(data.duration);
                Debug.Log("Shield");
                break;
            case PowerUpType.Shot:
                PlayerController.Instance.EnableShoot(data.duration);
                break;
            case PowerUpType.InvincibleBall:
                BallManager.Instance.ActivateInvincible(data.duration);
                break;
            default:
                break;
        }
    }

    void ReturnToPool()
    {
        PowerUpPool.Instance.ReturnPowerUp(gameObject);
    }
}