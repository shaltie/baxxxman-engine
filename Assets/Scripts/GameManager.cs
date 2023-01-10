using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ResultWindow _result;
    [SerializeField] private float _nextSpeed;
    [SerializeField] private int _nextHealthCount;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private UnityEvent _winGame;
    [SerializeField] private UnityEvent _gameOver;
   // [SerializeField] private int SHILD_COUNT = 3;
   // public Text CountShildText = null;
    [SerializeField] private const float _playShieldTime = 10;
    [SerializeField] private Transform _wall;
    [SerializeField] private Coroutine _shieldPlayJob;
    [SerializeField] private Coroutine _shieldPlayJobSecond;

    public int LiveCount = 3;

    private Guardin[] guardins;
    public Hero hero;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;
    public int MacShild = 6;
    public float KoefSpeedReed = 0;
    public bool IsPlayShield { get; private set; } = false;
    public int guardinMultiplier { get; private set; } = 1;

    public int Count  { get; private set; }
    public int lives { get; private set; }
    public int MaxCount { get; private set; }
    // public int Level { get; private set; }
    public float NextSpeed => _nextSpeed;
    public int NextHealthCount => _nextHealthCount;
    //---
    public float maxTimeShild = 2f;
    public bool EnamShield = false;
    public float TimerShild = 2f;

    private void Start()
    {
       // SaveData.Save(SaveData.Shield, 4);
        SetupLevel();
        NewGame();
        _result.GenericBonusBut();
       // CountShildText.text = SHILD_COUNT.ToString();
        _result.ShowResult();
        Invoke(nameof(SetMaxBax), 0.1f);
        
    }
    private void Update()
    {
        PlayTemerShild();
    }
    private void SetupLevel()
    {
        //int localLevel = 0; // Now al scenes must have only 1 level in _maps; @TODO add multiple local levels logic
        Map mapScript = FindObjectOfType<Map>().GetComponent<Map>();
        _wall = mapScript.Wall;
        Transform mapCoins = mapScript.mapCoins;

        foreach (Transform coin in mapCoins)
            coin.gameObject.SetActive(true);

        guardins = mapScript.Guardins.ToArray();

        MacShild = SaveData.GetInt(SaveData.Shield);
        KoefSpeedReed = SaveData.GetInt(SaveData.KoefSpeedHero);
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

            SetGuardinMode("chase");

            foreach (var guardin in guardins)
                guardin.Follow(bite);
        }
    }

    public void UseShield()
    {
        if (TrySpendBoost(SaveData.Shield) && IsPlayShield == false)//--
        {
            if (SaveData.GetInt(SaveData.Shield) > 0)
            {
                int shieldCount = SaveData.GetInt(SaveData.Shield) - 1;
                SaveData.Save(SaveData.Shield, shieldCount);
            }
            // PlayShield(_playShieldTime);
            UsedPlayShield();
        }
    }

    public void PlayShield(float duration = _playShieldTime)
    {
        UsedPlayShield();
        /* if (_shieldPlayJob != null)
         {
             StopCoroutine(_shieldPlayJob);
             _shieldPlayJob = null;
         }

             _shieldPlayJob = StartCoroutine(WaitPlayShield(duration));
         if (IsPlayShield == false)
         _shieldPlayJobSecond = StartCoroutine(WaitPlayShield(duration));*/
    }

    public void UsedPlayShield()
    {
        EnamShield = true;
        if (TimerShild < maxTimeShild)
            TimerShild += maxTimeShild;
    }
    public void PlayTemerShild()
    {
        if(EnamShield)
        {
            if (TimerShild > 0)
            {
                TimerShild -= 1 * Time.deltaTime;
                //--
                _result.ShowResult();

                hero.PlayShieldAnimation(TimerShild);

                IsPlayShield = true;
                foreach (var guardin in guardins)
                    guardin.Collider.isTrigger = true;
               

                
                //--
            }
            else
            {
                IsPlayShield = false;
                foreach (var guardin in guardins)
                    guardin.Collider.isTrigger = false;

                TimerShild = maxTimeShild;
                EnamShield = false;

            }

        }
    }

    public void StopBite()
    {
        SetGuardinMode("scatter");

        foreach (var guardin in guardins)
            guardin.StopFollow();
    }

    public void HideObstacle()
    {
        if (_obstacle == null)
            return;

        _obstacle.SetActive(false);
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
        SetLives(LiveCount);
        NewRound();
        SetShilds(); //--moe
        
        //---
        //   int Level = 1;
        //   SaveData.Save(SaveData.Level, Level.ToString());
        //---
        _result.ShowResult();
        Invoke(nameof(SetMaxBax), 0.1f);
    }

    private void SetMaxBax()
    {
        MaxCount = FindObjectsOfType<Bax>(true).ToList().Count;
        SetScore(0);
        _result.ShowResult();
    }

    private void NewRound()
    {

        Debug.Log("New round");
        // gameOverText.enabled = false;

        ResetState();
        SetGuardinMode("scatter");
        SetScore(0);
    }

    private void ResetState()
    {
        ResetGuardinMultiplier();

        if (guardins == null)
            return;

        for (int i = 0; i < this.guardins.Length; i++)
        {
            //-------
          //  this.guardins[i].ResetState();
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
        //----
      //   livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.Count = score;
        _result.ShowResult();
        // scoreText.text = score.ToString().PadLeft(2, '0');
    }
    private void SetShilds()
    {

        SaveData.Save(SaveData.Shield, MacShild);
    }
    public bool HeroCaught()
    {
        if (hero.IsUseShield())
            return false;

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
        return true;
    }

    public void GuardinCaught(Guardin guardin)
    {
        int points = guardin.points * this.guardinMultiplier;
        //SetScore(this.Count + points);
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
    //------
   
    private bool TrySpendBoost(string key)
    {
        if (SaveData.Has(key))
        {


            int count = SaveData.GetInt(key);
           // CountShildText.text = SHILD_COUNT.ToString();
            return count > 0;
        }
        else
        {
            return false;
        }

      /*  if (SHILD_COUNT>0)// (SaveData.Has(key))
        {
           SaveData.GetInt(key);            
            SHILD_COUNT -= 1;
            int count = SHILD_COUNT;//SaveData.GetInt(key);
            CountShildText.text = SHILD_COUNT.ToString();
            return count > 0;
        }
        else
        {
            return false;
        }*/
    }
    //--------
    public void SetGuardinMode(string mode) {
        SaveData.Save(SaveData.GuardinMode, mode);
    }

    private IEnumerator WaitPlayShield(float duration)
    {
        
        _result.ShowResult();

        hero.PlayShieldAnimation(duration);

        IsPlayShield = true;
        foreach (var guardin in guardins)
            guardin.Collider.isTrigger = true;

        yield return new WaitForSeconds(duration);

        IsPlayShield = false;
         foreach (var guardin in guardins)
            guardin.Collider.isTrigger = false;
    }
}
