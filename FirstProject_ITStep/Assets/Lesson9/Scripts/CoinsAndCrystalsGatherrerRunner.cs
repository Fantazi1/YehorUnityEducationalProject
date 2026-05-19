using TMPro;
using UnityEngine;

public class CoinsAndCrystalsGatherrerRunner : MonoBehaviour
{
    [SerializeField] private int _collectedCoins;
    [SerializeField] private int _collectedCrystals;
    [SerializeField] private TMP_Text _textCoins;
    [SerializeField] private TMP_Text _textCrystals;

    public int _CollectedCoins {
        get { 
            return _collectedCoins; 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CoinsTag")
        {
            _collectedCoins++;
            Destroy(other.gameObject); 
            CoinsTextWriter();
        }
        if (other.gameObject.tag == "CrystalTag")
        {
            _collectedCrystals++;
            Destroy(other.gameObject);
            CrystalsTextWriter();
        }
    }

    private void CoinsTextWriter()
    {
        _textCoins.text = _collectedCoins.ToString();
    }

    private void CrystalsTextWriter()
    {
        _textCrystals.text = _collectedCrystals.ToString();
    }
}
