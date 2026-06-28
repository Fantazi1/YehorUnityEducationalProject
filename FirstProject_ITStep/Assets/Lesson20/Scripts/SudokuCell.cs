using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    public TMP_Text numberText;
    public Image backgroundImage;

    [Header("Налаштування кольорів")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.8f, 1f, 0.8f);
    public Color lockedColor = new Color(0.9f, 0.9f, 0.9f);
    public Color errorTextColor = Color.red;
    public Color botColor = new Color(1f, 0.9f, 0.9f); 

    public int Row { get; private set; }
    public int Col { get; private set; }
    public int Value { get; private set; }
    public int CorrectValue { get; private set; }
    public bool IsLocked { get; private set; }

    private SudokuManager _manager;
    private bool _isBotCell;

    public void Init(int row, int col, int correctValue, bool hideOriginal, SudokuManager manager, bool isBotCell)
    {
        Row = row;
        Col = col;
        CorrectValue = correctValue;
        _manager = manager;
        _isBotCell = isBotCell;

        if (!hideOriginal)
        {
            Value = correctValue;
            IsLocked = true;
            backgroundImage.color = lockedColor;
        }
        else
        {
            Value = 0;
            IsLocked = false;
            backgroundImage.color = _isBotCell ? botColor : normalColor;
        }

        UpdateText();

        if (!_isBotCell)
        {
            GetComponent<Button>().onClick.AddListener(() => _manager.SelectCell(this));
        }
    }

    public void SetValue(int newValue)
    {
        if (IsLocked) return;
        Value = newValue;
        UpdateText();
    }

    public void SetSelected(bool isSelected)
    {
        if (IsLocked || _isBotCell) return;
        backgroundImage.color = isSelected ? selectedColor : normalColor;
    }

    private void UpdateText()
    {
        if (Value == 0)
        {
            numberText.text = "";
        }
        else
        {
            numberText.text = Value.ToString();

            if (!IsLocked && Value != CorrectValue)
                numberText.color = errorTextColor;
            else
                numberText.color = Color.black;
        }
    }
}