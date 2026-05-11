using System.Collections;
using UnityEngine;
//using static ShopPanel;

//Controller of the player (the paddle), paddle settings
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private int initialGlobalLife = 2, actualGlobalLife; //Lifes. when player loses all the balls, he loses a life
    private float initialPaleLife, actualPaleLife; //Is the life of the pale. pale loses life when an enemy hits the paddle
    private int typePale;
    private int typeBall;
    [SerializeField] private float moveSpeed = 10f;
    private float movX; //movement in x
    Rigidbody2D paddleRB;
    Collider2D paddleColider;
    public Collider2D PaddleColider { get { return paddleColider; } }

    private bool isPlayerAlive = true;
    public bool IsPlayerAlive { get { return isPlayerAlive; } set { isPlayerAlive = value; } }
    private bool playerLoseALive = true;
    public bool PlayerLoseALive { get { return playerLoseALive; } }

    [Header("Bounce Settings")]
    [SerializeField] private float maxBounceAngle = 75f;
    public float MaxBounceAngle { get { return maxBounceAngle; } }

    

    [Header("Projectile")]
    [SerializeField] private Transform shootPointLeft;
    [SerializeField] private Transform shootPointRight;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] float fireRate = 0.25f;
    private bool canShoot = false;
    private float shootTimer;
    float fireTimer;
    public bool IsShotActive => canShoot;

    [Header("LongPaddle")]
    //[SerializeField] private Sprite[] paddleSprites;
    private int currentPaddleLevel = 0, maxPaddleLevel = 6, minPaddleLevel = 0;
    private SpriteRenderer sr;

    [Header("Bar shield")]
    [SerializeField] private GameObject bottomShield;
    private Coroutine shieldCoroutine;
    public bool IsBarActive = false;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("mas de un player");
            Destroy(this.gameObject);
        }

        paddleRB = GetComponent<Rigidbody2D>();
        if (paddleRB == null)
            Debug.LogError("<color=red>Rigidbody es nulo</color>");

        paddleColider = GetComponent<Collider2D>();
        if (paddleColider == null)
            Debug.LogError("<color=red>Collider es nulo</color>");

        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            Debug.LogError("<color=red>SpriteRendere es nulo</color>");
    }

    private void Start()
    {
        actualGlobalLife = GetInitialLives();
        isPlayerAlive = true;
        ApplyPaddleSize();
        PlayCanvas.Instance.UpdateLifes(actualGlobalLife);
        playerLoseALive = false;
    }

    private void Update()
    {
        movX = Input.GetAxisRaw("Horizontal");
        HandleShootMode();

        if (Input.GetKeyDown(KeyCode.E))
        {
            AbilityManager.Instance.TryUseAbility();
        }
    }

    private void FixedUpdate()
    {
        if (paddleRB != null)
        {
            paddleRB.linearVelocity = new Vector2(movX * moveSpeed, 0);
        }
    }

    int GetInitialLives()
    {
        int baseLives = 2;
        int extra = UpgradeManager.Instance.GetLevel(UpgradeType.StartLives);
        return baseLives + extra;
    }

    void ApplyPaddleSize()
    {
        int level = UpgradeManager.Instance.GetLevel(UpgradeType.PaddleSize);

        currentPaddleLevel = level;

        UpdatePaddleVisual();
    }

    #region powerUps
    //Shoot
    void HandleShootMode()
    {
        if (!canShoot)
            return;
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            canShoot = false;
            return;
        }
        fireTimer -= Time.deltaTime;
        if (Input.GetButton("Fire1") && fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    void Shoot()
    {
        AudioManager.Instance.PlayShoot();
        Instantiate(projectilePrefab, shootPointLeft.position, Quaternion.identity);
        Instantiate(projectilePrefab, shootPointRight.position, Quaternion.identity);
    }
    public void EnableShoot(float duration)
    {
        canShoot = true;
        shootTimer = duration;
        fireTimer = 0f;
    }

    //Expand paddle
    public void ExpandPaddle()
    {
        if (currentPaddleLevel >= maxPaddleLevel)
        {
            ScoreManager.Instance.AddScore(500);
            Debug.Log("AddScore");
            return;
        }

        currentPaddleLevel++;
        transform.localScale = Vector3.one * 1.1f;
        Invoke(nameof(ResetScale), 0.1f);
        UpdatePaddleVisual();
        
    }

    public void ShrinkPaddle()
    {
        if (currentPaddleLevel <= minPaddleLevel)
            return;
        currentPaddleLevel--;
        transform.localScale = Vector3.one * 0.9f;
        Invoke(nameof(ResetScale), 0.1f);
        UpdatePaddleVisual();
        
    }
    void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
    void UpdatePaddleVisual()
    {
        //if (paddleSprites != null && currentPaddleLevel < paddleSprites.Length)
        //{
        //    sr.sprite = paddleSprites[currentPaddleLevel];
            sr.sprite = AspectManager.Instance.GetPaddleSprite(currentPaddleLevel);
       // }
        if (paddleColider is BoxCollider2D box)
        {
            box.size = sr.sprite.bounds.size;
        }
    }

    //Extra life
    public void AddLife()
    {
        actualGlobalLife++;
        PlayCanvas.Instance.UpdateLifes(actualGlobalLife);
    }

    //Bar shield
    public void ActivateBottomShield(float duration)
    {
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);
        shieldCoroutine = StartCoroutine(BottomShieldRoutine(duration));
    }
    IEnumerator BottomShieldRoutine(float duration)
    {
        IsBarActive = true;
        bottomShield.SetActive(true);
        yield return new WaitForSeconds(duration);
        bottomShield.SetActive(false);
        IsBarActive = false;
    }
    #endregion

    void GameOver()
    {
        ScoreManager.Instance.LoseRun();
        RunTimer.Instance.StopRun();
        ResetPaddle();
        isPlayerAlive = false;
    }

    public void ResetPaddle()
    {
        currentPaddleLevel = 0;
        UpdatePaddleVisual();
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            bottomShield.SetActive(false);
        }
        canShoot = false;
    }

    public void LoseLife()
    {
        actualGlobalLife--;
        playerLoseALive = true;
        PlayCanvas.Instance.UpdateLifes(actualGlobalLife);
        BallManager.Instance.ResetSpeed();
        PowerUpPool.Instance.ResetPool();
        canShoot = false;
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            bottomShield.SetActive(false);
        }
        if (actualGlobalLife <= 0)
        {
            GameOver();
        }
    }
}