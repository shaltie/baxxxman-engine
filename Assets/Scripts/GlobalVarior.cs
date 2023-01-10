using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarior : MonoBehaviour
{
    // State variables
    [SerializeField] private int _currentLevelMax= 1;
    [SerializeField] private float _KoefSpeedHero = 1;
    void Awake()
    {
        
        int objectsCount = FindObjectsOfType<GlobalVarior>().Length;

        if (objectsCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SetCurrentLevelMax(int level)
    {
        _currentLevelMax = level;
        Debug.Log("SETLEVELGLOBAL = " + _currentLevelMax);
    }
    public int GetCurrentLevelMax()
    {
        Debug.Log("GETLEVELGLOBAL = " + _currentLevelMax);
        return _currentLevelMax;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
