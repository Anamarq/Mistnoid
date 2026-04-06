using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Mistnoid/Level")]
public class LevelData : ScriptableObject
{
    public int width = 12;
    public int height = 8;
    public List<LevelBlock> blocks = new List<LevelBlock>();

}

[System.Serializable]
public class LevelBlock
{
    public Vector2Int position;
    public BlockData blockData;
}
