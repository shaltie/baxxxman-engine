using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelViewInfo : MonoBehaviour
{
    [SerializeField] private Text _levelText;

    private const string _levelPrefix = "Level";

    private Action<int> _callback;
    private Button _button;
    private int _level;

    private void Awake()
    {
        if(GetComponent<Button>()!=null)
            _button = GetComponent<Button>();
    }

    public void Init(Action<int> callback, int level)
    {
        _callback = callback;
        _level = level;
        _levelText.text = _levelPrefix + level;
    }

    public void LoadLevel()
    {
        _callback?.Invoke(_level);
        SceneManager.LoadScene(_levelPrefix + _level);
    }

    public void Hide()
    {
        if (_button != null)
            _button.interactable = false;
    }
}
