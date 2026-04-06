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

        Block[] blocks = FindObjectsByType<Block>(FindObjectsSortMode.None);
        foreach (var b in blocks)
        {
            Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(b.transform.position.x),Mathf.RoundToInt(b.transform.position.y));
            levelData.blocks.Add(new LevelBlock
            {
                position = gridPos,
                blockData = b.GetBlockData()
            });
        }
        EditorUtility.SetDirty(levelData);
        Debug.Log("Level saved!");
    }
}
#endif