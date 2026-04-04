using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] private LevelData[] levelData;
    [SerializeField] private GameObject blockPrefab;
    private float blockSpacingX = 0.85f, blockSpacingY = 0.45f;

    private int remainingBlocks;
    private int currentLevel = 0;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if(levelData.Length > 0)
            GenerateLevel(levelData[0]);
        RunTimer.Instance.StartRun();
        PlayCanvas.Instance.UpdatePhase(0);
        AbilityManager.Instance.ResetUses();
        ScoreManager.Instance.StartRun();
    }

    void GenerateLevel(LevelData level)
    {
        remainingBlocks = 0;

        foreach (var block in level.blocks)
        {
            Vector3 pos = new Vector3(
                block.position.x * blockSpacingX,
                block.position.y * blockSpacingY,
                0
            );

            GameObject obj = Instantiate(blockPrefab, pos, Quaternion.identity);

            Block blockComponent = obj.GetComponent<Block>();
            blockComponent.SetBlockData(block.blockData);

            if (!block.blockData.indestructible)
                remainingBlocks++;
        }

        Debug.Log("Bloques destruibles: " + remainingBlocks);
    }
    void NextLevel()
    {
        ++currentLevel;
        if (currentLevel < levelData.Length)
        {
            PlayCanvas.Instance.PanelLevel(true);
            GenerateLevel(levelData[currentLevel]);
            PowerUpPool.Instance.ResetPool();
            PlayCanvas.Instance.UpdatePhase(currentLevel);
        }
        else
            FinishGame();
    }
    void FinishGame()
    {
        ScoreManager.Instance.WinRun();
        RunTimer.Instance.StopRun();
        PlayerController.Instance.ResetPaddle();
    }
    void WinLevel()
    {
        Debug.Log("Nivel completado");
        PlayerController.Instance.ResetPaddle();
        NextLevel();
        GameManager.Instance.SetPause(true);

    }

    public void BlockDestroyed()
    {
        
        remainingBlocks--;
        Debug.Log("remainingBlocks " + remainingBlocks);
        if (remainingBlocks <= 0)
        {
            WinLevel();
        }
    }
}