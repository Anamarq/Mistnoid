using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public Action OnDialogueEnd;

    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;

    private string[] lines;
    private int index;

    void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(string[] dialogueLines)
    {
        
        panel.SetActive(true);
        lines = dialogueLines;
        index = 0;
        ShowLine();
    }

    void Update()
    {
        //if (GameManager.Instance.GetCurrentState() == GameManager.StateMachine.Game)
        //    GameManager.Instance.SetPause(true);
        if (panel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();
        }
    }

    void ShowLine()
    {
        text.text = lines[index];
    }

    void NextLine()
    {
       
        index++;
        Debug.Log("GameManager.Instance.State " + GameManager.Instance.GetCurrentState());
        if (index >= lines.Length)
        {
            panel.SetActive(false);
            OnDialogueEnd?.Invoke();
        }
        else
        {
            ShowLine();
        }
    }
}