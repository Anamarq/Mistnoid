using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private AchievementSlot[] slots;
    //Dialogue first time
    const string ACH_DIALOGUE_SHOWN = "AchDialogueShown";
    [SerializeField] private DialogueData AchDialogue;

    void OnEnable()
    {
        CheckFirstTimeOpen();
        Refresh();
    }
    #region Dialogue
    //Dialogue first time
    void CheckFirstTimeOpen()
    {
        if (PlayerPrefs.GetInt(ACH_DIALOGUE_SHOWN, 0) == 1)
            return;
        GameManager.Instance.SetPause(true);
        GameManager.Instance.IsDialogue = true;

        DialogueManager.Instance.StartDialogue(AchDialogue);
        DialogueManager.Instance.OnDialogueEnd += OnAchDialogueEnd;

        PlayerPrefs.SetInt(ACH_DIALOGUE_SHOWN, 1);
        PlayerPrefs.Save();
    }
    void OnAchDialogueEnd()
    {
        DialogueManager.Instance.OnDialogueEnd -= OnAchDialogueEnd;

        GameManager.Instance.SetPause(false);
        GameManager.Instance.IsDialogue = false;
    }
    #endregion
    void Refresh()
    {
        foreach (var slot in slots)
        {
            bool unlocked = AchievementManager.Instance.IsUnlocked(slot.type);

            slot.SetState(unlocked);
        }
    }
}