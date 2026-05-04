using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum StateMachine
    {
        Init,
        Menu,
        MenuGame,
        Game,
        Pause,
        Exit
    }

    [SerializeField] private StateMachine currentState;
    //public StateMachine CurrentState { get { return currentState; } set { currentState = value; } }

    [SerializeField] private StateMachine lastState;

    public bool IsDialogue = false;

    #region MonoBehaviour
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this.gameObject);
        }


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (currentState == StateMachine.Init)
            SetState(StateMachine.Menu);

    }


    void Update()
    {
       if(currentState == StateMachine.Game)
       {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !IsDialogue)
            {
                PlayCanvas.Instance.PanelPause(true);
            }
       } else if(currentState == StateMachine.Pause && !IsDialogue)
       {
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && !IsDialogue)
            {
                PlayCanvas.Instance.PanelPause(false);
            }
       }

    }
    #endregion

    #region States

    private void OnChangeState()
    {
        switch (currentState)
        {
            case StateMachine.Init: Debug.Log("Init"); break;

            case StateMachine.Menu:
                SceneManager.LoadScene("Menu");
                break;
            case StateMachine.MenuGame:
                SceneManager.LoadScene("MenuGame");
                break;
            case StateMachine.Game:
                SceneManager.LoadScene("Game");
                break;
            //case StateMachine.Pause:
            //    Debug.Log("PAUSE");
            //    break;
            case StateMachine.Exit:
                ExitState();
                break;
            default:
                break;
        }
    }


    private void ExitState()
    {


#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
    #endregion
    //----------------------------------------- external calls
    #region ExternalCalls

    public void SetState(StateMachine newState)
    {
        Debug.Log("Currnet: " + currentState + " new " + newState);
        if (currentState == newState)
            return;

        lastState = currentState;
        currentState = newState;

        if(lastState != StateMachine.Pause)
            OnChangeState();
    }
    public StateMachine GetCurrentState()
    {
        return currentState;
    }

    public void SetPause(bool state)
    {
        //pause = state;
        if (state)
        {
            Time.timeScale = 0.0f;
            SetState(StateMachine.Pause);

        }
        else
        {
            Time.timeScale = 1.0f;
            SetState(StateMachine.Game);
        }
    }

    #endregion


}
