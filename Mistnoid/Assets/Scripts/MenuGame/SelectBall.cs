using UnityEngine;
using UnityEngine.UI;

public class SelectBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
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
            buttons[i].onClick.AddListener(() => SelectBallAspect(index));
        }
    }
    public void SelectBallAspect(int index)
    {
        AspectManager.Instance.SetBallAspect(index);

        ApplyToPrefab();
        RefreshSelection();
    }

    void ApplyToPrefab()
    {
        var sr = ballPrefab.GetComponent<SpriteRenderer>();

        sr.sprite = AspectManager.Instance.GetBallSprite();
        shopPanel.PreviewBallImage.sprite = AspectManager.Instance.GetBallSprite(); 
    }

    public void BuyRandom()
    {
        if (ScoreManager.Instance.Souls < cost)
        {
            AudioManager.Instance.PlayWrong();
            return;
        }

        int index = AspectManager.Instance.GetRandomLockedBall();
        if (index == -1)
        {
            Debug.Log("Todo desbloqueado");
            AudioManager.Instance.PlayWrong();
            return;
        }
        ScoreManager.Instance.AddSouls(-cost);
        AspectManager.Instance.UnlockBall(index);
        RefreshButtons();
    }

    void RefreshButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable =
                AspectManager.Instance.IsBallUnlocked(i);
        }
    }
    void RefreshSelection()
    {
        int selected = AspectManager.Instance.GetBallAspect();

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