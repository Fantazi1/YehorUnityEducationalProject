using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class PuzzlePieceDrag : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    public Vector2 TargetPosition;

    public float SnapDistance = 40f;

    public bool IsPlaced { get; private set; }

    RectTransform rect;
    CanvasGroup canvasGroup;
    Canvas canvas;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (IsPlaced) return;

        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData e)
    {
        if (IsPlaced) return;

        float scale = canvas != null
            ? canvas.scaleFactor
            : 1f;

        rect.anchoredPosition += e.delta / scale;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (IsPlaced) return;

        canvasGroup.blocksRaycasts = true;

        if (Vector2.Distance(rect.anchoredPosition, TargetPosition) <= SnapDistance)
        {
            rect.anchoredPosition = TargetPosition;
            IsPlaced = true;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsFirstSibling();

            Puzzle.instancePuzzle.listPieceDrag.Add(IsPlaced);

            if(Puzzle.instancePuzzle.listPieceDrag.Count == Puzzle.instancePuzzle.columns * Puzzle.instancePuzzle.rows)
            {
                //Debug.Log("Ви перемогли!");
                Puzzle.instancePuzzle.WinPuzzle();
            }
        }
    }
}