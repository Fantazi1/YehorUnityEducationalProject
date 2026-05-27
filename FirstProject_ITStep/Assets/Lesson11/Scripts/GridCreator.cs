using UnityEngine;

public class GridCreater : MonoBehaviour
{
    [SerializeField] private GameObject _btnPref;
    [SerializeField] private Transform _parent;

    [SerializeField] private int _rows;
    [SerializeField] private int _cols;

    [SerializeField] private float _spacing;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        float offsetX = (_rows - 1) * _spacing / 2f;
        float offsetY = (_cols - 1) * _spacing / 2f;

        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                GameObject button = Instantiate(_btnPref, _parent);

                button.SetActive(true);

                RectTransform rectTransform = button.GetComponent<RectTransform>();

                rectTransform.anchoredPosition = new Vector2(i * _spacing - offsetX, -j * _spacing + offsetY);
            }
        }
    }
}