using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;

    [SerializeField] private Transform _parent;

    [SerializeField] private int _width = 3;
    [SerializeField] private int _height = 3;

    [SerializeField] private float _spacing = 110f;
    [SerializeField] private MenuTicTacToe _menuTicTacToe;

    private string[,] _board;

    private GameObject[,] _cells;

    private bool _playerTurn = true;
    private bool _gameOver;

    private const int _lastNormalSizeOfAGrid = 39;
    private const int _lastNormalWidth = 73;


    private void Awake()
    {
        if (_width > _lastNormalSizeOfAGrid && _height > _lastNormalSizeOfAGrid) {
            _width = _lastNormalSizeOfAGrid;
            _height = _lastNormalSizeOfAGrid;
        }

        if (_width > _lastNormalWidth) { 
            _width = _lastNormalWidth;
        }

        if (_height > _lastNormalSizeOfAGrid)
        {
            _height = _lastNormalSizeOfAGrid;
        }
    }

    void Start()
    {
        _board = new string[_width, _height];
        _cells = new GameObject[_width, _height];
        CreateGrid();
    }

    void CreateGrid()
    {
        float offsetX = (_width - 1) * _spacing / 2f;
        float offsetY = (_height - 1) * _spacing / 2f;

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                GameObject cell = Instantiate(_cellPrefab, _parent);
                cell.SetActive(true);

                RectTransform rt = cell.GetComponent<RectTransform>();

                rt.anchoredPosition = new Vector2(
                    x * _spacing - offsetX,
                    -y * _spacing + offsetY
                );

                int cx = x;
                int cy = y;

                cell.GetComponent<Button>().onClick.AddListener(() => ClickCell(cx, cy));

                _cells[x, y] = cell;
            }
        }


        MatrixResizing();
    }


    void MatrixResizing()
    {
        AdaptiveMatrixGrid adaptiveGrid = _parent.GetComponent<AdaptiveMatrixGrid>();

        adaptiveGrid.columns = _width; // I'm giving the script the new matrix width and making it resize
        adaptiveGrid.UpdateGrid();     
    }


    void ClickCell(int x, int y)
    {
        if (_gameOver || !_playerTurn || _board[x, y] != null) return;

        MakeMove(x, y, "X");

        if (CheckWin("X"))
        {
            _gameOver = true;
            _menuTicTacToe.onWin();
            return;
        }

        _playerTurn = false;
        BotMove();
    }

    void BotMove()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
                if (_board[x, y] == null)
                    emptyCells.Add(new Vector2Int(x, y));

        if (emptyCells.Count == 0) return;

        Vector2Int move = emptyCells[Random.Range(0, emptyCells.Count)];

        MakeMove(move.x, move.y, "O");

        if (CheckWin("O"))
        {
            _gameOver = true;
            _menuTicTacToe.onLose();
            return;
        }

        _playerTurn = true;
    }

    void MakeMove(int x, int y, string symbol)
    {
        _board[x, y] = symbol;
        _cells[x, y].GetComponentInChildren<Text>().text = symbol;
    }

    bool CheckWin(string symbol)
    {
        int needed = Mathf.Min(_width, _height);

        for (int y = 0; y < _height; y++)
        {
            int count = 0;
            for (int x = 0; x < _width; x++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        for (int x = 0; x < _width; x++)
        {
            int count = 0;
            for (int y = 0; y < _height; y++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        for (int startX = 0; startX < _width; startX++)
        {
            int count = 0;
            for (int x = startX, y = 0; x < _width && y < _height; x++, y++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        for (int startY = 1; startY < _height; startY++)
        {
            int count = 0;
            for (int x = 0, y = startY; x < _width && y < _height; x++, y++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        for (int startX = 0; startX < _width; startX++)
        {
            int count = 0;
            for (int x = startX, y = 0; x >= 0 && y < _height; x--, y++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        for (int startY = 1; startY < _height; startY++)
        {
            int count = 0;
            for (int x = _width - 1, y = startY; x >= 0 && y < _height; x--, y++)
            {
                count = _board[x, y] == symbol ? count + 1 : 0;
                if (count >= needed) return true;
            }
        }

        return false;
    }
}