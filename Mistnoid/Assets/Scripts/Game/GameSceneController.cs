using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    [SerializeField] private DialogueData introDialogue;
    void Start()
    {
        CheckFirstRun();
    }

    void CheckFirstRun()
    {
        var state = GameProgressManager.Instance.State;

        if (state == GameProgressState.FirstRun)
        {
            StartTutorial();
        }
    }

    void StartTutorial()
    {
        GameManager.Instance.IsDialogue = true;
        PowerUpManager.Instance.Unlock(PowerUpType.ExpandPaddle);
        PowerUpManager.Instance.Unlock(PowerUpType.ShrinkPaddle);
        //string[] dialogue =
        //{
        //    "¡Eh! Has venido ??",
        //    "Vale, escucha rápido...",
        //    "Para romper los bloques tienes que rebotar la bola con la pala.",
        //    "Yo puedo ayudarte un poco...",
        //    "Intentaré hacer la pala más grande con mi magia ?",
        //    "…aunque a veces no me sale muy bien ??",
        //    "¡Vamos, inténtalo!"
        //};

        DialogueManager.Instance.StartDialogue(introDialogue);

        DialogueManager.Instance.OnDialogueEnd += StartGame;
    }


    void StartGame()
    {
        Debug.Log("START");
        DialogueManager.Instance.OnDialogueEnd -= StartGame;

        GameManager.Instance.SetPause(false);
        GameManager.Instance.IsDialogue = false;

        GameProgressManager.Instance.SetState(GameProgressState.AfterFirstRun);
    }
}