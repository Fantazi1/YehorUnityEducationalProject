using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TetrisStartExit : MonoBehaviour
{
    [SerializeField] private GameObject _gameManager;
    [SerializeField] private GameObject _scoreText;
    [SerializeField] private GameObject _linesText;
    [SerializeField] private GameObject _startGameButtonGmObj;
    [SerializeField] private GameObject _exitButtonGmObj;
    [SerializeField] private GameObject _startGameCanvas;
    private Button startGameButton;
    void Start()
    {
        startGameButton = _startGameButtonGmObj.GetComponent<Button>();
        startGameButton.onClick.AddListener(OnButtonClickedStartGame);
        _exitButtonGmObj.GetComponent<Button>().onClick.AddListener(OnButtonClickedExit);
    }

    private void OnButtonClickedStartGame()
    {
        _gameManager.SetActive(true);
        _startGameCanvas.SetActive(false);
        _scoreText.SetActive(true);
        _linesText.SetActive(true);
    }

    private void OnButtonClickedExit()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
