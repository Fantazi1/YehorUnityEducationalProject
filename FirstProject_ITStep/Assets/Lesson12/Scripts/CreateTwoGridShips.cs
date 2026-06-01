using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CreateTwoGridShips : MonoBehaviour
{
    [SerializeField] private int _rows = 10;
    [SerializeField] private int _columns = 10;

    [SerializeField] private string _shipSymbol = "#";

    [SerializeField] private Transform _leftGrid;
    [SerializeField] private Transform _rightGrid;

    [SerializeField] private GameObject _cellPrefab;

    private string[,] _leftField;
    private string[,] _rightField;

    private void Start()
    {
        _leftField = new string[_rows, _columns];
        _rightField = new string[_rows, _columns];

        CreateGrid(_leftGrid);
        CreateGrid(_rightGrid);

        PlaceShips(_leftField);
        PlaceShips(_rightField);

        DrawField(_leftGrid, _leftField);
        DrawField(_rightGrid, _rightField);
    }

    private void CreateGrid(Transform parent)
    {
        for (int y = 0; y < _columns; y++)
        {
            for (int x = 0; x < _rows; x++)
            {
                Instantiate(_cellPrefab, parent);
            }
        }
    }

    private void PlaceShips(string[,] field)
    {
        PlaceShip(field, 4, 1);
        PlaceShip(field, 3, 2);
        PlaceShip(field, 2, 3);
        PlaceShip(field, 1, 4);
    }

    private void PlaceShip(string[,] field, int shipSize, int count)
    {
        for (int i = 0; i < count; i++)
        {
            bool placed = false;

            while (!placed)
            {
                bool horizontal = Random.Range(0, 2) == 0;

                int x = Random.Range(0, _rows);
                int y = Random.Range(0, _columns);

                if (CanPlace(field, x, y, shipSize, horizontal))
                {
                    for (int j = 0; j < shipSize; j++)
                    {
                        int nx = horizontal ? x + j : x;
                        int ny = horizontal ? y : y + j;

                        field[nx, ny] = _shipSymbol;
                    }

                    placed = true;
                }
            }
        }
    }

    private bool CanPlace(string[,] field, int x, int y, int shipSize, bool horizontal)
    {
        if (horizontal && x + shipSize > _rows) return false;
        if (!horizontal && y + shipSize > _columns) return false;

        for (int i = -1; i <= shipSize; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int nx = horizontal ? x + i : x + j;
                int ny = horizontal ? y + j : y + i;

                if (nx >= 0 && nx < _rows &&
                    ny >= 0 && ny < _columns)
                {
                    if (field[nx, ny] == _shipSymbol) return false;
                }
            }
        }

        return true;
    }

    private void DrawField(Transform grid, string[,] field)
    {
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                Text text = grid.GetChild(i * _rows + j).GetComponentInChildren<Text>();

                text.text = field[i, j];


                UnityEngine.UI.Button _buttonGameShips = grid.GetChild(i * _rows + j).GetComponentInChildren<UnityEngine.UI.Button>();
                _buttonGameShips.onClick.AddListener(() => ButtonShipFightHandler(_buttonGameShips.gameObject));

                if (field[i, j] == _shipSymbol) {

                    Color myColor = new Color();
                    ColorUtility.TryParseHtmlString("#D25700", out myColor);
                    grid.GetChild(i * _rows + j).gameObject.GetComponentInChildren<UnityEngine.UI.Image>().color = myColor;

                }
            }
        }
    }



    private void ButtonShipFightHandler(GameObject _button)
    {
        Text textComponent = _button.GetComponentInChildren<Text>();

        if (textComponent.text == _shipSymbol)
        {
            textComponent.text = "X";
            textComponent.color = Color.red;
        }
        if (textComponent.text == string.Empty)
        {
            textComponent.text = ".";
            textComponent.color = Color.grey;

            Color myColor = new Color();
            ColorUtility.TryParseHtmlString("#AEAEAE", out myColor);

            _button.GetComponent<UnityEngine.UI.Image>().color = myColor;
        }
    }



}

