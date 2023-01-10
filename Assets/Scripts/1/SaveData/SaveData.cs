using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SaveData
{
    public const string Shield = nameof(Shield);
    public const string Gems = nameof(Gems);
    public const string Bite = nameof(Bite);
    public const string Accelerate = nameof(Accelerate);
    public const string Sound = nameof(Sound);
    public const string Level = nameof(Level);
    public const string GuardinMode = nameof(GuardinMode);
    public const string Dash = nameof(Dash);
    public const string SpeedUp = nameof(SpeedUp);
    public const string KoefSpeedHero = nameof(KoefSpeedHero);

    public static bool Has(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void Save(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static void Save(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static void Save(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public static int GetInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public static float GetFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
}
