using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionPanel;
    //MainPanel -> ButtonPlay
    public void LoadMenuGame()
    {
        GameManager.Instance.SetState(GameManager.StateMachine.MenuGame);
    }

    //MainPanel -> ButtonExit
    public void ExitGame()
    {
        GameManager.Instance.SetState(GameManager.StateMachine.Exit);
    }


    //MainPanel -> ButtonOptions
    public void Options()
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(true);
    }

    //OptionsPanel -> ButtonMenu
    public void Menu()
    {
        mainPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
}
