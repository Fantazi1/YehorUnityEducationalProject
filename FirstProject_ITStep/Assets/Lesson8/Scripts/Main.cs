using System;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    [SerializeField] private int _coins;

    [SerializeField] private Text _coinsText;
    [SerializeField] private float _countCoinsClick = 1;

    [SerializeField] private float[] priceUp;
    [SerializeField] private float[] lvlUp;

    [SerializeField] private Text[] priceUpText;
    [SerializeField] private Text[] lvlUpText;

    [SerializeField] private GameObject _shopPanel;

    void Start()
    {
        _coinsText.text = _coins.ToString() + "$";  
    }

    public void AddCoins()
    {
        _coins += (int) _countCoinsClick;
        Start();
    }

    public void ShowShopPanel()
    {
        _shopPanel.SetActive(!_shopPanel.activeSelf);
    }

    public void BuyUp(int index)
    {
        if (_coins >= priceUp[index])
        {
            _coins -= (int) priceUp[index];
            Start();
            switch (index) { 
                case 0: _countCoinsClick *= 1.2f; priceUp[index] *= 1.5f; break;
                case 1: _countCoinsClick *= 1.5f; priceUp[index] *= 2.3f; break;
                case 2: _countCoinsClick *= 2f; priceUp[index] *= 3.1f; break;
                default: break;
            }

            lvlUp[index]++;

            priceUpText[index].text = Convert.ToInt32(priceUp[index]).ToString();
            lvlUpText[index].text = Convert.ToInt32(lvlUp[index]).ToString();
        }
    }
}
