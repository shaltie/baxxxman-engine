using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    [SerializeField] private LevelViewInfo _template;
    [SerializeField] private Transform _target;
    [SerializeField] private int _levelCount;

    public static int CurrentLevel { get; private set; }
    public static int NumberOfPassedLevels { get; private set; }

    private void Start()
    {
        NumberOfPassedLevels = GetLevel();
        Setup();
    }

    private void Setup()
    {
        for (int level = 1; level <= _levelCount; level++)
        {
            LevelViewInfo createdTemplate = Instantiate(_template, _target);
            createdTemplate.Init(level0 => CurrentLevel = level, level);

            if (level >= NumberOfPassedLevels)
                createdTemplate.Hide();

           
        }
    }

    private int GetLevel()
    {
        string level = SaveData.Has(SaveData.Level) ? SaveData.Level : "1";

        if (int.TryParse(SaveData.GetString(level), out int result))
            return result;

        return 1;
    }
}
