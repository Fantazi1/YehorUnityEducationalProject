using UnityEngine;
using UnityEngine.UI;

public class CoinsGatherer : MonoBehaviour
{
    [SerializeField] private int _coins;
    [SerializeField] private Text coinsTextUI;

    public int Coins {  get { return _coins; } set { _coins = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CoinsTag")
        {
            _coins++;
            coinsTextUI.text = _coins.ToString();
            Destroy(other.gameObject);
        }
    }
}
