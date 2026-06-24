using UnityEngine;
using UnityEngine.UI;

public class AlphabetButton : MonoBehaviour
{
    [SerializeField] private char letter; // Сюди в інспекторі для кожної кнопки пишемо її літеру (а, б, в...)
    private Button button;
    private GameManager gameManager;

    void Awake()
    {
        button = GetComponent<Button>();
        gameManager = FindFirstObjectByType<GameManager>();

        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        gameManager.GuessLetter(letter);

        button.interactable = false;
    }

    public void ResetButton()
    {
        button.interactable = true;
    }
}