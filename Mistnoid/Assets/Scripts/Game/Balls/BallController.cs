using UnityEngine;

//Controller of each ball
[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    private Transform paddle;
    [SerializeField] private float speed = 8f;

    private Rigidbody2D rb;
    private bool isLaunched = false;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float scaleSpeed;

    //score. the multiplier increases with every block broken
    [SerializeField] private int multiplier = 1, multiplierIncrease = 1, maxMultiplier = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    void Start()
    {
        paddle = GameObject.FindWithTag("Paddle").transform;
        BallManager.Instance.RegisterBall(this);
    }

    void Update()
    {
        if (!isLaunched)
        {
            transform.position = paddle.position + offset;

            if (Input.GetButtonDown("Fire1") && PlayerController.Instance.IsPlayerAlive)
            {
                LaunchRandom();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isLaunched)
        {
            MaintainSpeed();
        }
     }

    void LaunchRandom()
    {
        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), 1f).normalized;
        Launch(dir);
    }

    void MaintainSpeed()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed * BallManager.Instance.SpeedMultiplier;
        speed += scaleSpeed * Time.fixedDeltaTime;
       // Debug.Log(speed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {

            float paddleWidth = PlayerController.Instance.PaddleColider.bounds.size.x;
            float hitPosition = transform.position.x - collision.transform.position.x;
            float normalizedHit = hitPosition / (paddleWidth / 2f);
            float bounceAngle = normalizedHit * PlayerController.Instance.MaxBounceAngle;
            float rad = bounceAngle * Mathf.Deg2Rad;
            Vector2 newDirection = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
            rb.linearVelocity = newDirection.normalized * speed;
        }
        else
        {
            Vector2 velocity = rb.linearVelocity;

            if (Mathf.Abs(velocity.y) < 0.5f)
            {
                velocity.y = velocity.y < 0 ? -1f : 1f;
            }

            rb.linearVelocity = velocity.normalized * speed;

            if (collision.gameObject.CompareTag("Block"))
            {
                Block block = collision.gameObject.GetComponent<Block>();

                if (block != null)
                {
                    block.TakeHit(this);
                }
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limit"))
        {
            isLaunched = false;
            BallManager.Instance.RemoveBall(this);
            Destroy(gameObject);
            
        }
    }

    //----------------------------------------- external calls
    public void Launch(Vector2 direction)
    {
        isLaunched = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = direction * speed;
    }


    //Score
    public void IncreaseMultiplier()
    {
        multiplier += multiplierIncrease;

        if (multiplier > maxMultiplier)
            multiplier = maxMultiplier;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }
}