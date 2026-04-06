using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Mistnoid/Level")]
public class LevelData : ScriptableObject
{
    public int width = 12;
    public int height = 8;
    public List<LevelBlock> blocks = new List<LevelBlock>();
#if UNITY_EDITOR
    public void AddRow(int y, int xMin, int xMax, BlockData blockType)
    {
        for (int x = xMin; x <= xMax; x++)
        {
            blocks.Add(new LevelBlock
            {
                position = new Vector2Int(x, y),
                blockData = blockType
            });
        }

        EditorUtility.SetDirty(this);
    }
#endif

}

[System.Serializable]
public class LevelBlock
{
    public Vector2Int position;
    public BlockData blockData;
}
