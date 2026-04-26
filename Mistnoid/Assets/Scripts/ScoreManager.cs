using UnityEngine;


// controls the score in the game, the points
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score, highScore, souls, fragments, soulsRun = 0;
     public int Fragments { get { return fragments; } }
     public int Souls { get { return souls; } }
     public int HighScore { get { return highScore; } }
    private float bestTime;
    public float BestTime { get { return bestTime; } }


    [Header("Limits")]
    [SerializeField] private int maxSouls = 9999;
    [SerializeField] private int maxFragments = 9999;
    [SerializeField] private int pointsPerFragment = 100;
    [SerializeField] private int pointsTime = 5000;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        score = PlayerPrefs.GetInt("Score", 0);
        souls = PlayerPrefs.GetInt("Souls", 0);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        fragments = PlayerPrefs.GetInt("Fragments", 0);
    }
    void Start()
    {
        PlayCanvas.Instance?.UpdatePoints(score);
        PlayCanvas.Instance?.UpdateSouls(souls);
    }

    // External calls
    public void AddScore(int points)
    {
        score += points;
        PlayCanvas.Instance?.UpdatePoints(score);
    }

    public void AddSouls(int _souls)
    {
        AudioManager.Instance.PlaySouls();
        souls += _souls;
        soulsRun += souls;
        souls = Mathf.Clamp(souls, 0, maxSouls);

        PlayCanvas.Instance?.UpdateSouls(soulsRun);
        CanvasMenuGame.Instance.ChangeTextSouls();
        PlayerPrefs.SetInt("Souls", souls);
        PlayerPrefs.Save();
    }

    public void AddFragments(int _fragments)
    {
        fragments += _fragments;
        fragments = Mathf.Clamp(fragments, 0, maxFragments);
        CanvasMenuGame.Instance.ChangeTextFragments();
        PlayerPrefs.SetInt("Fragments", fragments);
        PlayerPrefs.Save();
    }

    public void StartRun()
    {
        score = 0;
        soulsRun = 0;
    }

    public void LoseRun()
    {
        int _fragments = score / pointsPerFragment;
        // Guardar fragmentos
        AddFragments(_fragments);

        // UI GAME OVER
        PlayCanvas.Instance.ShowGameOver(soulsRun, score, _fragments);

    }
    public void WinRun()
    {
        float time = RunTimer.Instance.RunTime;

        // Guardar mejor tiempo
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
        if (time < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", time);
        }

        // BONUS DE TIEMPO
        int timeBonus = Mathf.Max(0, pointsTime - Mathf.RoundToInt(time * 50));
        int finalScore = score + timeBonus;

        // Calcular fragmentos
        int _fragments = finalScore / pointsPerFragment;


        // Guardar fragmentos
        AddFragments(_fragments);
        

        // Guardar highscore
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
        }

        PlayerPrefs.Save();

        // UI WIN
        PlayCanvas.Instance.ShowWin(
            finalScore,
            score,
            time,
            timeBonus,
            _fragments,
            soulsRun
        );

        CheckAchivements(finalScore);
    }

    private void CheckAchivements(int score)
    {
        float time = RunTimer.Instance.RunTime;
        //Achivements:
        // Without lose live
        if (!PlayerController.Instance.PlayerLoseALive)
        {
            AchievementManager.Instance.Unlock(AchievementType.WinNoDamage);
        }

        // Fast time
        if (time < 60f)
        {
            AchievementManager.Instance.Unlock(AchievementType.WinFast);
        }

        // With ability
        if (AbilityManager.Instance.CurrentAbility != Ability.None)
        {
            AchievementManager.Instance.Unlock(AchievementType.WinWithAbility);
        }
        //Score
        if (score >= 10000)
            AchievementManager.Instance.Unlock(AchievementType.Score10000);

        //Aspect master
        int ball = AspectManager.Instance.GetBallAspect();
        int paddle = AspectManager.Instance.GetSelectedPaddle();
        if (ball == paddle)
        {
            if (ball == 0)
                AchievementManager.Instance.Unlock(AchievementType.BlueMaster);

            if (ball == 1)
                AchievementManager.Instance.Unlock(AchievementType.RedMaster);
        }
    }

}