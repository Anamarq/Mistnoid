using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;

//menu in game (pause, gameover, win)
public class PlayCanvas : MonoBehaviour
{
    public static PlayCanvas Instance;
    [SerializeField] private GameObject panelPause, panelGameOver, panelWin, panelLevel;
    [SerializeField] private TextMeshProUGUI textPoints, textLifes, textSouls, textTimer, textWin, textGameOver, textPhase;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("mas de un PlayCanvas");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PanelPause(false);
        PanelGameOver(false);
        PanelWin(false);
        AudioManager.Instance.PlayGameMusic();
    }

    //----------------------------------------- class methods
    //Pause panel
    //PausePanel -> ButtonPlay
    public void ButtonPlay()
    {
        PanelPause(false);
    }

    //PausePanel -> ButtonExit
    public void ButtonExit()
    {
        PanelPause(false);
        GameManager.Instance.SetState(GameManager.StateMachine.MenuGame);
    }

    //PanelNextLevel -> ButtonNextLevel
    public void ButtonNextLevel()
    {
        panelLevel.SetActive(false);
        GameManager.Instance.SetPause(false);
        BallManager.Instance.ResetBallsNextLevel();
    }

    private void PanelGameOver(bool state)
    {
        panelGameOver.SetActive(state);
        GameManager.Instance.SetPause(state);
    }

    private void PanelWin(bool state)
    {
        panelWin.SetActive(state);
        GameManager.Instance.SetPause(state);
    }
    //----------------------------------------- external calls


    public void PanelPause(bool state)
    {
        panelPause.SetActive(state);
        GameManager.Instance.SetPause(state);
        if(state)
        {
            AudioManager.Instance.PlayButton();
            AudioManager.Instance.StopMusic();
        }
        else
        {
            AudioManager.Instance.PlayButtonBack();
            AudioManager.Instance.PlayGameMusic();
        }
    }

    public void PanelLevel(bool state)
    {
        panelLevel.SetActive(state);
    }
    public void UpdatePhase(int phase)
    {
        textPhase.text = phase.ToString();
    }
    public void UpdatePoints(int points)
    {
        textPoints.text = points.ToString();
    }

    public void UpdateLifes(int lifes)
    {
        textLifes.text = lifes.ToString();
    }

    public void UpdateSouls(int souls)
    {
        textSouls.text = souls.ToString();
    }
    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        textTimer.text = $"{minutes:00}:{seconds:00}";
    }


    public void ShowWin(int finalScore, int baseScore, float time, int timeBonus, int fragments, int souls)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        textWin.text =
            "LEVEL CLEAR\n\n" +
            "Puntos: " + baseScore + "\n" +
            "Tiempo: " + $"{minutes:00}:{seconds:00}" + "\n" +
            "Bonus de tiempo: +" + timeBonus + "\n\n" +
            "Puntuacion final: " + finalScore + "\n" +
            "Fragmentos de esencia: " + fragments + "\n" +
            "Almas: " + souls;

        PanelWin(true);
        AudioManager.Instance.PlayWin();
    }

    public void ShowGameOver(int souls, int score, int fragments)
    {
        textGameOver.text =
            "GAME OVER\n\n" +
            "Puntos: " + score + "\n" +
            "Fragmentos de esencia: " + fragments +"\n" +
            "Almas: " + souls;

        PanelGameOver(true);
        AudioManager.Instance.PlayLose();
    }
}
