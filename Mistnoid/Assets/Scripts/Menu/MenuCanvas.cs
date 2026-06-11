using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionPanel;
    const string LANGUAGE_KEY = "Language";

    private void Start()
    {
        LoadLanguage();
        AudioManager.Instance.PlayMenuMusic();
    }
    void LoadLanguage()
    {
        int index = PlayerPrefs.GetInt(LANGUAGE_KEY, 0);

        if (index < LocalizationSettings.AvailableLocales.Locales.Count)
        {
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales[index];
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
        SetLanguage(1);
    }

    public void English()
    {
        SetLanguage(0);
    }

    void SetLanguage(int index)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];

        PlayerPrefs.SetInt(LANGUAGE_KEY, index);
        PlayerPrefs.Save();
    }
}
