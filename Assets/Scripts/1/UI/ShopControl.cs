using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    [SerializeField] private GameObject _window;
    [SerializeField] private GameObject _nextWindow;
    [SerializeField] private Text _gems;

    private const int _acceleratePrice = 200;
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
        if (SaveData.Has(SaveData.Gems))
        {
            int gemsCount = SaveData.GetInt(SaveData.Gems);

            if (gemsCount >= _acceleratePrice)
            {
                gemsCount -= _acceleratePrice;
                UpdateGems(gemsCount);
                AddAccelerate();
                SaveData.Save(SaveData.Gems, gemsCount);
            }
        }
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

    private void AddAccelerate()
    {
        if (SaveData.Has(SaveData.Accelerate))
        {
            int accelerateCount = SaveData.GetInt(SaveData.Accelerate);
            accelerateCount++;
            SaveData.Save(SaveData.Accelerate, accelerateCount);
        }
        else
        {
            SaveData.Save(SaveData.Accelerate, 1);
        }
    }

    private void UpdateGems(int value) => _gems.text = value.ToString();
}
