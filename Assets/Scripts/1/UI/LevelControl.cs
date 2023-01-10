using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelControl : MonoBehaviour
{
    [SerializeField] private LevelViewInfo _template;
    [SerializeField] private Transform _target;
    [SerializeField] private int _levelCount;
    [SerializeField] private int Level;
    public static int CurrentLevel { get; private set; }
    public static int NumberOfPassedLevels { get; private set; }

    private void Start()
    {
       
        NumberOfPassedLevels = GetLevelNew();
        Setup();
    }

    private void Setup()
    {
        for (int level = 1; level <= _levelCount; level++)
        {
            LevelViewInfo createdTemplate = Instantiate(_template, _target);
            createdTemplate.Init(level0 => CurrentLevel = level, level);

            if (level > NumberOfPassedLevels)
            {
                if (createdTemplate.GetComponent<Button>() != null)
                    createdTemplate.GetComponent<Button>().interactable = false;
               // createdTemplate.Hide();
            }

           
        }
    }

    private int GetLevel()
    {
        string level = SaveData.Has(SaveData.Level) ? SaveData.Level : "1";
        int countLevel = SaveData.GetInt(SaveData.Level);
        Debug.Log("countLevel" +countLevel);
        /* if (int.TryParse(SaveData.GetString(level), out int result))
             return result;*/

       
        return countLevel;
    }
    private int GetLevelNew()
    {
        string _level = (SaveData.Has(SaveData.Level)) ? SaveData.Level : "1";

        if (System.Int32.TryParse(SaveData.GetString(_level), out int j))
        {
            Level = j;

            return Level;
        }
        else
        {
            Level = 1;
            return Level;
        }

    }
}
