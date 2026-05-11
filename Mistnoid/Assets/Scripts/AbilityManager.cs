using UnityEngine;

public enum Ability
{
    None,
    Dragon,
    Frog,
    PhoenixFire,
    BlackBird,
    WhiteBird,
    Cat,
    Nimbo,
    Heart
}

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;
    private Ability currentAbility = Ability.None;
    public Ability CurrentAbility => currentAbility;

    private int maxUses, currentUses;
    public int MaxUses => maxUses;

    [SerializeField] int maxToBuy = 7;
    private bool isMaxBuy = false;
    public bool IsMaxBuy => isMaxBuy;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Load();
    }

    void Load()
    {
        currentAbility = (Ability)PlayerPrefs.GetInt("ActiveAbility", 0);
        maxUses = PlayerPrefs.GetInt("AbilityUses", 1);
        currentUses = maxUses;
        if (maxUses >= maxToBuy)
            isMaxBuy = true;
    }

    public void SetAbility(Ability ability)
    {
        currentAbility = ability;
        PlayerPrefs.SetInt("ActiveAbility", (int)ability);
        PlayerPrefs.Save();
        Debug.Log("Habilidad activa: " + ability);
    }

    public void ResetUses()
    {
        currentUses = maxUses;
    }

    public void AddMaxUse()
    {
        maxUses++;
        if (maxUses >= maxToBuy)
            isMaxBuy = true;
        PlayerPrefs.SetInt("AbilityUses", maxUses);
        PlayerPrefs.Save();
    }

    public void UpdateCanvasUses()
    {
        PlayCanvas.Instance.UpdateAbility(currentUses);
    }


    public void TryUseAbility()
    {
        if (currentUses < 0) return;

        currentUses--;
        PlayCanvas.Instance.UpdateAbility(currentUses);
        switch (currentAbility)
        {
            case Ability.Dragon:
                if (!PlayerController.Instance.IsBarActive)
                {
                    AudioManager.Instance.PlayDragon();
                    PlayerController.Instance.ActivateBottomShield(10);
                }

                break;

            case Ability.PhoenixFire:
                if (!PlayerController.Instance.IsShotActive)
                {
                    AudioManager.Instance.PlayPhoenix();
                    PlayerController.Instance.EnableShoot(5);
                }
                break;

            case Ability.Frog:
                AudioManager.Instance.PlayFrog();
                if (BallManager.Instance.GetNumBalls() > 0)
                {
                    BallManager.Instance.SpawnExtraBalls(
                        BallManager.Instance.GetBallPosition(),
                        2
                    );
                }
                break;
            case Ability.BlackBird:
                AudioManager.Instance.PlayBird();
                BallManager.Instance.ModifySpeed(2);
                break;
            case Ability.WhiteBird:
                AudioManager.Instance.PlayBirdW();
                BallManager.Instance.ModifySpeed(2);
                break;
            case Ability.Nimbo:
                AudioManager.Instance.PlayNimbo();
                PlayerController.Instance.ExpandPaddle();
                break;
            case Ability.Heart:
                AudioManager.Instance.PlayLife();
                PlayerController.Instance.AddLife();
                break;
            case Ability.Cat:
                if (!BallManager.Instance.IsInvActive)
                {
                    AudioManager.Instance.PlayCat();
                    BallManager.Instance.ActivateInvincible(2);
                }
                break;
            default:
                break;
        }
    }

}