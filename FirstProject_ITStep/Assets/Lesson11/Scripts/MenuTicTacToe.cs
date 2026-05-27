using UnityEngine;
using UnityEngine.UI;

public class MenuTicTacToe : MonoBehaviour
{
    [SerializeField] private Button _startGame;
    [SerializeField] private Button _exitGame;
    [SerializeField] private Canvas _gameMenu;
    [SerializeField] private Button _backToMenu;
    [SerializeField] private GameObject _panelMenu;
    [SerializeField] private GameObject _panelWinLose;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;
    
    void Start()
    {
        _startGame.onClick.AddListener(StartGameHandler);
        _exitGame.onClick.AddListener(ExitGameHandler);
        _backToMenu.onClick.AddListener(BackToMenuHandler);
    }

    private void StartGameHandler()
    {
        _panelMenu.SetActive(false);
    }

    private void BackToMenuHandler()
    {
        _panelMenu.SetActive(true);
        _panelWinLose.SetActive(false);
    }

    private void ExitGameHandler()
    {
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif  
    }

    private void OnDestroy()
    {
        _startGame.onClick.RemoveAllListeners();
        _exitGame.onClick.RemoveAllListeners();
    }
    
    public void onWin()
    {
        _panelWinLose.SetActive(true);
        _winText.SetActive(true);
    }

    public void onLose()
    {
        _panelWinLose.SetActive(true);
        _loseText.SetActive(true);
    }
}
