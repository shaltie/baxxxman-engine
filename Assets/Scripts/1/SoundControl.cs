using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundControl : MonoBehaviour
{
    [SerializeField] private Toggle _soundToggle;

    private bool _isSoundActive = true;

    private void Awake()
    {
        if (SaveData.Has(SaveData.Sound))
            bool.TryParse(SaveData.GetString(SaveData.Sound), out _isSoundActive);

        SetupSound();
    }

    public void ChangeSoundState()
    {
        _isSoundActive = !_isSoundActive;
        SetupSound();
        SaveData.Save(SaveData.Sound, _isSoundActive.ToString());
    }

    private void SetupSound() => _soundToggle.SetIsOnWithoutNotify(_isSoundActive);
}
