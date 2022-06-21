using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    string levelName = "level";
    public void LoadLevel(int level) {
        SceneManager.LoadScene(levelName + level);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void LoadGameOver() {
        SceneManager.LoadScene("GameOver");
    }

    public void ReloadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void LoadScene(string name) => SceneManager.LoadScene(name);
}
