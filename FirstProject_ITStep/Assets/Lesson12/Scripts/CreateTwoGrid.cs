using UnityEngine;

public class CreateTwoGrid : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;

    [SerializeField] private Transform _leftGrid;
    [SerializeField] private Transform _rightGrid;

    [SerializeField] private int _rows = 10;
    [SerializeField] private int _columns = 10;

    void Start()
    {
        CreateGrid(_leftGrid);
        CreateGrid(_rightGrid);
    }

    void CreateGrid(Transform parent)
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                Instantiate(_cellPrefab, parent);
            }
        }
    }
}
