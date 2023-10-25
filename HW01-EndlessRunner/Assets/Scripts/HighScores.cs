using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour{
    public TMP_Text scores;
    int[] scoresArray;
    string scores_string;

    // Start is called before the first frame update
    void Start(){
        scoresArray = GameManager.loadAllScores();
        for (int i = 0; i <  scoresArray.Length; i++) {
            scores_string += scoresArray[i] + ",";
        }
        scores.text = scores_string;
    }

}
