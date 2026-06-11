using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu2DGameManager : MonoBehaviour
{
    [SerializeField] private Button[] buttonsStartGameChoosingGrid;
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _exitGame;
    [SerializeField] private PuzzleManager puzzleManager;
    [SerializeField] private GameObject chooseGridPanel;
    [SerializeField] private GameObject menuPanel;
    private int gridSize = 4;

    void Start()
    {
        _startGame.onClick.AddListener(StartGame);
        _exitGame.onClick.AddListener(ExitGame);
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { 
            ExitGame();
        }
    }

    private void StartGame()
    {
        menuPanel.SetActive(false);
        chooseGridPanel.SetActive(true);

        foreach (Button btn in buttonsStartGameChoosingGrid)
        {
            int tempGridSize = gridSize;
            btn.onClick.AddListener(() => GridSizeVal(tempGridSize));
            gridSize++;
        }
    }

    private void GridSizeVal(int gridSize)
    {
        puzzleManager.enabled = true;
        puzzleManager._SIZE_Acces = gridSize;
        chooseGridPanel.SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
