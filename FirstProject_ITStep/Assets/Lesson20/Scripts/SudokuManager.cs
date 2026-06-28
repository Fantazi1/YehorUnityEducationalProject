using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SudokuManager : MonoBehaviour
{
    [Header("Посилання на UI")]
    public GameObject cellPrefab;
    public Transform playerBoardPanel;
    public Transform botBoardPanel;

    [Header("Налаштування Бота")]
    [Tooltip("Скільки секунд бот думає перед наступним ходом")]
    public float botMoveDelay = 3f;

    private SudokuCell[,] _playerGrid = new SudokuCell[9, 9];
    private SudokuCell[,] _botGrid = new SudokuCell[9, 9];

    private List<SudokuCell> _botEmptyCells = new List<SudokuCell>();

    private SudokuCell _selectedCell;
    private bool _gameOver = false;
    
    [SerializeField] private GameObject _winImage;
    [SerializeField] private GameObject _loseImage;

    private void Start()
    {
        GenerateDualBoards();

        StartCoroutine(BotPlayRoutine());
    }

    private void GenerateDualBoards()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int correctValue = (row * 3 + row / 3 + col) % 9 + 1;
                bool hideOriginal = Random.value > 0.4f;

                GameObject playerCellObj = Instantiate(cellPrefab, playerBoardPanel);
                SudokuCell pCell = playerCellObj.GetComponent<SudokuCell>();
                pCell.Init(row, col, correctValue, hideOriginal, this, isBotCell: false);
                _playerGrid[row, col] = pCell;

                GameObject botCellObj = Instantiate(cellPrefab, botBoardPanel);
                SudokuCell bCell = botCellObj.GetComponent<SudokuCell>();
                bCell.Init(row, col, correctValue, hideOriginal, this, isBotCell: true);
                _botGrid[row, col] = bCell;

                if (hideOriginal)
                {
                    _botEmptyCells.Add(bCell);
                }
            }
        }
    }

    private IEnumerator BotPlayRoutine()
    {
        while (!_gameOver && _botEmptyCells.Count > 0)
        {
            yield return new WaitForSeconds(botMoveDelay);

            if (_gameOver) yield break;

            int randomIndex = Random.Range(0, _botEmptyCells.Count);
            SudokuCell targetCell = _botEmptyCells[randomIndex];

            targetCell.SetValue(targetCell.CorrectValue);
            _botEmptyCells.RemoveAt(randomIndex);

            CheckWinCondition(_botGrid, "бот");
        }
    }

    public void SelectCell(SudokuCell cell)
    {
        if (cell.IsLocked || _gameOver) return;

        if (_selectedCell != null)
        {
            _selectedCell.SetSelected(false);
        }

        _selectedCell = cell;
        _selectedCell.SetSelected(true);
    }

    private void Update()
    {
        if (_selectedCell == null || _gameOver) return;

        for (int i = 1; i <= 9; i++)
        {
            KeyCode alphaKey = KeyCode.Alpha0 + i;
            KeyCode padKey = KeyCode.Keypad0 + i; 

            if (Input.GetKeyDown(alphaKey) || Input.GetKeyDown(padKey))
            {
                SetCellAndCheck(i);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) ||
            Input.GetKeyDown(KeyCode.Delete) ||
            Input.GetKeyDown(KeyCode.Alpha0) ||
            Input.GetKeyDown(KeyCode.Keypad0))
        {
            _selectedCell.SetValue(0);
        }
    }

    private void SetCellAndCheck(int number)
    {
        _selectedCell.SetValue(number);
        CheckWinCondition(_playerGrid, "гравець");
    }

    private void CheckWinCondition(SudokuCell[,] gridToCheck, string winnerName)
    {
        foreach (var cell in gridToCheck)
        {
            if (cell.Value != cell.CorrectValue)
            {
                return;
            }
        }

        _gameOver = true;
        Debug.Log($"Матч завершено, переміг: {winnerName}!");
        if (winnerName == "гравець") { 
            _winImage.SetActive(true);
        }
        else if (winnerName == "бот")
        {
            _loseImage.SetActive(true);
        }

    }
}