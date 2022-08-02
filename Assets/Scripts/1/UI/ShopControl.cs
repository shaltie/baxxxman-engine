using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private GameObject _nextWindow;
    [SerializeField] private Text _gems;

    private const int _acceleratePrice = 50;
    private const int _bitePrice = 50;
    private const int _shieldPrice = 50;
    private bool _isNextWindow = false;

    private void Start()
    {
        if (SaveData.Has(SaveData.Gems))
        {
            int gemsCount = SaveData.GetInt(SaveData.Gems);
            UpdateGems(gemsCount);
        }
        else
        {
            UpdateGems(0);
        }
    }

    public void SetNextWindow(bool isNextWindow) => _isNextWindow = isNextWindow;

    public void ShowWindow()
    {
        if (_isNextWindow)
            _nextWindow.SetActive(true);
        else
            _window.SetActive(true);
    }

    public void TryBuyAccelerate()
    {
        TryBuy(SaveData.Accelerate, _acceleratePrice, UpdateGems);
    }

    public void TryBuyBite()
    {
        TryBuy(SaveData.Bite, _bitePrice, UpdateGems);
    }

    public void TryBuyShield()
    {
        TryBuy(SaveData.Shield, _shieldPrice, UpdateGems);
    }

    public void BuyGems(int value)
    {
        if (SaveData.Has(SaveData.Gems))
        {
            int gemsCount = SaveData.GetInt(SaveData.Gems);
            gemsCount += value;
            UpdateGems(gemsCount);
            SaveData.Save(SaveData.Gems, gemsCount);
        }
        else
        {
            UpdateGems(value);
            SaveData.Save(SaveData.Gems, value);
        }
    }

    public void UpdateGems() => _gems.text = SaveData.GetInt(SaveData.Gems).ToString();

    private void TryBuy(string key, int price, UnityAction<int> callback)
    {
        if (SaveData.Has(SaveData.Gems))
        {
            int gems = SaveData.GetInt(SaveData.Gems);

            if (gems >= price)
            {
                gems -= price;
                callback?.Invoke(gems);
                Add(key);
                SaveData.Save(SaveData.Gems, gems);
            }
        }
    }

    private void Add(string key)
    {
        if (SaveData.Has(key))
        {
            int count = SaveData.GetInt(key);
            count++;
            SaveData.Save(key, count);
        }
        else
        {
            SaveData.Save(key, 1);
        }
    }

    private void UpdateGems(int value) => _gems.text = value.ToString();
}
