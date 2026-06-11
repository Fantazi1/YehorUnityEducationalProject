using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    public int Number;

    public TextMeshProUGUI NumberText;

    public void SetNumber(int number)
    {
        Number = number;
        NumberText.text = Number.ToString();
    }
}