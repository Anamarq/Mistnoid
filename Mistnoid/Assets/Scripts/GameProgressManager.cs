using UnityEngine;
public enum GameProgressState
{
    Intro,
    FirstRun,
    AfterFirstDeath,
    UnlockingPowerUps,
    AllPowerUpsUnlocked,
    FirstWin,
    AchievementsPhase,
    FinalEscape
}
public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private GameProgressState currentState;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteAll(); //BORRAR
        currentState = (GameProgressState)
            PlayerPrefs.GetInt("GameState", 0);
    }
    public GameProgressState State => currentState;

    public void SetState(GameProgressState newState)
    {
        currentState = newState;
        PlayerPrefs.SetInt("GameState", (int)newState);
        PlayerPrefs.Save();
    }
}