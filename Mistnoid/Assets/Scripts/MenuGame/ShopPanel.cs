using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopPanel : MonoBehaviour
{

    [SerializeField] private Upgrade[] upgrades;
    [SerializeField] private TextMeshProUGUI textSouls;

    [SerializeField] private PaddlePreviewUI paddlePreview;
    void Start()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            int index = i;
            upgrades[i].level = UpgradeManager.Instance.GetLevel(upgrades[i].type);
            upgrades[i].buttonUp.onClick.AddListener(() => LevelUp(index));
            upgrades[i].buttonDown.onClick.AddListener(() => LevelDown(index));
            upgrades[i].buttonHelp.onClick.AddListener(() => ShowHelp(index));
            upgrades[i].buttonCloseHelp.onClick.AddListener(() => HideHelp(index));
            UpdateUpgradeUI(i);
        }

        UpdateSoulsUI();
    }


    void LevelUp(int index)
    {
        Upgrade u = upgrades[index];

        int cost = GetCost(u);

        if ((ScoreManager.Instance.Souls >= cost) && (u.level < u.maxLevel))
        {
            ScoreManager.Instance.AddSouls(-cost);
            u.level++;
            UpgradeManager.Instance.SetLevel(u.type, u.level);
            UpdateUpgradeUI(index);
            UpdateSoulsUI();
            if (u.type == UpgradeType.PaddleSize)
            {
                paddlePreview.UpdatePreview();
            }
        }
    }

    void LevelDown(int index)
    {
        Upgrade u = upgrades[index];

        if (u.level > 0)
        {
            u.level--;
            int refund = GetCost(u);
            ScoreManager.Instance.AddSouls(refund);
            UpgradeManager.Instance.SetLevel(u.type, u.level);
            UpdateUpgradeUI(index);
            UpdateSoulsUI();
            if (u.type == UpgradeType.PaddleSize)
            {
                paddlePreview.UpdatePreview();
            }
        }
    }
    private int GetCost(Upgrade u)
    {
        return u.baseCost * (u.level + 1);
    }
    private void UpdateUpgradeUI(int index)
    {
        Upgrade u = upgrades[index];
        u.textLevel.text = "Nivel " + u.level;
        u.textCost.text = GetCost(u).ToString();

        switch (u.type)
        {
            case UpgradeType.StartLives:
                u.textEffect.text =  u.level.ToString();
                break;

            case UpgradeType.PowerUpChance:
                float percent = u.level * 5f;
                u.textEffect.text = "+" + percent + "%";
                break;

            case UpgradeType.PaddleHealth:
                u.textEffect.text = "HP: +" + u.level;
                break;
            default:
                break;
        }
    }

    private void UpdateSoulsUI()
    {
        textSouls.text = ScoreManager.Instance.Souls.ToString();
    }

    private void ShowHelp(int index)
    {
        upgrades[index].helpPanel.SetActive(true);
    }

    private void HideHelp(int index)
    {
        upgrades[index].helpPanel.SetActive(false);
    }

    public enum UpgradeType
    {
        PaddleSize,
        StartLives,
        PowerUpChance,
        PaddleHealth // futuro
    }

    [System.Serializable]
    public class Upgrade
    {
        public UpgradeType type;

        [Header("Stats")]
        public int level;
        public int maxLevel = 5;
        public int baseCost = 10;

        [Header("UI")]
        public TextMeshProUGUI textLevel;
        public TextMeshProUGUI textCost;
        public TextMeshProUGUI textEffect;
        public GameObject helpPanel;
        

        [Header("Buttons")]
        public Button buttonUp;
        public Button buttonDown;
        public Button buttonHelp;
        public Button buttonCloseHelp;
    }
}
