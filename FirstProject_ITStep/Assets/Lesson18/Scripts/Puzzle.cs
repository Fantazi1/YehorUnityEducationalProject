using NUnit.Framework;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Puzzle : MonoBehaviour
{
    public Texture2D[] sourceImagesArr;
    public Texture2D sourceImage;

    public string loadFromPath = "";

    public int columns = 4;
    public int rows = 4;

    public RectTransform board;
    public RectTransform tray;
    public RectTransform pieceLayer;

    public float snapDistance = 40f;

    public bool showBorders = true;

    Canvas canvas;

    public List<bool> listPieceDrag = new List<bool>();
    public List<GameObject> allPiecesImages = new List<GameObject>();

    public static Puzzle instancePuzzle;

    [SerializeField] private GameObject winImgPanel;
    private int imgCounter = 0;
    private void Awake()
    {
        instancePuzzle = this;
    }

    void Start()
    {
        sourceImage = sourceImagesArr[imgCounter];
        canvas = GetComponentInParent<Canvas>();

        if (pieceLayer == null)
            pieceLayer = (RectTransform)transform;

        pieceLayer.anchorMin = Vector2.zero;
        pieceLayer.anchorMax = Vector2.one;
        pieceLayer.pivot = new Vector2(0.5f, 0.5f);
        pieceLayer.offsetMin = Vector2.zero;
        pieceLayer.offsetMax = Vector2.zero;

        if (!string.IsNullOrEmpty(loadFromPath))
            LoadImageFromDisk(loadFromPath);

        if (sourceImage == null) return;

        if (board == null || tray == null) return;

        BuildPuzzle();
    }

    void LoadImageFromDisk(string path)
    {
        if (!File.Exists(path)) return;

        byte[] data = File.ReadAllBytes(path);
        var tex = new Texture2D(2, 2);
        tex.LoadImage(data);
        sourceImage = tex;
    }

    void BuildPuzzle()
    {
        float boardW = board.rect.width;
        float boardH = board.rect.height;

        float cellW = boardW / columns;
        float cellH = boardH / rows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var go = new GameObject($"Piece_{j}_{i}", typeof(RectTransform));
                allPiecesImages.Add(go);

                var pieceRect = go.GetComponent<RectTransform>();

                pieceRect.SetParent(pieceLayer, false);
                pieceRect.anchorMin = pieceRect.anchorMax = new Vector2(0.5f, 0.5f);
                pieceRect.pivot = new Vector2(0.5f, 0.5f);
                pieceRect.sizeDelta = new Vector2(cellW, cellH);

                var img = go.AddComponent<RawImage>();
                img.texture = sourceImage;
                img.uvRect = new Rect(
                    (float)j / columns,
                    (float)i / rows,
                    1f / columns,
                    1f / rows
                );

                if (showBorders)
                {
                    var outline = go.AddComponent<Outline>();
                    outline.effectColor = new Color(0f, 0f, 0f, 0.6f);
                    outline.effectDistance = new Vector2(2f, 2f);
                }

                go.AddComponent<CanvasGroup>();

                var drag = go.AddComponent<PuzzlePieceDrag>();
                drag.SnapDistance = snapDistance;

                Vector3 cellLocal = new Vector3(
                    board.rect.xMin + cellW * (j + 0.5f),
                    board.rect.yMin + cellH * (i + 0.5f),
                    0f
                );

                Vector3 cellWorld = board.TransformPoint(cellLocal);

                drag.TargetPosition = WorldToLayer(cellWorld);

                pieceRect.anchoredPosition = RandomPointInTray();
            }
        }
    }

    Vector2 RandomPointInTray()
    {
        float x = Random.Range(tray.rect.xMin, tray.rect.xMax);
        float y = Random.Range(tray.rect.yMin, tray.rect.yMax);

        Vector3 world = tray.TransformPoint(new Vector3(x, y, 0f));

        return WorldToLayer(world);
    }

    Vector2 WorldToLayer(Vector3 world)
    {
        Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? canvas.worldCamera
            : null;

        Vector2 screen = RectTransformUtility.WorldToScreenPoint(cam, world);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            pieceLayer,
            screen,
            cam,
            out Vector2 local
        );

        return local;
    }

    public void WinPuzzle()
    {
        StartCoroutine(WinFleshRoutine());
    }

    IEnumerator WinFleshRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        winImgPanel.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        winImgPanel.SetActive(false);

        imgCounter++;
        columns++;
        rows++;

        foreach (GameObject elem in allPiecesImages)
        {
            Destroy(elem);
        }

        allPiecesImages.Clear();
        listPieceDrag.Clear();

        Start();
    }
}