using UnityEngine;
using UnityEngine.UI;
using static ShopPanel;

public class PaddlePreviewUI : MonoBehaviour
{
    [SerializeField] private Image paddleImage;

    void Start()
    {
        UpdatePreview();
    }

    public void UpdatePreview()
    {
        int level = UpgradeManager.Instance.GetLevel(UpgradeType.PaddleSize);

        Sprite sprite = AspectManager.Instance.GetPaddleSprite(level);

        paddleImage.sprite = sprite;
        paddleImage.SetNativeSize();
        paddleImage.rectTransform.localScale = Vector3.one * 0.5f;
    }
}