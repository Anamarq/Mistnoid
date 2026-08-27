#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public LevelData levelData;

    private float blockSpacingX = 0.85f, blockSpacingY = 0.45f;

    [ContextMenu("Save Level")]
    public void SaveLevel()
    {
        levelData.blocks.Clear();
        levelData.enemyPositions.Clear();
        levelData.enemyAcidPositions.Clear();

        // blocks

        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);

        foreach (var block in blocks)
        {
            Vector2Int gridPos = WorldToGrid(block.transform.position);

            levelData.blocks.Add(new LevelBlock
            {
                position = gridPos,
                blockData = block.GetBlockData()
            });
        }

        // enemies

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            Vector2Int gridPos = WorldToGrid(enemy.transform.position);
            levelData.enemyPositions.Add(gridPos);
        }

        // acid enemies

        AcidEnemy[] acidEnemies = FindObjectsByType<AcidEnemy>(FindObjectsSortMode.None);

        foreach (var acidEnemy in acidEnemies)
        {
            Vector2Int gridPos = WorldToGrid(acidEnemy.transform.position);
            levelData.enemyAcidPositions.Add(gridPos);
        }

        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();

        Debug.Log(
            $"Level saved! " +
            $"Blocks: {blocks.Length}, " +
            $"Enemies: {enemies.Length}, " +
            $"Acid enemies: {acidEnemies.Length}"
        );
    }

    private Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / blockSpacingX),
            Mathf.RoundToInt(worldPosition.y / blockSpacingY)
        );
    }
}
#endif