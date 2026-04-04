using UnityEngine;

public class RunTimer : MonoBehaviour
{
    public static RunTimer Instance;
    private float runTime;
    private bool running;
    public float RunTime => runTime;

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        runTime = 0f;
    }
    void Update()
    {
        if (!running) return;

        runTime += Time.deltaTime;

        PlayCanvas.Instance?.UpdateTimer(runTime);
    }

    public void StartRun()
    {
        running = true;
    }

    public void StopRun()
    {
        running = false;
    }
}