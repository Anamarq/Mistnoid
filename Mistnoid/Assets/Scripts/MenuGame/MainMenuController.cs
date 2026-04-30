using UnityEngine;
using UnityEngine.UI;

//Updates menu with the progress and dialogues.
//Different that CanvasMenuGame.cs, that control button functions ans menus in general.

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button shopButton, achievementsButton, bookButton;

    [SerializeField] private DialogueData introDialogue, birdsDialogue, frogDialogue;

    void Start()
    {
        CheckFirstTime();
        CheckProgress();
        UpdateButtons();
    }

    // FIRST TIME
    void CheckFirstTime()
    {
        bool isFirstTime = PlayerPrefs.GetInt("FirstTime", 1) == 1;

        if (isFirstTime)
        {
            GameProgressManager.Instance.SetState(GameProgressState.Intro);

            PlayerPrefs.SetInt("FirstTime", 0);
            PlayerPrefs.Save();
        }
    }

    // PROGRESS FLOW
    void CheckProgress()
    {
        var state = GameProgressManager.Instance.State;

        switch (state)
        {
            case GameProgressState.Intro:
                LockButtons();
                StartIntroDialogue();
                break;

            case GameProgressState.AfterFirstRun:
                LockButtons();
                StartBirdsDialogue();
                break;
            case GameProgressState.AfterBirds:
                LockButtons();
                StartFrogDialogue();
                break;

            default:
                UnlockButtons();
                break;
        }
    }

    // DIALOGUES

    void StartIntroDialogue()
    {
        DialogueManager.Instance.StartDialogue(introDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnIntroEnd;
    }

    void OnIntroEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnIntroEnd;
        GameProgressManager.Instance.SetState(GameProgressState.FirstRun);
        UpdateButtons();
    }

    void StartBirdsDialogue()
    {
        DialogueManager.Instance.StartDialogue(birdsDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnBirdsEnd;
    }
    void StartFrogDialogue()
    {
        DialogueManager.Instance.StartDialogue(frogDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnFrogEnd;
    }

    void OnBirdsEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnBirdsEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.FastBall);
        PowerUpManager.Instance.Unlock(PowerUpType.SlowBall);
        GameProgressManager.Instance.SetState(GameProgressState.AfterBirds);
        UpdateButtons();
    }

    void OnFrogEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnFrogEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.MultiBall);
        GameProgressManager.Instance.SetState(GameProgressState.AfterFrog);
        UpdateButtons();
    }

    // BUTTON CONTROL

    void LockButtons()
    {
        shopButton.interactable = false;
        achievementsButton.interactable = false;
        bookButton.interactable = false;
    }

    void UnlockButtons()
    {
        var state = GameProgressManager.Instance.State;
        if (state >= GameProgressState.AfterFrog)
        {
            shopButton.interactable = true;
          //  achievementsButton.interactable = true;
        }
        if (state >= GameProgressState.AllPowerUpsUnlocked)
        {
            bookButton.interactable = true;
        }
    }

    void UpdateButtons()
    {
      //  var state = GameProgressManager.Instance.State;
        LockButtons();
        UnlockButtons();
    }
}