using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class AdaptiveMatrixGrid : MonoBehaviour
{
    [Header("Matrix Settings")]
    [Tooltip("What is the matrix size")]
    public int columns = 5;

    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;

    void Start()
    {
        UpdateGrid();
    }

    void OnRectTransformDimensionsChange()
    {
        UpdateGrid();
    }

    private void OnValidate()
    {
        if (columns < 1) columns = 1;
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        if (gridLayout == null) gridLayout = GetComponent<GridLayoutGroup>();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (gridLayout == null || rectTransform == null) return;

        int childCount = transform.childCount;
        if (childCount == 0) return;

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        int rows = Mathf.CeilToInt((float)childCount / columns);

        float availableWidth = rectTransform.rect.width - (gridLayout.padding.left + gridLayout.padding.right);
        float availableHeight = rectTransform.rect.height - (gridLayout.padding.top + gridLayout.padding.bottom);

        if (columns > 1) availableWidth -= gridLayout.spacing.x * (columns - 1);
        if (rows > 1) availableHeight -= gridLayout.spacing.y * (rows - 1);

        float cellWidth = availableWidth / columns;
        float cellHeight = availableHeight / rows;

        float finalSize = Mathf.Min(cellWidth, cellHeight);

        if (finalSize < 0) finalSize = 0;

        gridLayout.cellSize = new Vector2(finalSize, finalSize);
    }
}