using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    string levelName = "Scenes/Level";
    public int Level;//{get; private set;}
    public int MaxLevel = 9;
    void Awake()
    {
       
        GetLevel();

    }

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

    private void GetLevel()
    {
        string _level = (SaveData.Has(SaveData.Level)) ? SaveData.Level : "1";

        if (Int32.TryParse(SaveData.GetString(_level), out int j))
        {
            Level = j;
            
            return;
        }
        else
        {
            Level = 1;
           
        }
       
    }

    public void SetNextLevelAsActual()
    {
        if (Level < MaxLevel)
        {
            Level++;
            SaveData.Save(SaveData.Level, Level.ToString());
            
        }
        else//moe
        {
            Level = 1;
            SaveData.Save(SaveData.Level, Level.ToString());
        }
    }

    /*
    * Get current level from SaveData or 1 
    * and load Scene with this level;
    */
    public void LoadActualLevel()
    {
        string _level = levelName + Level.ToString();

        SceneManager.LoadScene(_level);
        return;
        //if(SceneManager.GetSceneByName(_level).IsValid()){
        //    Debug.Log("LoadActualLevel: Load <" + _level + "> Scene");
        //    SceneManager.LoadScene(_level);
        //} else {
        //    Debug.Log("LoadActualLevel: No <" + _level + "> Scene Found");
        //    SceneManager.LoadScene(levelName + "1");
        //}
    }

    // public void LoadLevel(int level) {
    //     string _level = levelName + level.ToString();
    //     if(SceneManager.GetSceneByName(_level).IsValid()){
    //         SceneManager.LoadScene(_level);
    //     } else {
    //         SceneManager.LoadScene(level + "1");
    //     }   
    // }
}
