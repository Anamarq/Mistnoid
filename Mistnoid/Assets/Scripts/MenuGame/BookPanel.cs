using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookPanel : MonoBehaviour
{
    public static BookPanel Instance;

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private List<BookPage> pages;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textFragments, textAbility;
    

    [Header("Buy")]
    [SerializeField] private int fragmentCostPerPiece = 5;
    [SerializeField] private int costIncreaseUse = 20;

    private int currentPageIndex = -1; // -1 = main panel

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        pages = new List<BookPage>(GetComponentsInChildren<BookPage>(true));
    }

    void Start()
    {
        ShowMainPanel();
        UpdateFragmentsUI();
        textAbility.text = AbilityManager.Instance.MaxUses.ToString();
        // ?? ORDEN CONTROLADO
        foreach (var page in pages)
        {
            page.LoadPieces();   // primero carga piezas
        }

        foreach (var page in pages)
        {
            page.InitializeToggle(); // luego evalúa toggles
        }
    }

    //Navegation
    public void ShowMainPanel()
    {
        mainPanel.SetActive(true);
        HideAllPages();
        currentPageIndex = -1;
    }

    public void OpenBook()
    {
        if (pages.Count == 0)
            return;
        mainPanel.SetActive(false);
        currentPageIndex = 0;
        ShowPage(currentPageIndex);
    }

    public void NextPage()
    {
        if (currentPageIndex < 0)
            return;

        int next = currentPageIndex + 1;
        if (next >= pages.Count)
            return;

        if (!pages[next].IsUnlocked)
        {
            Debug.Log("Página bloqueada");
            return;
        }

        HidePage(currentPageIndex);
        currentPageIndex = next;
        ShowPage(currentPageIndex);
    }

    public void PreviousPage()
    {
        if (currentPageIndex < 0)
            return;

        HidePage(currentPageIndex);

        int prev = currentPageIndex - 1;
        if (prev < 0)
        {
            AudioManager.Instance.PlayCloseBook();
            ShowMainPanel();
        }
        else
        {
            currentPageIndex = prev;
            ShowPage(currentPageIndex);
        }
    }

    public void CloseBook()
    {
        AudioManager.Instance.PlayCloseBook();
        if (currentPageIndex >= 0)
            HidePage(currentPageIndex);
        ShowMainPanel();
    }

    void ShowPage(int index)
    {
        AudioManager.Instance.PlayPage();
        pages[index].gameObject.SetActive(true);
    }

    void HidePage(int index)
    {
        pages[index].gameObject.SetActive(false);
    }

    void HideAllPages()
    {
        foreach (var page in pages)
            page.gameObject.SetActive(false);
    }

    //Buy pieces (random)
    public void BuyCreaturePiece()
    {
        if (ScoreManager.Instance.Fragments < fragmentCostPerPiece)
        {
            Debug.Log("No tienes suficientes fragmentos.");
            return;
        }

        List<Piece> allMissingPieces = new List<Piece>();
        foreach (var page in pages)
        {
            if (!page.IsUnlocked)
                continue;
            allMissingPieces.AddRange(page.GetMissingPieces());
        }

        if (allMissingPieces.Count == 0)
        {
            Debug.Log("Ya tienes todas las piezas desbloqueadas.");
            return;
        }

        Piece selected = allMissingPieces[Random.Range(0, allMissingPieces.Count)];
        selected.SetObtained(true);
        PlayerPrefs.SetInt(selected.PieceID, 1);
        PlayerPrefs.Save();
        AudioManager.Instance.PlayFragments();
        ScoreManager.Instance.AddFragments(-fragmentCostPerPiece);
        UpdateFragmentsUI();
        selected.ParentPage.CheckCompletion();
        Debug.Log("Pieza obtenida: " + selected.PieceID);
    }
    //Buy ability use
    public void BuyAbilityUse()
    {
        if (ScoreManager.Instance.Fragments < costIncreaseUse)
            return;
        AudioManager.Instance.PlayFragments();
        ScoreManager.Instance.AddFragments(-costIncreaseUse);
        AbilityManager.Instance.AddMaxUse();

        UpdateFragmentsUI();
        textAbility.text = AbilityManager.Instance.MaxUses.ToString();
    }
    //UI 
    public void UpdateFragmentsUI()
    {
        textFragments.text = ScoreManager.Instance.Fragments.ToString();
    }

    public void DeselectAllTogglesExcept(BookPage selectedPage)
    {
        foreach (var page in pages)
        {
            if (page != selectedPage)
                page.ForceToggleOff();
        }
    }

    //Unlock pages
    public void UnlockPage(int index)
    {
        if (index >= 0 && index < pages.Count)
        {
            pages[index].UnlockPage();
        }
    }
}