using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void Load(string name) => SceneManager.LoadScene(name);
    public void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void LoadCurrentLevel()
    {
        if (int.TryParse(SaveData.GetString(SaveData.Level), out int level))
        {
            SceneManager.LoadScene("Level" + level);
        }
        else
        {
            SceneManager.LoadScene("Level" + 1);
        }
    }
}
