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
    [SerializeField] private Button _nextLevelButton;

    public void BoostHealth()
    {
        int health = 0;

        if (SaveData.Has(SaveData.Health))
            health = SaveData.GetInt(SaveData.Health);
        else
            health = _gameManager.LiveCount;

        SaveData.Save(SaveData.Health, health + _gameManager.NextHealthCount);
        _nextLevelButton.interactable = true;
    }

    public void BoostSpeed()
    {
        float speed = 0;

        if (SaveData.Has(SaveData.Speed))
            speed = SaveData.GetFloat(SaveData.Speed);
        else
            speed = FindObjectOfType<Hero>(true).movement.speed;

        SaveData.Save(SaveData.Speed, speed + _gameManager.NextSpeed);
        _nextLevelButton.interactable = true;
    }

    public void ShowResult()
    {
        foreach (var levelText in _levelText)
            levelText.text = string.Format(levelText.text, _gameManager.Level);

        foreach (var healthText in _healthText)
            healthText.text = "Health Count: " + _gameManager.lives;

        foreach (var cristalText in _cristalText)
            cristalText.text = $"{_gameManager.Count}/{_gameManager.MaxCount}";

        _nextLevelText.text = string.Format(_nextLevelText.text, _gameManager.Level + 1);
        _accelerateText.text = SaveData.GetInt(SaveData.Accelerate).ToString();
        _biteText.text = SaveData.GetInt(SaveData.Bite).ToString();
    }
}
