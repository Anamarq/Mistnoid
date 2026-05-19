using UnityEngine;
using UnityEngine.UI;

public class SelectPaddle : MonoBehaviour
{
    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private Button[] buttons;
    [SerializeField] private int cost = 50;
    [SerializeField] private ShopPanel shopPanel;

    void Start()
    {
        SetupButtons();
        RefreshButtons();
        ApplyToPrefab();
    }

    private void OnEnable()
    {
        RefreshSelection();
    }

    void SetupButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => SelectPaddleAspect(index));
        }
    }

    public void SelectPaddleAspect(int index)
    {
        AspectManager.Instance.SetPaddleAspect(index);

        ApplyToPrefab();
        RefreshSelection();
    }

    void ApplyToPrefab()
    {
        var sr = paddlePrefab.GetComponent<SpriteRenderer>();

        int level = UpgradeManager.Instance.GetLevel(UpgradeType.PaddleSize);

        Sprite sprite = AspectManager.Instance.GetPaddleSprite(level);

        sr.sprite = sprite;
        shopPanel.PreviewPaddleImage.sprite = sprite;
    }

    public void BuyRandom()
    {
        if (ScoreManager.Instance.Souls < cost)
        {
            AudioManager.Instance.PlayWrong();
            return;
        }

        int index = AspectManager.Instance.GetRandomLockedPaddle();

        if (index == -1)
        {
            Debug.Log("Todo desbloqueado");
            AudioManager.Instance.PlayWrong();
            return;
        }

        ScoreManager.Instance.AddSouls(-cost);
        AspectManager.Instance.UnlockPaddle(index);

        RefreshButtons();
    }

    void RefreshButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable =
                AspectManager.Instance.IsPaddleUnlocked(i);
        }
    }
    void RefreshSelection()
    {
        int selected = AspectManager.Instance.GetSelectedPaddle();

        for (int i = 0; i < buttons.Length; i++)
        {
            var colors = buttons[i].colors;
            if (i == selected)
                colors.normalColor = Color.green;
            else
                colors.normalColor = Color.white;
            buttons[i].colors = colors;
        }
    }
}