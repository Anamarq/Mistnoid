using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementSlot : MonoBehaviour
{
    public AchievementType type;

    [SerializeField] private Image unlockedSprite, lockedSprite, background1, background2, moonImage;
    [SerializeField] private TextMeshProUGUI textName, textlocked, textDescription;

    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color lockedColor = Color.gray;

    public void SetState(bool unlocked)
    {
        if (unlocked)
        {
            unlockedSprite.gameObject.SetActive(true);
            lockedSprite.gameObject.SetActive(false);
            textName.color = unlockedColor;
            textlocked.gameObject.SetActive(false);
            textDescription.gameObject.SetActive(true);
            background1.color = unlockedColor;
            background2.color = unlockedColor;
            moonImage.color = unlockedColor;  

        }
        else
        {
            unlockedSprite.gameObject.SetActive(false);
            lockedSprite.gameObject.SetActive(true);
            textName.color = lockedColor;
            textDescription.color = lockedColor;
            textlocked.gameObject.SetActive(true);
            textDescription.gameObject.SetActive(false);
            background1.color = lockedColor;
            background2.color= lockedColor;
            moonImage.color = lockedColor;

        }
    }
}