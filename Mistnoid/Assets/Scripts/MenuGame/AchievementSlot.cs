using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementSlot : MonoBehaviour
{
    public AchievementType type;

    [SerializeField] private Image icon, background1, background2;
    [SerializeField] private TextMeshProUGUI textName, textlocked, textDescription;
    [SerializeField] private Sprite unlockedSprite, lockedSprite;

    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;

    public void SetState(bool unlocked)
    {
        if (unlocked)
        {
            icon.sprite = unlockedSprite;
            textName.color = unlockedColor;
            textlocked.gameObject.SetActive(false);
            textDescription.gameObject.SetActive(true);
            background1.color = unlockedColor;
            background2.color = unlockedColor;

        }
        else
        {
            icon.sprite = lockedSprite;
            textName.color = lockedColor;
            textDescription.color = lockedColor;
            textlocked.gameObject.SetActive(true);
            textDescription.gameObject.SetActive(false);
            background1.color = lockedColor;
            background2.color= lockedColor;

        }
    }
}