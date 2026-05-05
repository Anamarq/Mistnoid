using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPage : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int pageIndex;
    [SerializeField] private bool isUnlocked = true;

    [Header("Pieces")]
    [SerializeField] private List<Piece> pieces;

    [Header("Abilities")]
    [SerializeField] private Toggle abilityToggle;
    [SerializeField] private Ability abilityType;
    [SerializeField] private Image imgToggle;
    [SerializeField] private Sprite spriteSelected, spriteUnselected;

    public bool IsUnlocked => isUnlocked;

    void Awake()
    {

    }
    void Start()
    {
        abilityToggle.onValueChanged.AddListener(OnToggleChanged);
        //abilityToggle.interactable = false;
    }

    public void LoadPieces()
    {
        foreach (var piece in pieces)
        {
            bool obtained = PlayerPrefs.GetInt(piece.PieceID, 0) == 1;
            piece.SetObtained(obtained);
        }
    }

    void OnToggleChanged(bool state)
    {
        if (state)
        {
            AudioManager.Instance.PlayAbility();
            AbilityManager.Instance.SetAbility(abilityType);
            BookPanel.Instance.DeselectAllTogglesExcept(this);
            imgToggle.sprite = spriteSelected;
        }
        //else
        //    AudioManager.Instance.PlayButtonBack();
    }

    public List<Piece> GetMissingPieces()
    {
        List<Piece> missing = new List<Piece>();
        foreach (var piece in pieces)
        {
            bool obtained = PlayerPrefs.GetInt(piece.PieceID, 0) == 1;
            if (!obtained)
                missing.Add(piece);
        }
        return missing;
    }

    public void CheckCompletion()
    {
        foreach (var piece in pieces)
        {
            if (!piece.gameObject.activeSelf)
                return;
        }

        Debug.Log("Página " + pageIndex + " completada!");
        abilityToggle.interactable = true;
        BookPanel.Instance.canBuyAbUse = true;
        InitializeToggle();
    }

    public void UnlockPage()
    {
        isUnlocked = true;
    }

    //Abilities
    public void ForceToggleOff()
    {
        abilityToggle.isOn = false;
        imgToggle.sprite = spriteUnselected;
    }

    public void InitializeToggle()
    {
        bool completed = IsPageCompleted();
        Debug.Log("Completed " + completed);
        abilityToggle.interactable = completed;

        if (!completed)
        {
            abilityToggle.SetIsOnWithoutNotify(false);
            imgToggle.sprite = spriteUnselected;
            return;
        }

        var activeAbility = AbilityManager.Instance.CurrentAbility;
        Debug.Log("activeAbility " + activeAbility + " abilityType " + abilityType);
        if (activeAbility == abilityType)
        {
            abilityToggle.SetIsOnWithoutNotify(true);
            imgToggle.sprite = spriteSelected;

        }
        else
        {
            abilityToggle.SetIsOnWithoutNotify(false);
            imgToggle.sprite = spriteUnselected;
        }
    }

    public bool IsPageCompleted()
    {
        foreach (var piece in pieces)
        {
            if (!piece.Obtained)
                return false;
        }
        return true;
    }
}