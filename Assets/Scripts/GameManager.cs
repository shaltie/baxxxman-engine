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

    public Guardin[] guardins;
    public Hero hero;
    public Transform baxes;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;

    [SerializeField] private UnityEvent _winGame;
    [SerializeField] private UnityEvent _gameOver;

    public int guardinMultiplier { get; private set; } = 1;

    public int Count  { get; private set; }
    public int lives { get; private set; }
    public int MaxCount { get; private set; }
    public int Level { get; private set; }

    private void Awake()
    {
        if (SaveData.Has(SaveData.Level))
            Level = SaveData.GetInt(SaveData.Level);
        else
            Level = 1;
    }

    private void Start()
    {
        SetupLevel(Level);
        NewGame();

        _result.ShowResult();
    }

    private void Update()
    {
        //if (lives <= 0 && Input.anyKey)
        //{
        //    NewGame();
        //}
    }

    public void Accelerate()
    {
        if (TrySpendAccelerate())
            FindObjectOfType<Hero>().movement.PlayAccelerate(() => _result.ShowResult());
    }

    public void SaveNextLevel()
    {
        Level++;
        SaveData.Save(SaveData.Level, Level);
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();

        _result.ShowResult();
        Invoke(nameof(SetMaxBax), 0.1f);
    }

    private void SetMaxBax()
    {
        MaxCount = FindObjectsOfType<Bax>(true).ToList().Count;
        _result.ShowResult();
    }

    private void NewRound()
    {
        // gameOverText.enabled = false;
        foreach (Transform bax in this.baxes)
        {
            bax.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGuardinMultiplier();
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

    public void Win() => _winGame?.Invoke();

    public void BaxCollected(Bax bax)
    {
        bax.gameObject.SetActive(false);
        //SetScore(this.Count + bax.points);
        SetScore(this.Count + 1);
        _result.ShowResult();

        if (!HasRemainingBax())
        {
            //_winGame?.Invoke();
            //this.hero.gameObject.SetActive(false);
            //Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void BaxMajorCollected(BaxMajor baxMajor)
    {
        BaxCollected(baxMajor);
        CancelInvoke();
        Invoke(nameof(ResetGuardinMultiplier), baxMajor.duration);
    }

    private bool HasRemainingBax()
    {
        foreach (Transform bax in this.baxes)
        {
            if (bax.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGuardinMultiplier()
    {
        this.guardinMultiplier = 1;
    }

    private void SetupLevel(int index)
    {
        ResetAllMap();

        if (index < 0)
            index = 0;

        if (_maps.Count < index)
            index = _maps.Count - 1;
        else
            index--;

        baxes = _maps[index].Money;
        guardins = _maps[index].Guardins.ToArray();
        _maps[index].gameObject.SetActive(true);
    }

    private void ResetAllMap()
    {
        foreach (var map in _maps)
            map.gameObject.SetActive(false);
    }

    private bool TrySpendAccelerate()
    {
        if (SaveData.Has(SaveData.Accelerate))
        {
            int accelerateCount = SaveData.GetInt(SaveData.Accelerate);
            return accelerateCount > 0;
        }
        else
        {
            return false;
        }
    }
}
