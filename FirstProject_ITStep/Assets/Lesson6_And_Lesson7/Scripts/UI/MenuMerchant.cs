using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMerchant : MonoBehaviour
{
    [SerializeField] private GameObject _btnBuy;
    [SerializeField] private GameObject _btnCancel;
    [SerializeField] private CoinsGatherer _coinsGather;

    public void BuyInShop()
    {
        if(_coinsGather.Coins >= 5) { 
            _coinsGather.Coins -= 5;
        }
    }

    public void CancelInShop()
    {
        
    }
}
