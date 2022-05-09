using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Guardin[] guardins;
    public Hero hero;
    public Transform baxes;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;

    public int guardinMultiplier { get; private set; } = 1;

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKey) {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound() {
      // gameOverText.enabled = false;
      foreach(Transform bax in this.baxes) {
        bax.gameObject.SetActive(true);
      }
      ResetState();
    }

    private void ResetState() {
      ResetGuardinMultiplier();
      for(int i = 0; i < this.guardins.Length; i++) {
        this.guardins[i].ResetState();
      }

      this.hero.ResetState();
    }

    private void GameOver() {
      gameOverText.enabled = true;
      for(int i = 0; i < this.guardins.Length; i++) {
        this.guardins[i].gameObject.SetActive(false);
      }

      this.hero.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        // livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score) {
      this.score = score;
      // scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void HeroCaught() {
      this.hero.DeathSequence();

      SetLives(this.lives - 1);

      if(this.lives > 0) {
        Invoke(nameof(ResetState), 3.0f);
      } else {
        GameOver();
      }
    }

    public void GuardinCaught(Guardin guardin){
      int points = guardin.points * this.guardinMultiplier;
      SetScore(this.score + points);
      this.guardinMultiplier++;
    }

    public void BaxCollected(Bax bax) {
      bax.gameObject.SetActive(false);
      SetScore(this.score + bax.points);

      if(!HasRemainingBax()) {
        this.hero.gameObject.SetActive(false);
        Invoke(nameof(NewRound), 3.0f);
      }
    }

    public void BaxMajorCollected(BaxMajor baxMajor) {
      BaxCollected(baxMajor);
      CancelInvoke();
      Invoke(nameof(ResetGuardinMultiplier), baxMajor.duration);
    }

    private bool HasRemainingBax() {
      foreach(Transform bax in this.baxes) {
        if(bax.gameObject.activeSelf) {
          return true;
        }
      }

      return false;
    }

    private void ResetGuardinMultiplier() {
      this.guardinMultiplier = 1;
    }
}
