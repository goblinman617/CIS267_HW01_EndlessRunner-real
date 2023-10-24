using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HudScript : MonoBehaviour{
    public TMP_Text score;
    public TMP_Text combo;
    public TMP_Text speed;
    public TMP_Text score_endScreen;
    public TMP_Text popUp;
    public GameObject gameOverScreen;
    public GameObject CSS_menu;
    private double curSpeed;
    private bool changedScene;
    private const string txtScore = "Score: ";
    private const string txtCombo = "Combo: ";



    // Update is called once per frame
    void Update(){
        score.text = txtScore + (int)GameManager.getScore();
        combo.text = txtCombo + GameManager.getCombo();
        changeSpeedColor(); //sets curSpeed
        speed.text = "" + curSpeed;

        if (GameManager.getGameOver() && !changedScene) {
            enableGameOverScreen();
        }

    }
    private void Start() {
        changedScene = false;
        //i dont think we need this but here it is
/*        gameOverScreen.SetActive(false);
        CSS_menu.SetActive(true);*/
    }

    private void changeSpeedColor() {
        curSpeed = Math.Round(GameManager.getSpeed(), 2);

        if (curSpeed >= 15) {
            speed.color = new Color32(255, 75, 50, 255);
        } else if (curSpeed >= 10) {
            speed.color = new Color32(255, 183, 51, 255);
        } else if (curSpeed >= 5) {
            speed.color = new Color32(255, 243, 0, 255);
        } else {
            speed.color = new Color32(255, 255, 255, 255);
        }
    }
    private void enableGameOverScreen() {
        changedScene = true;
        gameOverScreen.SetActive(true);
        CSS_menu.SetActive(false);
        score_endScreen.text = "Final Score: " + (int) GameManager.getScore();
    }

    //button functions
    public void exitGame() {
        Application.Quit();
    }
    public void restartGame() {
        GameManager.setGameOver(false); //changes timescale back
        SceneManager.LoadScene("Game"); //loads gamescene
    }
    public void statsPage() {
        SceneManager.LoadScene("HighScores");
    }
}