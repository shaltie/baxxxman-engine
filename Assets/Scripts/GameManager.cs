using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultWindow _result;
    [SerializeField] private List<Map> _maps;
    [SerializeField] private float _nextSpeed;
    [SerializeField] private int _nextHealthCount;
    [SerializeField] private LevelManager _levelManager;

    public int LiveCount = 3;

    private Guardin[] guardins;
    public Hero hero;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;

    [SerializeField] private UnityEvent _winGame;
    [SerializeField] private UnityEvent _gameOver;

    public int guardinMultiplier { get; private set; } = 1;

    public int Count  { get; private set; }
    public int lives { get; private set; }
    public int MaxCount { get; private set; }
    // public int Level { get; private set; }
    public float NextSpeed => _nextSpeed;
    public int NextHealthCount => _nextHealthCount;

    private void Awake() {}

    private void Start()
    {
        SetupLevel();
        NewGame();

        _result.ShowResult();
    }

    private void SetupLevel()
    {

        int localLevel = 0; // Now al scenes must have only 1 level in _maps; @TODO add multiple local levels logic

        
        
        Map mapScript = _maps[localLevel].GetComponent<Map>();
        Transform mapCoins = mapScript.mapCoins;

        foreach (Transform coin in mapCoins)
        {
            coin.gameObject.SetActive(true);
        }

        guardins = _maps[localLevel].Guardins.ToArray();
        
    }

    public void SaveNextLevel()
    {
        _levelManager.LoadActualLevel();
    }

    public void Accelerate()
    {
        if (TrySpendBoost(SaveData.Accelerate))
            FindObjectOfType<Hero>().movement.PlayAccelerate(() => _result.ShowResult());
    }

    public void Bite()
    {
        if (TrySpendBoost(SaveData.Bite) && IsFollow() == false)
        {
            int biteCount = SaveData.GetInt(SaveData.Bite) - 1;
            SaveData.Save(SaveData.Bite, biteCount);
            _result.ShowResult();

            Transform bite = FindObjectOfType<Hero>().CreateBite();

            foreach (var guardin in guardins)
                guardin.Follow(bite);
        }
    }

    public void StopBite()
    {
        foreach (var guardin in guardins)
            guardin.StopFollow();
    }

    private bool IsFollow()
    {
        return guardins.All(guardin => guardin._isFollow);
    }

    public void HideAllGuardin()
    {
        foreach (var guardin in guardins)
        {
            Destroy(guardin.gameObject);
        }

        guardins = null;
    }

    

    private void NewGame()
    {
        SetScore(0);
        SetLives(GetLive());
        NewRound();

        _result.ShowResult();
        Invoke(nameof(SetMaxBax), 0.1f);
    }

    private int GetLive()
    {
        if (SaveData.Has(SaveData.Health))
            return SaveData.GetInt(SaveData.Health);

        return LiveCount;
    }

    private void SetMaxBax()
    {
        MaxCount = FindObjectsOfType<Bax>(true).ToList().Count;
        _result.ShowResult();
    }

    private void NewRound()
    {

        Debug.Log("New round");
        // gameOverText.enabled = false;

        ResetState();
        SetGuardinMode("scatter");
    }

    private void ResetState()
    {
        ResetGuardinMultiplier();

        if (guardins == null)
            return;

        for (int i = 0; i < this.guardins.Length; i++)
        {
            this.guardins[i].ResetState();
        }

        this.hero.ResetState();
    }

    private void GameOver()
    {
        _gameOver?.Invoke();
        //gameOverText.enabled = true;

        if (guardins == null)
            return;

        for (int i = 0; i < this.guardins.Length; i++)
        {
            this.guardins[i].gameObject.SetActive(false);
        }

        this.hero.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        _result.ShowResult();
        // livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.Count = score;
        _result.ShowResult();
        // scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void HeroCaught()
    {
        this.hero.DeathSequence();

        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void GuardinCaught(Guardin guardin)
    {
        int points = guardin.points * this.guardinMultiplier;
        SetScore(this.Count + points);
        this.guardinMultiplier++;
    }

    public void Win() {
        _levelManager.SetNextLevelAsActual(); // Change Level to next
        _winGame?.Invoke();
    }

    public void BaxCollected(Bax bax)
    {
        bax.gameObject.SetActive(false);
        //SetScore(this.Count + bax.points);
        SetScore(this.Count + 1);

        SaveData.Save(SaveData.Gems, SaveData.GetInt(SaveData.Gems) + 1);

        _result.ShowResult();
    }

    public void BaxMajorCollected(BaxMajor baxMajor)
    {
        BaxCollected(baxMajor);
        CancelInvoke();
        Invoke(nameof(ResetGuardinMultiplier), baxMajor.duration);
    }

    private void ResetGuardinMultiplier()
    {
        this.guardinMultiplier = 1;
    }

    

    

    private bool TrySpendBoost(string key)
    {
        if (SaveData.Has(key))
        {
            int count = SaveData.GetInt(key);
            return count > 0;
        }
        else
        {
            return false;
        }
    }

    public void SetGuardinMode(string mode) {
        SaveData.Save(SaveData.GuardinMode, mode);
    }
}
