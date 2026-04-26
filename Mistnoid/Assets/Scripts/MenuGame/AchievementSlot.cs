using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementSlot : MonoBehaviour
{
    public AchievementType type;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textDescription;

    [Header("Visuals")]
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;

    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;

    public void SetState(bool unlocked)
    {
        if (unlocked)
        {
            icon.sprite = unlockedSprite;
            icon.color = unlockedColor;
            textName.color = unlockedColor;
            textDescription.color = unlockedColor;
        }
        else
        {
            icon.sprite = lockedSprite;
            icon.color = lockedColor;
            textName.color = lockedColor;
            textDescription.color = lockedColor;
        }
    }
}