using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button shopButton, achievementsButton, bookButton;
    [SerializeField] private DialogueData introDialogue;
    void Start()
    {
        CheckFirstTime();
        UpdateButtons();
    }

    void CheckFirstTime()
    {
        bool isFirstTime = PlayerPrefs.GetInt("FirstTime", 1) == 1;

        if (isFirstTime)
        {
            GameProgressManager.Instance.SetState(GameProgressState.Intro);

            StartIntroDialogue();

            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();
        }
    }

    void UpdateButtons()
    {
        var state = GameProgressManager.Instance.State;
        shopButton.interactable = false;
        achievementsButton.interactable = false;
        bookButton.interactable = false;

        // Unlock with progress
        if (state >= GameProgressState.AllPowerUpsUnlocked)
        {
            shopButton.interactable = true;
            achievementsButton.interactable = true;
        }

        if (state >= GameProgressState.AllPowerUpsUnlocked)
        {
            bookButton.interactable = true;
        }
    }

    void StartIntroDialogue()
    {
        //string[] dialogue =
        //{
        //    "Eh… hola ??",
        //    "No esperaba encontrar a nadie aquí...",
        //    "Estoy… un poco atrapado.",
        //    "Y creo que tú también.",
        //    "Si me ayudas, quizá podamos salir de este sitio.",
        //    "Prueba a pulsar JUGAR..."
        //};

        DialogueManager.Instance.StartDialogue(introDialogue);
    }
}