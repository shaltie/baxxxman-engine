using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private List<Text> _healthText;
    [SerializeField] private List<Text> _cristalText;
    [SerializeField] private List<Text> _levelText;
    [SerializeField] private Text _nextLevelText;
    [SerializeField] private Text _accelerateText;
    [SerializeField] private Text _biteText;
    [SerializeField] private Text _shieldText;
    [SerializeField] private Button _nextLevelButton;

    public void BoostBite()
    {
        SaveBoost(SaveData.Bite);
    }

    public void BoostAccelerate()
    {
        SaveBoost(SaveData.Accelerate);
    }

    public void ShowResult()
    {
        foreach (var levelText in _levelText)
            levelText.text = string.Format(levelText.text, SaveData.Level);

        foreach (var healthText in _healthText)
            healthText.text = "Health Count: " + _gameManager.lives;

        foreach (var cristalText in _cristalText)
            cristalText.text = $"Gems: {_gameManager.Count}/{_gameManager.MaxCount}";

        _nextLevelText.text = string.Format(_nextLevelText.text, SaveData.Level);
        _accelerateText.text = SaveData.GetInt(SaveData.Accelerate).ToString();
        _biteText.text = SaveData.GetInt(SaveData.Bite).ToString();
        _shieldText.text = SaveData.GetInt(SaveData.Shield).ToString();
    }

    private void SaveBoost(string key)
    {
        int count = 0;

        if (SaveData.Has(key))
            count = SaveData.GetInt(key);

        SaveData.Save(key, count + 1);
        _nextLevelButton.interactable = true;
    }
}
