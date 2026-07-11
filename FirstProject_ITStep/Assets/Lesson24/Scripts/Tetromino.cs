using UnityEngine;

public class Tetromino : MonoBehaviour
{
    [HideInInspector] public int pieceIndex;
    [HideInInspector] public Vector2[] cells;
    [HideInInspector] public Color color;

    private Transform[] blockObjects = new Transform[4];
    private GridManagerTetris gridManager;
    private Sprite blockSprite;

    private static readonly Vector2[] WallKicks = new Vector2[]
    {
        Vector2.zero,
        Vector2.left,
        Vector2.right,
        new Vector2(-2, 0),
        new Vector2(2, 0),
        Vector2.up,
    };

    public void Initialize(int index, Vector2 spawnPos, GridManagerTetris gm, Sprite sprite)
    {
        pieceIndex = index;
        cells = PieceDataTetris.Shapes[index];
        color = PieceDataTetris.Colors[index];
        gridManager = gm;
        blockSprite = sprite;

        transform.position = new Vector2(spawnPos.x,spawnPos.y-2); //spawnPos

        for (int i = 0; i < 4; i++)
        {
            GameObject block = new GameObject("Cell");
            block.transform.SetParent(transform);
            block.transform.localPosition = cells[i];
            block.transform.localScale = Vector3.one;

            SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
            sr.sprite = blockSprite;
            sr.color = color;

            blockObjects[i] = block.transform;
        }
    }

    public bool CanFitAtPosition(Vector2 pos)
    {
        return gridManager.CanFit(cells, pos);
    }

    public bool TryMove(Vector2 direction)
    {
        Vector2 newPos = (Vector2)transform.position + direction;
        if (gridManager.CanFit(cells, newPos))
        {
            transform.position = newPos;
            return true;
        }
        return false;
    }

    public bool TryRotate()
    {
        Vector2[] rotated = PieceDataTetris.Rotate(cells, clockwise: true);

        foreach (Vector2 kick in WallKicks)
        {
            Vector2 newPos = (Vector2)transform.position + kick;
            if (gridManager.CanFit(rotated, newPos))
            {
                transform.position = newPos;
                cells = rotated;
                UpdateBlockPositions();
                return true;
            }
        }
        return false;
    }

    private void UpdateBlockPositions()
    {
        for (int i = 0; i < 4; i++)
            blockObjects[i].localPosition = cells[i];
    }

    public int HardDrop()
    {
        int steps = 0;
        while (TryMove(Vector2.down))
            steps++;
        return steps;
    }

    public void Land()
    {
        gridManager.PlaceBlocks(cells, transform.position, color, blockSprite);
        GameManagerTetris.Instance.OnPieceLanded();
        Destroy(gameObject);
    }
}
