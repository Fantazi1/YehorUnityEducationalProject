using UnityEngine;
using UnityEngine.UI;

public class CoinsGather : MonoBehaviour
{
    [SerializeField] private int _coins;
    [SerializeField] private Text coinsTextUI;

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
