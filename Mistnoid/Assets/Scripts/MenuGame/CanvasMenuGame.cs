using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasMenuGame : MonoBehaviour
{
    public static CanvasMenuGame Instance;
    [SerializeField] private GameObject mainPanel, bookPanel, shopPanel;
    [SerializeField]
    private TextMeshProUGUI soulsText, pointTe, timeText, fragmentText;

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        soulsText.text = ScoreManager.Instance.Souls.ToString();
        fragmentText.text = ScoreManager.Instance.Fragments.ToString();
        pointTe.text = "Mejor puntuación: " + ScoreManager.Instance.HighScore.ToString();
        float _time = ScoreManager.Instance.BestTime;
        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        timeText.text = "Mejor tiempo: " + $"{minutes:00}:{seconds:00}";

        AudioManager.Instance.PlayMenuMusic();
    }

    public void ChangeTextSouls()
    {
        soulsText.text = ScoreManager.Instance.Souls.ToString();
    }
    public void ChangeTextFragments()
    {
        fragmentText.text = ScoreManager.Instance.Fragments.ToString();
    }
    public void ChangeTextTime()
    {
        float _time = ScoreManager.Instance.BestTime;
        int minutes = Mathf.FloorToInt(_time / 60);
        int seconds = Mathf.FloorToInt(_time % 60);
        timeText.text = "Mejor tiempo: " + $"{minutes:00}:{seconds:00}";
    }
    //MainPanel -> ButtonPlay
    public void LoadGame()
    {
        AudioManager.Instance.PlayButton();
        GameManager.Instance.SetState(GameManager.StateMachine.Game);
    }

    //MainPanel -> ButtonMainMenu
    public void ReturnToMenu()
    {
        AudioManager.Instance.PlayButtonBack();
        GameManager.Instance.SetState(GameManager.StateMachine.Menu);
    }

    //MainPanel -> Buttonbook
    public void LoadBook()
    {
        AudioManager.Instance.PlayButton();
        mainPanel.SetActive(false);
        bookPanel.SetActive(true);
        //pages[actualPage].SetActive(false);
        //actualPage = 0;
        //pages[actualPage].SetActive(true);
    }

    

    //MainPanel -> ButtonShop
    public void LoadShop()
    {
        AudioManager.Instance.PlayButton();
        mainPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    //ShopPanel -> ButtonReturn
    public void ReturnShop()
    {
        AudioManager.Instance.PlayButtonBack();
        mainPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    //BookPanel -> ButtonReturn
    public void ReturnBook()
    {
        AudioManager.Instance.PlayButtonBack();
        mainPanel.SetActive(true);
        bookPanel.SetActive(false);
    }
}
