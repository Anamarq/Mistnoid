using UnityEngine;


// controls the score in the game, the points
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private int score, highScore, souls, fragments;
     public int Fragments { get { return fragments; } }
     public int Souls { get { return souls; } }
     public int HighScore { get { return highScore; } }
    private float bestTime;
    public float BestTime { get { return bestTime; } }


    // Ajustable (balance)
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

    //void AddTimeBonus(float time)
    //{
    //    int bonus = Mathf.Max(0, pointsTime - Mathf.RoundToInt(time * 50));
    //    AddScore(bonus);
    //}
    //int CalculateEssence(int finalScore)
    //{
    //    return finalScore / pointsPerFragment;
    //}
    // External calls
    public void AddScore(int points)
    {
        score += points;
        PlayCanvas.Instance?.UpdatePoints(score);
    }

    public void AddSouls(int _souls)
    {
        souls += _souls;   
        PlayCanvas.Instance?.UpdateSouls(souls);
    }

    public void AddFragments(int _fragments)
    {
        fragments += _fragments;

    }

    public void LoseRun()
    {
        // Guardar almas
        int prevSouls = PlayerPrefs.GetInt("Souls", 0);
        PlayerPrefs.SetInt("Souls", souls + prevSouls);

        int fragments = score / pointsPerFragment;
        // Guardar fragmentos
        int prevFragments = PlayerPrefs.GetInt("Fragments", 0);
        PlayerPrefs.SetInt("Fragments", prevFragments + fragments);

        PlayerPrefs.Save();

        // UI GAME OVER
        PlayCanvas.Instance.ShowGameOver(souls, score, fragments);

        // Reset run
        score = 0;
        souls = 0;
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
        int fragments = finalScore / pointsPerFragment;

        // Guardar almas
        int prevSouls = PlayerPrefs.GetInt("Souls", 0);
        PlayerPrefs.SetInt("Souls", souls + prevSouls);

        // Guardar fragmentos
        int prevFragments = PlayerPrefs.GetInt("Fragments", 0);
        PlayerPrefs.SetInt("Fragments", prevFragments + fragments);

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
            fragments,
            souls
        );

        // Reset run
        score = 0;
        souls = 0;
    }

}