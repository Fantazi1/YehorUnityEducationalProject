using UnityEngine;
using UnityEngine.UI;

public class UIManagerTetris : MonoBehaviour
{
    public Text scoreText;
    public Text linesText;
    public Text gameOverScoreText;
    
    public GameObject gameOverPanel;
    public Button restartButton;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManagerTetris.Instance.RestartGame());
    }

    public void UpdateScore(int score, int linesCleared)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
        if (linesText != null)
            linesText.text = $"Lines: {linesCleared}";
    }

    public void ShowGameOver(int score, int linesCleared)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverScoreText != null)
            gameOverScoreText.text = $"Game Over!\n\nScore: {score}\nLines: {linesCleared}";
    }
}
