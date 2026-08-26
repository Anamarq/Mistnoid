#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public LevelData levelData;

    [ContextMenu("Save Level")]
    public void SaveLevel()
    {
        levelData.blocks.Clear();
        levelData.enemyPositions.Clear();
        levelData.enemyAcidPositions.Clear();

        // blocks

        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);

        foreach (var b in blocks)
        {
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt(b.transform.position.x),
                Mathf.RoundToInt(b.transform.position.y)
            );

            levelData.blocks.Add(new LevelBlock
            {
                position = gridPos,
                blockData = b.GetBlockData()
            });
        }

        // enemies

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt(enemy.transform.position.x),
                Mathf.RoundToInt(enemy.transform.position.y)
            );

            levelData.enemyPositions.Add(gridPos);
        }

        // Acid enemies

        AcidEnemy[] acidEnemies = FindObjectsByType<AcidEnemy>(FindObjectsSortMode.None);

        foreach (var acidEnemy in acidEnemies)
        {
            Vector2Int gridPos = new Vector2Int(
                Mathf.RoundToInt(acidEnemy.transform.position.x),
                Mathf.RoundToInt(acidEnemy.transform.position.y)
            );

            levelData.enemyAcidPositions.Add(gridPos);
        }

        // save
        EditorUtility.SetDirty(levelData);
        AssetDatabase.SaveAssets();

        Debug.Log(
            "Level saved! " +
            blocks.Length + " bloques, " +
            enemies.Length + " enemigos y " +
            acidEnemies.Length + " enemigos de ácido."
        );
    }
}
#endif