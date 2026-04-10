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
                AudioManager.Instance.PlaySouls();
                ScoreManager.Instance.AddSouls(data.value);
                break;
            case PowerUpType.ExpandPaddle:
                Debug.Log("Expand paddle");
                AudioManager.Instance.PlayWopi();
                PlayerController.Instance.ExpandPaddle();
                break;

            case PowerUpType.ShrinkPaddle:
                Debug.Log("SHIRNK paddle");
                AudioManager.Instance.PlayWopiSad();
                PlayerController.Instance.ShrinkPaddle();
                break;

            case PowerUpType.ExtraLife:
                Debug.Log("Extra life");
                AudioManager.Instance.PlayLife();
                PlayerController.Instance.AddLife();
                break;

            case PowerUpType.MultiBall:
                Debug.Log("MultiBall " + BallManager.Instance.GetNumBalls());
                AudioManager.Instance.PlayFrog();
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
                AudioManager.Instance.PlayBirdW();
                BallManager.Instance.ModifySpeed(data.speedMultiplier);
                break;

            case PowerUpType.SlowBall:
                Debug.Log("Slow");
                AudioManager.Instance.PlayBird();
                BallManager.Instance.ModifySpeed(data.speedMultiplier);
                break;

            case PowerUpType.BarShield:
                AudioManager.Instance.PlayDragon();
                PlayerController.Instance.ActivateBottomShield(data.duration);
                Debug.Log("Shield");
                break;
            case PowerUpType.Shot:
                AudioManager.Instance.PlayPhoenix();
                PlayerController.Instance.EnableShoot(data.duration);
                break;
            case PowerUpType.InvincibleBall:
                AudioManager.Instance.PlayCat();
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