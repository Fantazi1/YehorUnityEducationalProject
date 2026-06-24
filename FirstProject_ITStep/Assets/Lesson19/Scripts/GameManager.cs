using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Слова та мова")]
    [SerializeField] private string[] ukrainianWords = { "програміст", "юніті", "архітектура", "скрипт", "всесвіт", "краєвид" };
    [SerializeField] private string[] englishWords = { "developer", "unity", "architecture", "script", "universe", "landscape" };

    [Header("UI Елементи")]
    [SerializeField] private Text wordDisplayPoints; 
    [SerializeField] private Image hangmanImage;
    [SerializeField] private GameObject loserImage;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject allGamePanel;

    [Header("Налаштування балансу")]
    [SerializeField] private int maxErrors = 6;      

    private string currentWord;
    private char[] hiddenWordProgress;
    private int wrongGuessesCount = 0;
    private bool isGameActive = true;

    void Start()
    {
        StartNewGame(true);
    }

    public void StartNewGame(bool isUkrainian)
    {
        wrongGuessesCount = 0;
        isGameActive = true;

        if (hangmanImage != null)
        {
            hangmanImage.fillAmount = 0f;
        }

        string[] selectedList = isUkrainian ? ukrainianWords : englishWords;
        int randomIndex = Random.Range(0, selectedList.Length);
        currentWord = selectedList[randomIndex].ToLower();

        hiddenWordProgress = new char[currentWord.Length];
        for (int i = 0; i < hiddenWordProgress.Length; i++)
        {
            hiddenWordProgress[i] = '_';
        }

        UpdateWordDisplay();

        AlphabetButton[] allButtons = FindObjectsByType<AlphabetButton>(FindObjectsSortMode.None);
        for (int i = 0; i < allButtons.Length; i++)
        {
            allButtons[i].ResetButton();
        }
    }

    public void GuessLetter(char letter)
    {
        if (!isGameActive) return;

        char lowerLetter = char.ToLower(letter);
        bool foundMatch = false;

        for (int i = 0; i < currentWord.Length; i++)
        {
            if (currentWord[i] == lowerLetter)
            {
                hiddenWordProgress[i] = currentWord[i];
                foundMatch = true;
            }
        }

        if (foundMatch)
        {
            UpdateWordDisplay();
            CheckWinCondition();
        }
        else
        {
            RegisterWrongGuess();
        }
    }

    private void RegisterWrongGuess()
    {
        wrongGuessesCount++;

        if (hangmanImage != null)
        {
            float progress = (float)wrongGuessesCount / maxErrors;
            hangmanImage.fillAmount = progress;
        }

        if (wrongGuessesCount >= maxErrors)
        {
            isGameActive = false;
            wordDisplayPoints.text = "Ви програли! Слово: " + currentWord;

            if (hangmanImage != null) hangmanImage.fillAmount = 1f;
            Debug.Log("Програш. Шибениця відображена повністю.");

            loserImage.SetActive(true);
        }
    }

    private void CheckWinCondition()
    {
        for (int i = 0; i < hiddenWordProgress.Length; i++)
        {
            if (hiddenWordProgress[i] == '_') return;
        }

        isGameActive = false;
        wordDisplayPoints.text = "Перемога! Слово: " + currentWord;
        Debug.Log("Перемога! Слово відгадано.");
        winImage.SetActive(true);
        allGamePanel.SetActive(false);
    }

    private void UpdateWordDisplay()
    {
        string displayString = "";
        for (int i = 0; i < hiddenWordProgress.Length; i++)
        {
            displayString += hiddenWordProgress[i] + " ";
        }
        wordDisplayPoints.text = displayString;
    }
}