using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public Tile tilePrefab;

    private static int SIZE = 4;

    public int _SIZE_Acces { 
        set { 
            SIZE = value;
        } 
        get { 
            return SIZE;
        } 
    }

    public float cellSize = 100f;

    public float cellSpacing = 5f;

    [SerializeField] private GameObject winPanel;

    private Tile[,] board;

    private int emptyX;
    private int emptyY;

    private bool isShuffling = false;

    void Start()
    {
        //Debug.Log("Size: " + SIZE);
        board = new Tile[SIZE, SIZE];
        if (winPanel != null) winPanel.SetActive(false);

        GenerateBoard();

        isShuffling = true;
        Shuffle(200);
        isShuffling = false;
    }

    void Update()
    {
        if (winPanel != null && winPanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            InstantWinCheat();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) EmptyMove(0, -1);
        if (Input.GetKeyDown(KeyCode.DownArrow)) EmptyMove(0, 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) EmptyMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) EmptyMove(1, 0);
    }

    private void GenerateBoard()
    {
        int number = 1;

        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                if (x == SIZE - 1 && y == SIZE - 1)
                {
                    emptyX = x;
                    emptyY = y;
                    board[x, y] = null;
                    continue;
                }

                Tile tile = Instantiate(tilePrefab, transform);
                tile.SetNumber(number);
                board[x, y] = tile;

                number++;
            }
        }

        UpdateVisuals();
    }

    private void EmptyMove(int dx, int dy)
    {
        int targetX = emptyX + dx;
        int targetY = emptyY + dy;

        if (targetX < 0 || targetX >= SIZE) return;
        if (targetY < 0 || targetY >= SIZE) return;

        board[emptyX, emptyY] = board[targetX, targetY];
        board[targetX, targetY] = null;

        emptyX = targetX;
        emptyY = targetY;

        UpdateVisuals();

        if (!isShuffling)
        {
            CheckWin();
        }
    }

    private void UpdateVisuals()
    {
        float startX = -((SIZE - 1) * (cellSize + cellSpacing)) / 2f;
        float startY = ((SIZE - 1) * (cellSize + cellSpacing)) / 2f;

        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                if (board[x, y] == null) continue;

                RectTransform rect = board[x, y].GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(
                    startX + x * (cellSize + cellSpacing),
                    startY - y * (cellSize + cellSpacing)
                );
            }
        }
    }

    private void CheckWin()
    {
        int expectedNumber = 1;

        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                if (x == SIZE - 1 && y == SIZE - 1)
                {
                    if (board[x, y] != null) return;
                    continue;
                }

                if (board[x, y] == null) return;

                if (board[x, y].Number != expectedNumber)
                {
                    return;
                }

                expectedNumber++;
            }
        }

        OnWin();
    }

    private void OnWin()
    {
        Debug.Log("Ви перемогли!");
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    private void InstantWinCheat()
    {
        // 1. Збираємо абсолютно всі плашки, які зараз є на сцені
        List<Tile> tempTiles = new List<Tile>();
        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                if (board[x, y] != null)
                {
                    tempTiles.Add(board[x, y]);
                }
            }
        }

        // 2. Повністю очищаємо матрицю дошки перед новим розподілом
        System.Array.Clear(board, 0, board.Length);

        // 3. Визначаємо координати для ситуації "1 хід до перемоги"
        // Пуста клітинка буде ліворуч від кутка, а остання плашка — в самому кутку
        int lastRowY = SIZE - 1;
        int cornerX = SIZE - 1;
        int preCornerX = SIZE - 2;

        int tileIndex = 0;
        int number = 1;

        // 4. Проходимо по всій сітці
        for (int y = 0; y < SIZE; y++)
        {
            for (int x = 0; x < SIZE; x++)
            {
                // Пропускаємо дві останні клітинки всієї дошки, їх налаштуємо окремо
                if (y == lastRowY && (x == preCornerX || x == cornerX))
                {
                    continue;
                }

                // Розставляємо плашки по порядку
                if (tileIndex < tempTiles.Count)
                {
                    Tile tile = tempTiles[tileIndex];
                    tile.SetNumber(number);
                    board[x, y] = tile;
                    tileIndex++;
                }
                number++;
            }
        }

        // 5. Налаштовуємо фінальні дві клітинки для створення умови в 1 хід

        // Ставимо ПУСТОТУ на передостаннє місце (ліворуч від кутка)
        board[preCornerX, lastRowY] = null;
        emptyX = preCornerX;
        emptyY = lastRowY;

        // Ставимо ОСТАННЮ плашку в самий куток
        // Її номер дорівнює загальній кількості плашок на дошці: (SIZE * SIZE) - 1
        int lastTileNumber = (SIZE * SIZE) - 1;

        // У списку tempTiles вона завжди гарантовано лежить в самому кінці (останній індекс)
        Tile lastTile = tempTiles[tempTiles.Count - 1];
        lastTile.SetNumber(lastTileNumber);
        board[cornerX, lastRowY] = lastTile;

        // 6. Оновлюємо позиції об'єктів на екрані
        UpdateVisuals();

        Debug.Log($"[Чіт] Дошка {SIZE}x{SIZE} активована! Залишився 1 хід: натисніть Стрілочку Вліво (Left Arrow).");
    }

    private void Shuffle(int moves)
    {
        Vector2Int[] dirs =
        {
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
        };

        for (int i = 0; i < moves; i++)
        {
            Vector2Int dir = dirs[Random.Range(0, dirs.Length)];
            EmptyMove(dir.x, dir.y);
        }
    }
}