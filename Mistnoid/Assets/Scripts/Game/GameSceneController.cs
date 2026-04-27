using UnityEngine;

public class GameSceneController : MonoBehaviour
{
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
        GameManager.Instance.SetPause(true);
        PowerUpManager.Instance.EnableOnly(
            PowerUpType.ExpandPaddle,
            PowerUpType.ShrinkPaddle
        );
        string[] dialogue =
        {
            "¡Eh! Has venido ??",
            "Vale, escucha rápido...",
            "Para romper los bloques tienes que rebotar la bola con la pala.",
            "Yo puedo ayudarte un poco...",
            "Intentaré hacer la pala más grande con mi magia ?",
            "…aunque a veces no me sale muy bien ??",
            "¡Vamos, inténtalo!"
        };

        DialogueManager.Instance.StartDialogue(dialogue);

        DialogueManager.Instance.OnDialogueEnd += StartGame;
    }

    void StartGame()
    {
        DialogueManager.Instance.OnDialogueEnd -= StartGame;

        GameManager.Instance.SetPause(false);
    }
}