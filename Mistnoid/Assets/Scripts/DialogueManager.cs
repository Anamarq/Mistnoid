using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Action OnDialogueEnd;

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image portraitImage;

    private DialogueLine[] lines;
    private int index;

    void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(DialogueData data)
    {
        panel.SetActive(true);
        lines = data.lines;
        index = 0;
        ShowLine();
    }

    void Update()
    {
        if (panel.activeSelf &&
            (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")))
        {
            NextLine();
        }
    }

    void ShowLine()
    {
        var line = lines[index];

        nameText.text = line.characterName;
        portraitImage.sprite = line.characterSprite;

        // ?? CLAVE: pedir texto localizado
        line.text.GetLocalizedStringAsync().Completed += handle =>
        {
            text.text = handle.Result;
        };
    }

    void NextLine()
    {
        index++;

        if (index >= lines.Length)
        {
            panel.SetActive(false);
            OnDialogueEnd?.Invoke();
        }
        else
        {
            AudioManager.Instance.PlayClickDialogue();
            ShowLine();
        }
    }
}