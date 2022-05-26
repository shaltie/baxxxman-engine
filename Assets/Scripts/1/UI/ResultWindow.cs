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
    [SerializeField] private Button _nextLevelButton;

    public void SelectBoost()
    {
        _nextLevelButton.interactable = true;
    }

    public void ShowResult()
    {
        Debug.LogWarning(_gameManager.Level);

        foreach (var levelText in _levelText)
            levelText.text = string.Format(levelText.text, _gameManager.Level);

        foreach (var healthText in _healthText)
            healthText.text = "Health Count: " + _gameManager.lives;

        foreach (var cristalText in _cristalText)
            cristalText.text = $"{_gameManager.Count}/{_gameManager.MaxCount}";

        _nextLevelText.text = string.Format(_nextLevelText.text, _gameManager.Level + 1);
    }
}
