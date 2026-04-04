using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private BlockData selectedBlock;

    [SerializeField] private GameObject blockPreviewPrefab;
    [SerializeField] private float gridSize = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlaceBlock();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RemoveBlock();
        }
    }

    void PlaceBlock()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(mousePos.x / gridSize),
            Mathf.RoundToInt(mousePos.y / gridSize)
        );

        foreach (var block in levelData.blocks)
        {
            if (block.position == gridPos)
                return;
        }

        LevelBlock newBlock = new LevelBlock
        {
            position = gridPos,
            blockData = selectedBlock
        };

        levelData.blocks.Add(newBlock);

        Instantiate(blockPreviewPrefab,
            new Vector3(gridPos.x * gridSize, gridPos.y * gridSize),
            Quaternion.identity);
    }

    void RemoveBlock()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int gridPos = new Vector2Int(
            Mathf.RoundToInt(mousePos.x / gridSize),
            Mathf.RoundToInt(mousePos.y / gridSize)
        );

        for (int i = levelData.blocks.Count - 1; i >= 0; i--)
        {
            if (levelData.blocks[i].position == gridPos)
            {
                levelData.blocks.RemoveAt(i);
            }
        }
    }

    public void SelectBlock(BlockData block)
    {
        selectedBlock = block;
    }
}