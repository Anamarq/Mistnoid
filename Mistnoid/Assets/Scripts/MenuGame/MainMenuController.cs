using UnityEngine;
using UnityEngine.UI;

//Updates menu with the progress and dialogues.
//Different that CanvasMenuGame.cs, that control button functions ans menus in general.

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button shopButton, achievementsButton, bookButton;

    [SerializeField] private DialogueData introDialogue, birdsDialogue, frogDialogue, heartDialogue, failLevel1Dialogue,
        dragonDialogue, phoenixDialogue, level5WarningDialogue, catDialogue;

    const string HAS_PLAYED_KEY = "HasPlayedAfterDialogue";

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
        if (CheckLevel5Event())
            return;

        var state = GameProgressManager.Instance.State;

        switch (state)
        {
            case GameProgressState.Intro:
                LockButtons();
                StartIntroDialogue();
                break;

            case GameProgressState.AfterFirstRun:
                if (HasPlayed())
                {
                    LockButtons();
                    StartBirdsDialogue();
                }
                break;
            case GameProgressState.AfterBirds:
                if (HasPlayed())
                {
                    LockButtons();
                    StartFrogDialogue();
                }
                break;
            case GameProgressState.AfterFrog:
                if (HasPlayed())
                {
                    if (!IsLevelCompleted(0))
                    {
                        LockButtons();

                        if (!(PlayerPrefs.GetInt("FailLevel1Shown", 0) == 1))
                        {
                            StartFailLevel1Dialogue();
                            PlayerPrefs.SetInt("FailLevel1Shown", 1);
                        }
                    }
                    else
                    {
                        LockButtons();
                        StartHeartDialogue();
                    }
                }
                break;

            case GameProgressState.AfterHeart:
                if (HasPlayed())
                {
                    LockButtons();
                    StartDragonDialogue();
                }
                break;
            case GameProgressState.AfterDragon:
                if (HasPlayed())
                {
                    LockButtons();
                    StartPhoenixDialogue();
                }
                break;
            case GameProgressState.AfterPhoenix:
                if (HasPlayed())
                {
                    if (!ProgressFlags.Level5Reached)
                    {
                        UnlockButtons();
                        break;
                    }

                    LockButtons();
                    if (!ProgressFlags.Level5ImpossibleSeen)
                        StartLevel5WarningThenCat();
                    else
                        StartCatDialogue();
                }
                break;
            default:
                UnlockButtons();
                break;
        }
    }

    private bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt("Level_" + levelIndex, 0) == 1;
    }
    bool HasPlayed()
    {
        return PlayerPrefs.GetInt(HAS_PLAYED_KEY, 0) == 1;
    }

    #region Dialogues
    // DIALOGUES
    //Event lvel 5 reached
    bool CheckLevel5Event()
    {
        if (ProgressFlags.Level5Reached && !ProgressFlags.Level5ImpossibleSeen)
        {
            LockButtons();
            DialogueManager.Instance.StartDialogue(level5WarningDialogue);
            DialogueManager.Instance.OnDialogueEnd += OnLevel5WarningEnd;
            ProgressFlags.Level5ImpossibleSeen = true;
            PlayerPrefs.Save();
            return true;
        }

        return false;
    }
    void OnLevel5WarningEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnLevel5WarningEnd;
        UpdateButtons();
        CheckProgress();
    }
    //1 Intro
    void StartIntroDialogue()
    {
        DialogueManager.Instance.StartDialogue(introDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnIntroEnd;
    }

    void OnIntroEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnIntroEnd;
        GameProgressManager.Instance.SetState(GameProgressState.FirstRun);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }
    //2 Birds
    void StartBirdsDialogue()
    {
        DialogueManager.Instance.StartDialogue(birdsDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnBirdsEnd;
    }
    void OnBirdsEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnBirdsEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.FastBall);
        PowerUpManager.Instance.Unlock(PowerUpType.SlowBall);
        GameProgressManager.Instance.SetState(GameProgressState.AfterBirds);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }
    //3 frog
    void StartFrogDialogue()
    {
        DialogueManager.Instance.StartDialogue(frogDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnFrogEnd;
    }

    void OnFrogEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnFrogEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.MultiBall);
        GameProgressManager.Instance.SetState(GameProgressState.AfterFrog);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }

    //If player cant reach level 2 before heart
    void StartFailLevel1Dialogue()
    {
        
        DialogueManager.Instance.StartDialogue(failLevel1Dialogue);
        DialogueManager.Instance.OnDialogueEnd += OnFailLevel1End;
    }

    void OnFailLevel1End()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnFailLevel1End;

        UpdateButtons();
    }

    //4 Heart
    void StartHeartDialogue()
    {
        DialogueManager.Instance.StartDialogue(heartDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnHeartEnd;
    }

    void OnHeartEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnHeartEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.ExtraLife);
        GameProgressManager.Instance.SetState(GameProgressState.AfterHeart);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }

    //5 Dragon
    void StartDragonDialogue()
    {
        DialogueManager.Instance.StartDialogue(dragonDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnDragonEnd;
    }

    void OnDragonEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnDragonEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.BarShield);
        GameProgressManager.Instance.SetState(GameProgressState.AfterDragon);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }
    //6 phoenix
    void StartPhoenixDialogue()
    {
        DialogueManager.Instance.StartDialogue(phoenixDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnPhoenixEnd;
    }

    void OnPhoenixEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnPhoenixEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.Shot);
        GameProgressManager.Instance.SetState(GameProgressState.AfterPhoenix);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        UpdateButtons();
    }

    //When reach level 5 and cant win
    void StartLevel5WarningThenCat()
    {
        DialogueManager.Instance.StartDialogue(level5WarningDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnWarningThenCat;
    }

    void OnWarningThenCat()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnWarningThenCat;
        ProgressFlags.Level5ImpossibleSeen = true;
        PlayerPrefs.Save();
        StartCatDialogue();
    }
    //7 Cat
    void StartCatDialogue()
    {
        DialogueManager.Instance.StartDialogue(catDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnCatEnd;
    }

    void OnCatEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnCatEnd;
        PowerUpManager.Instance.Unlock(PowerUpType.InvincibleBall);
        ProgressFlags.CatUnlocked = true;
        GameProgressManager.Instance.SetState(GameProgressState.AllPowerUpsUnlocked);
        PlayerPrefs.SetInt(HAS_PLAYED_KEY, 0);
        PlayerPrefs.Save();
        // BookPanel.Instance.UnlockPage(7);
        UpdateButtons();
    }
    #endregion
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
            shopButton.interactable = true;
        if (state >= GameProgressState.AfterPhoenix)
            bookButton.interactable = true;
        if (state >= GameProgressState.AllPowerUpsUnlocked)
            achievementsButton.interactable = true;
    }

    void UpdateButtons()
    {
      //  var state = GameProgressManager.Instance.State;
        LockButtons();
        UnlockButtons();
    }
}