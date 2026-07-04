using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width = 8;
    [SerializeField] private int _height = 8;
    [SerializeField] private float _tileSize = 1f;
    [SerializeField] private TileLesson22 _tilePrefab;
    [SerializeField] private TileTypeConfig _tileConfig;
    [SerializeField] private GameManagerLesson22 _gameManager;

    private TileLesson22[,] _grid;
    private Vector3 _gridOrigin; // ФІКС: Vector3
    private bool _isProcessing;

    public bool IsProcessing => _isProcessing;

    private void Start()
    {
        if (_tilePrefab == null || _tileConfig == null || _gameManager == null)
        {
            Debug.LogError("Board: missing references.");
            return;
        }

        CalculateGridOrigin();
        InitializeBoard();
    }

    private void CalculateGridOrigin()
    {
        float totalWidth = _width * _tileSize;
        float totalHeight = _height * _tileSize;
        _gridOrigin = new Vector3(
            -totalWidth / 2f + _tileSize / 2f,
            -totalHeight / 2f + _tileSize / 2f,
            0f
        );
    }

    private void InitializeBoard()
    {
        _grid = new TileLesson22[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                SpawnTile(x, y);
            }
        }

        StartCoroutine(RemoveInitialMatches());
    }

    private IEnumerator RemoveInitialMatches()
    {
        _isProcessing = true;
        List<Vector2Int> matches = FindAllMatches();

        while (matches.Count > 0)
        {
            foreach (Vector2Int pos in matches)
            {
                DestroyTile(pos.x, pos.y);
            }

            yield return null;
            yield return StartCoroutine(ApplyGravityAndRefill());
            matches = FindAllMatches();
        }

        _isProcessing = false;
    }

    private void SpawnTile(int column, int row)
    {
        TileType type = _tileConfig.GetRandomType();
        Vector3 position = GetWorldPosition(column, row);
        Sprite sprite = _tileConfig.GetSprite(type);
        Color color = _tileConfig.GetColor(type);

        if (_grid[column, row] != null)
        {
            _grid[column, row].gameObject.SetActive(false);
            Destroy(_grid[column, row].gameObject);
        }

        TileLesson22 tile = Instantiate(_tilePrefab, position, Quaternion.identity, transform);
        tile.Initialize(this, column, row, type, sprite, color);
        tile.Clicked += _gameManager.OnTileClicked;
        _grid[column, row] = tile;
    }

    private void DestroyTile(int column, int row)
    {
        if (_grid[column, row] != null)
        {
            _grid[column, row].Clicked -= _gameManager.OnTileClicked;
            _grid[column, row].PlayDestroyEffect();
            _grid[column, row].gameObject.SetActive(false);
            Destroy(_grid[column, row].gameObject);
            _grid[column, row] = null;
        }
    }

    public Vector3 GetWorldPosition(int column, int row)
    {
        return new Vector3(
            _gridOrigin.x + column * _tileSize,
            _gridOrigin.y + row * _tileSize,
            0f
        );
    }

    public bool IsAdjacent(TileLesson22 tile1, TileLesson22 tile2)
    {
        return Mathf.Abs(tile1.Column - tile2.Column) + Mathf.Abs(tile1.Row - tile2.Row) == 1;
    }

    public void TrySwap(TileLesson22 tile1, TileLesson22 tile2)
    {
        if (_isProcessing) return;
        StartCoroutine(SwapCoroutine(tile1, tile2));
    }

    private IEnumerator SwapCoroutine(TileLesson22 tile1, TileLesson22 tile2)
    {
        if (tile1 == null || tile2 == null)
        {
            _isProcessing = false;
            yield break;
        }

        _isProcessing = true;

        int col1 = tile1.Column;
        int row1 = tile1.Row;
        int col2 = tile2.Column;
        int row2 = tile2.Row;

        _grid[col1, row1] = tile2;
        _grid[col2, row2] = tile1;
        tile1.SetGridPosition(col2, row2);
        tile2.SetGridPosition(col1, row1);

        tile1.MoveToPosition(GetWorldPosition(col2, row2));
        tile2.MoveToPosition(GetWorldPosition(col1, row1));

        yield return null;

        while (AnyTilesMoving())
        {
            yield return null;
        }

        List<Vector2Int> matches = FindAllMatches();

        if (matches.Count > 0)
        {
            yield return StartCoroutine(ProcessMatches(matches));
        }
        else
        {
            if (tile1 != null && tile2 != null)
            {
                _grid[col1, row1] = tile1;
                _grid[col2, row2] = tile2;
                tile1.SetGridPosition(col1, row1);
                tile2.SetGridPosition(col2, row2);
                tile1.MoveToPosition(GetWorldPosition(col1, row1));
                tile2.MoveToPosition(GetWorldPosition(col2, row2));

                yield return null;

                while (AnyTilesMoving())
                {
                    yield return null;
                }
            }
        }

        _isProcessing = false;
    }

    private List<Vector2Int> FindAllMatches()
    {
        HashSet<Vector2Int> matchedPositions = new HashSet<Vector2Int>();

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width - 2; x++)
            {
                TileLesson22 tile = _grid[x, y];
                if (tile == null) continue;

                TileType type = tile.Type;
                int matchLength = 1;

                while (x + matchLength < _width && _grid[x + matchLength, y] != null && _grid[x + matchLength, y].Type == type)
                {
                    matchLength++;
                }

                if (matchLength >= 3)
                {
                    for (int i = 0; i < matchLength; i++)
                    {
                        matchedPositions.Add(new Vector2Int(x + i, y));
                    }
                }

                x += matchLength - 1;
            }
        }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height - 2; y++)
            {
                TileLesson22 tile = _grid[x, y];
                if (tile == null) continue;

                TileType type = tile.Type;
                int matchLength = 1;

                while (y + matchLength < _height && _grid[x, y + matchLength] != null && _grid[x, y + matchLength].Type == type)
                {
                    matchLength++;
                }

                if (matchLength >= 3)
                {
                    for (int i = 0; i < matchLength; i++)
                    {
                        matchedPositions.Add(new Vector2Int(x, y + i));
                    }
                }

                y += matchLength - 1;
            }
        }

        return new List<Vector2Int>(matchedPositions);
    }

    private IEnumerator ProcessMatches(List<Vector2Int> matches)
    {
        _gameManager.AddScore(matches.Count);

        foreach (Vector2Int pos in matches)
        {
            DestroyTile(pos.x, pos.y);
        }

        yield return new WaitForSeconds(0.05f);
        yield return StartCoroutine(ApplyGravityAndRefill());

        List<Vector2Int> newMatches = FindAllMatches();
        if (newMatches.Count > 0)
        {
            _gameManager.IncrementCombo();
            yield return StartCoroutine(ProcessMatches(newMatches));
        }
        else
        {
            _gameManager.ResetCombo();
            _gameManager.OnMoveCompleted();

            if (!HasValidMoves())
            {
                ShuffleBoard();
            }
        }
    }

    private IEnumerator ApplyGravityAndRefill()
    {
        for (int x = 0; x < _width; x++)
        {
            int emptyCount = 0;

            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] == null)
                {
                    emptyCount++;
                }
                else if (emptyCount > 0)
                {
                    int newY = y - emptyCount;

                    if (_grid[x, newY] != null)
                    {
                        _grid[x, newY].gameObject.SetActive(false);
                        Destroy(_grid[x, newY].gameObject);
                    }

                    _grid[x, newY] = _grid[x, y];
                    _grid[x, y] = null;
                    _grid[x, newY].SetGridPosition(x, newY);
                    _grid[x, newY].MoveToPosition(GetWorldPosition(x, newY));
                }
            }

            for (int i = 0; i < emptyCount; i++)
            {
                int row = _height - emptyCount + i;
                Vector3 targetPos = GetWorldPosition(x, row);

                float spawnY = GetWorldPosition(x, _height - 1).y + _tileSize + (i * _tileSize);
                Vector3 spawnPos = new Vector3(targetPos.x, spawnY, 0f);

                TileType type = _tileConfig.GetRandomType();
                Sprite sprite = _tileConfig.GetSprite(type);
                Color color = _tileConfig.GetColor(type);

                if (_grid[x, row] != null)
                {
                    _grid[x, row].gameObject.SetActive(false);
                    Destroy(_grid[x, row].gameObject);
                }

                TileLesson22 tile = Instantiate(_tilePrefab, spawnPos, Quaternion.identity, transform);
                tile.Initialize(this, x, row, type, sprite, color);
                tile.Clicked += _gameManager.OnTileClicked;
                tile.MoveToPosition(targetPos);
                _grid[x, row] = tile;
            }
        }

        yield return null;

        while (AnyTilesMoving())
        {
            yield return null;
        }
    }

    // ГЛОБАЛЬНИЙ ФІКС: Тепер опираємося виключно на стан самого тайлу.
    // Жодних математичних порівнянь відстаней у крос-скриптах.
    private bool AnyTilesMoving()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                TileLesson22 tile = _grid[x, y];
                if (tile != null && tile.IsMoving)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool HasValidMoves()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] == null) continue;

                if (x < _width - 1)
                {
                    SwapGrid(x, y, x + 1, y);
                    if (FindAllMatches().Count > 0)
                    {
                        SwapGrid(x, y, x + 1, y);
                        return true;
                    }
                    SwapGrid(x, y, x + 1, y);
                }

                if (y < _height - 1)
                {
                    SwapGrid(x, y, x, y + 1);
                    if (FindAllMatches().Count > 0)
                    {
                        SwapGrid(x, y, x, y + 1);
                        return true;
                    }
                    SwapGrid(x, y, x, y + 1);
                }
            }
        }
        return false;
    }

    private void SwapGrid(int x1, int y1, int x2, int y2)
    {
        TileLesson22 temp = _grid[x1, y1];
        _grid[x1, y1] = _grid[x2, y2];
        _grid[x2, y2] = temp;
    }

    public void ShuffleBoard()
    {
        StopAllCoroutines();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (_grid[x, y] != null)
                {
                    _grid[x, y].Clicked -= _gameManager.OnTileClicked;
                    Destroy(_grid[x, y].gameObject);
                    _grid[x, y] = null;
                }
            }
        }

        InitializeBoard();
    }
}