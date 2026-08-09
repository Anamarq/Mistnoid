using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [SerializeField] private LevelData[] levelData;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject enemyPrefab;

    private Transform currentEnemiesParent;
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
        GenerateEnemies(level);
        Debug.Log("Bloques destruibles: " + remainingBlocks);
    }
    void GenerateEnemies(LevelData level)
    {
        // Eliminar enemigos del nivel anterior
        if (currentEnemiesParent != null)
            Destroy(currentEnemiesParent.gameObject);

        // Crear nuevo contenedor
        currentEnemiesParent = new GameObject(
            "Enemies_Level_" + currentLevel
        ).transform;

        // Crear enemigos de este nivel
        foreach (Vector2Int position in level.enemyPositions)
        {
            Vector3 worldPosition = new Vector3(
                position.x * blockSpacingX,
                position.y * blockSpacingY,
                0
            );

            Instantiate(
                enemyPrefab,
                worldPosition,
                Quaternion.identity,
                currentEnemiesParent
            );
        }
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
        PlayerPrefs.SetInt("Level_" + currentLevel, 1);
        PlayerPrefs.Save();
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
        if(currentLevel == 1 && remainingBlocks == 1)
        {
            Debug.Log("LEVEL %");
            ProgressFlags.Level5Reached = true;
            PlayerPrefs.Save();
        }
    }

}