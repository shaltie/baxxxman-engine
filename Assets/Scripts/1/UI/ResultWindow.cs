using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultWindow : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private List<Text> _healthText;
    [SerializeField] private List<Text> _cristalText;
    [SerializeField] private List<Text> _levelText;
    [SerializeField] private Text _nextLevelText;
    [SerializeField] private Text _accelerateText;
    [SerializeField] private Text _biteText;
    [SerializeField] private Text _shieldText;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private int CurentLevel = 0;

    [SerializeField] private int Randombust_first = 0;
    [SerializeField] private int Randombust_second = 1;
    [SerializeField] private Button BustButFirst;
    [SerializeField] private Button BustButSecond;

    public void Awake()
    {
        
    }
    public void InputButBustFirst()
    {
        switch (Randombust_first)
        {
            case 0:
                BoostBite();
                break;
            case 1:
                BoostSpeedUp();
                break;
            case 2:
                BoostShield();
                break;
            case 3:
                BoostDash();
                break;           
        }
    }
    public void InputButBustSecond()
    {
        switch (Randombust_second)
        {
            case 0:
                BoostBite();
                break;
            case 1:
                BoostSpeedUp();
                break;
            case 2:
                BoostShield();
                break;
            case 3:
                BoostDash();
                break;
        }
    }
    public void BoostBite()
    {
        SaveBoost(SaveData.Bite);
    }

   /* public void BoostSpeedUP()
    {
        SaveBoost(SaveData.SpeedUp);
    }*/
    public void BoostShield()
    {
        SaveBoost(SaveData.Shield);
    }
    public void BoostDash()
    {
        SaveBoost(SaveData.Accelerate);
    }
    public void BoostSpeedUp()
    {
       // SaveBoost(SaveData.SpeedUp);
        SaveBoost(SaveData.KoefSpeedHero);
    }
    public void ShowResult()
    {
        foreach (var levelText in _levelText)
            levelText.text = "Level: "+ CurentLevel.ToString();//string.Format(levelText.text, SaveData.Level);
       // Debug.Log("SHOW LEVEL NUMBER =" + SaveData.GetInt(SaveData.Level).ToString());
       // foreach (var levelText in _levelText)
        //    levelText.text = string.Format(levelText.text, SaveData.GetInt(SaveData.Level));
        //     levelText.text = SceneManager.GetActiveScene().name;
        //     levelText.text = string.Format(levelText.text, SaveData.GetInt(SaveData.Level).ToString());

        foreach (var healthText in _healthText)
            healthText.text = "Health Count: " + _gameManager.lives;

        foreach (var cristalText in _cristalText)
            cristalText.text = $"Gems: {_gameManager.Count}/{_gameManager.MaxCount}";

        _nextLevelText.text = string.Format(_nextLevelText.text, SaveData.Level);
        _accelerateText.text = SaveData.GetInt(SaveData.Accelerate).ToString();
        _biteText.text = SaveData.GetInt(SaveData.Bite).ToString();
        _shieldText.text = SaveData.GetInt(SaveData.Shield).ToString();
        

    }
    public void GenericBonusBut()
    {
        //назначение кнопок буста
        Randombust_first = RandomBuster();
        Randombust_second = RandomBuster();
        if (Randombust_first > 3)
            Randombust_first = 1;

        if (Randombust_second == Randombust_first)
            Randombust_second += 1;
        if (Randombust_second > 3)
            Randombust_second = 0;

        if(Randombust_first == 1)
        {
            if(SaveData.GetInt(SaveData.KoefSpeedHero)>=3)
            {
                Randombust_first = 0;
            }
        }
        if (Randombust_second == 1)
        {
            if (SaveData.GetInt(SaveData.KoefSpeedHero) >= 3)
            {
                Randombust_second = 2;
            }
        }
        BustButFirst.GetComponentInChildren<Text>().text = InputButText(Randombust_first);
        BustButSecond.GetComponentInChildren<Text>().text = InputButText(Randombust_second);
    }
    private void SaveBoost(string key)
    {
        int count = 0;

        if (SaveData.Has(key))
            count = SaveData.GetInt(key);

        SaveData.Save(key, count + 1);
        _nextLevelButton.interactable = true;
        BustButFirst.interactable = false;
        BustButSecond.interactable = false;
        _nextLevelButton.GetComponentInChildren<Text>().text = "Run Next Level";// + SaveData.GetInt(SaveData.Level).ToString() ;// GetComponent<LevelManager>().Level.ToString();
    }

    private int RandomBuster()
    {
        int R = Random.Range(0, 4);
        Debug.Log("RANDOM R=" + R);
        return R;
    }
    private string InputButText(int R)
    {
        string s = "--";
        switch (R)
        {
            case 0:
                s = "+Bite";
                break;
            case 1:
                s = "+Speed Up";
                break;
            case 2:
                s = "+Shield";
                break;
            case 3:
                s = "+Dash";
                break;
            default:
                s = "+Shield_k";
                break;
        }
        return s;
    }
}
