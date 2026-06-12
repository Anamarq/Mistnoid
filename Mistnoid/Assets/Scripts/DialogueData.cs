using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "Mistnoid/Dialogue")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    public string characterName;
    public Sprite characterSprite;

    public LocalizedString text;
}