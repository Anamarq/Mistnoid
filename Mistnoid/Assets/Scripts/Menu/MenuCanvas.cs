using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionPanel;

    private void Awake()
    {
        LoadLanguage();
    }

    private void Start()
    {
        
        AudioManager.Instance.PlayMenuMusic();
    }
    void LoadLanguage()
    {
        string code = PlayerPrefs.GetString("Language", "en");

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                break;
            }
        }
    }

    //MainPanel -> ButtonPlay
    public void LoadMenuGame()
    {
        GameManager.Instance.SetState(GameManager.StateMachine.MenuGame);
        AudioManager.Instance.PlayButton();
    }

    //MainPanel -> ButtonExit
    public void ExitGame()
    {
        AudioManager.Instance.PlayButtonBack();
        GameManager.Instance.SetState(GameManager.StateMachine.Exit);
    }


    //MainPanel -> ButtonOptions
    public void Options()
    {
        AudioManager.Instance.PlayButton();
        //  mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    //OptionsPanel -> ButtonMenu
    public void Menu()
    {
        mainPanel.SetActive(true);

        optionPanel.SetActive(false);
    }

    //OptionsPanel -> Languajes
    public void Spanish()
    {
        SetLanguage("es");
    }

    public void English()
    {
        SetLanguage("en");
    }

    void SetLanguage(string code)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == code)
            {
                LocalizationSettings.SelectedLocale = locale;
                PlayerPrefs.SetString("Language", code);
                PlayerPrefs.Save();
                break;
            }
        }
    }
}
