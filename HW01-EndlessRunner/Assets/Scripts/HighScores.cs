using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour{
    public TMP_Text scores;
    int[] scoresArray;

    // Start is called before the first frame update
    void Start(){
        scoresArray = GameManager.loadAllScores();

        string txt = "1) " + scoresArray[0] + "\n2) " + scoresArray[1] + "\n3) " + scoresArray[2] + "\n4) " + scoresArray[3] + "\n5) " + scoresArray[4];
        scores.text = txt;
    }

    public void newGame() {
        SceneManager.LoadScene("Game");
    }

    public void mainMenu() {
        SceneManager.LoadScene("HomeScreen");
    }

    public void closeGame() {
        Application.Quit();
    }
}
