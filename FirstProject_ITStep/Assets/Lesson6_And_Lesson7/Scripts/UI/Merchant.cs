using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Collections;

public class Merchant : MonoBehaviour
{
    [Header("Allowing player to click and see balance")]
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _coinsShopBalanceTextUI;
    [SerializeField] private Text _coinsTextUI;
    [SerializeField] private CoinsGatherer _coinsGatherer;
    [SerializeField] private GameObject _shopPanel;
    [SerializeField] private FirstPersonController _firstPersonController;

    private bool isMenuOpen = false;

    [Header("Shop functional buy and cancel")]
    [Space(10)]
    [SerializeField] private GameObject _btnBuy;
    [SerializeField] private GameObject _btnCancel;
    [SerializeField] private GameObject _thanksText;
    [SerializeField] private GameObject _notEnoughText;

    public void BuyInShop()
    {
        if (_coinsGatherer.Coins >= 5)
        {
            _coinsGatherer.Coins -= 5;
            _coinsShopBalanceTextUI.text = _coinsGatherer.Coins.ToString();
            _coinsTextUI.text = _coinsGatherer.Coins.ToString();
            showThanksForBuyingOrNotEnoughMoney(_thanksText);
        }
        else {
            showThanksForBuyingOrNotEnoughMoney(_notEnoughText);
        }
    }

    public void CancelInShop()
    {
        isMenuOpen = false;
        _shopPanel.SetActive(false);
        _firstPersonController.canRotate = true;
    }

    public void showThanksForBuyingOrNotEnoughMoney(GameObject _gameObject)
    {
        StartCoroutine(FlashRoutine(_gameObject));
    }

    private IEnumerator FlashRoutine(GameObject _gameObject)
    {
        _gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (isMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _firstPersonController.canRotate = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isMenuOpen = true;
            _shopPanel.SetActive(true);
            _coinsShopBalanceTextUI.text = _coinsGatherer.Coins.ToString();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isMenuOpen = false;
            _shopPanel.SetActive(false);
            _firstPersonController.canRotate = true;
        }
    }
}
