using UnityEngine;
using UnityEngine.UI;
using static ShopPanel;

public class PaddlePreviewUI : MonoBehaviour
{
    [SerializeField] private Image paddleImage;
    [SerializeField] private Sprite[] paddleSprites;

    void Start()
    {
        UpdatePreview();
    }

    public void UpdatePreview()
    {
        int level = UpgradeManager.Instance.GetLevel(UpgradeType.PaddleSize);
        if (paddleSprites != null && level < paddleSprites.Length)
        {
            paddleImage.sprite = paddleSprites[level];
            paddleImage.SetNativeSize();
            paddleImage.rectTransform.localScale = Vector3.one * 0.5f;
        }
    }
}