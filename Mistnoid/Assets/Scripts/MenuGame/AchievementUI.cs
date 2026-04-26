using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private AchievementSlot[] slots;

    void OnEnable()
    {
        Refresh();
    }

    void Refresh()
    {
        foreach (var slot in slots)
        {
            bool unlocked = AchievementManager.Instance.IsUnlocked(slot.type);

            slot.SetState(unlocked);
        }
    }
}