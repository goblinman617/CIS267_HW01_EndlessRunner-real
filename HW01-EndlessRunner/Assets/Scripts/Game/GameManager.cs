using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Unity.Properties;

public static class GameManager {
    //I thought Time.deltaTime was a part of monobehavior for some reason
    //thats why it's passed a bunch but im not gonna change it
    private static float defaultSpeed = 3f; //3f
    private static float baseSpeed = 3f; 
    private static float maxBaseSpeed = 15f; //15f
    private static float maxSpeed = 20f; //20f
    private static float speed = 3f;
    private static int combo = 0;
    private static float score = 0;

    private static bool gameOver;
    //variables for math (K for constant)
    private const float comboK = .36f;
    private const float scoreK = .005f;
    private const float speedReduction = 1.75f; // * time deltaTime
    
    public static void resetAllValues() {
        baseSpeed = 3f;
        speed = 3f;
        combo = 0;
        score = 0;
    }
    public static void setGameOver(bool gameOverBool) {
        gameOver = gameOverBool;
        if (gameOver) {
            Debug.Log("saved: " + saveScore((int)score));
            Time.timeScale = 0f;
        } else {
            resetAllValues();
            Time.timeScale = 1f;
        }
    }
    public static void setSpeed(float deltaTime){
        speed = calcSpeed(deltaTime);
    }
    public static void addCombo() {
        combo++;
    }
    public static void resetCombo() {
        combo = 0;
    }
    public static int getCombo() { return combo;}
    public static float getScore() { return score; }
    public static bool getGameOver() { return gameOver; }
    public static float getSpeed() { return speed; }
    public static void addScore(float deltaTime) {
        score += (speed * deltaTime) + (combo * deltaTime * 10);// change the combo component when you make combo work.
    }

    //fix for delta time
    private static float calcSpeed(float deltaTime) {
        baseSpeed = defaultSpeed + (combo * comboK) + (score * scoreK);
        float reducedSpeed;
        if (speed > maxSpeed) {
            reducedSpeed = maxSpeed; //hard cap to max speed. sorry team
            return reducedSpeed;
        }else if (speed > baseSpeed) { //speed > baseSpeed
            reducedSpeed = speed - (3.5f * speedReduction * deltaTime);
            return reducedSpeed;
        }else if (baseSpeed > maxBaseSpeed) {
            return maxBaseSpeed;
        }
        return baseSpeed;
    }

    //fix both for delta time
    public static void slideStart(float deltaTime) {
        calcSpeed(deltaTime); //Get updated base Speed
        
        speed *= 1.35f;//deal with slow down later
    }
    public static void slideContinued(float deltaTime) {
        //
        if (speed > maxSpeed) {
            speed -= speedReduction * deltaTime * 10f;
        }else if (speed > baseSpeed + 5) {
            speed -= speedReduction * deltaTime * 2f;

        }else if (speed > baseSpeed/2){
            speed -= speedReduction * deltaTime;
        }
        //no else because we dont want keep slowing the player down
    }


    //Score methods
    public static bool saveScore(int score) {
        string path = Application.persistentDataPath + "/playerScore.sc";
        BinaryFormatter bf = new BinaryFormatter();
        const char delim = ',';

        if (!File.Exists(path)) {
            //create the file
            const string defaultScores = "0,0,0,0,0"; //5 default scores
            FileStream stream = new FileStream(path, FileMode.Create);
            
            bf.Serialize(stream, defaultScores); 

            stream.Close(); 
        }

        //check to see if score is top 5
        if (score > loadLowestScore()) {
            int[] scoresArray = loadAllScores(); //load all scores and store them in int array
            string newScoreString = "";

            for (int i = 0; i < 5; i++) {
                if (score > scoresArray[i]) { 
                    int tempScore = scoresArray[i];
                    scoresArray[i] = score;

                    //make old score at i "score" so that it will be checked against all the other scores and be moved accordingly
                    score = tempScore; 
                }
                //scoreArray[i] is correct so we can add it to the string
                if (i != 4) {
                    newScoreString += scoresArray[i].ToString() + delim.ToString();
                } else {
                    newScoreString += scoresArray[i].ToString();
                }
            }
            //newScoreString is now built

            FileStream stream = new FileStream (path, FileMode.Create);

            bf.Serialize(stream, newScoreString); //put new string into the file

            stream.Close();
            //did save score, return true
            return true;
        }
        // didnt save score, return false
        return false;
    }
    public static int loadLowestScore() {
        string path = Application.persistentDataPath + "/playerScore.sc";
        const char delim = ',';

        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            string scores = (string) bf.Deserialize(stream);

            stream.Close(); //close file

            //the list will be in order (hopefully)

            for (int i = 0; i < 4; i++) {
                int spot = scores.IndexOf(delim); //find spot of first ','
                scores = scores.Substring(spot+1); //get the substring of everything after the comma
            }
            //scores = last number now

            return Int32.Parse(scores); //return lowest score so we know we need to save.
        }
        else
        {
            Debug.Log("File not found in " + path);
            return -9; //return nothing
        }
    }

    public static int[] loadAllScores() {
        const char delim = ',';
        string path = Application.persistentDataPath + "/playerScore.sc";

        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            string scores = (string)bf.Deserialize(stream);

            int[] scoresArray = new int[5];

            for (int i = 0; i<5; i++) {
                int spot = scores.IndexOf(delim); //returns -1 when delim not found
                if (spot == -1) {
                    scoresArray[i] = Int32.Parse(scores);
                } else {
                    scoresArray[i] = Int32.Parse(scores.Substring(0, spot));
                    scores = scores.Substring(spot + 1); //set string to everything after the score we just added
                }
                
            }
            stream.Close();

            return scoresArray;
        } else {
            //no file exists;
            //attempt to save a score of -1
            //because file doesn't exist it makes the file first
            //because -1 is less than all the scores (0) it doesn't save the score
            saveScore(-1);
            //now we can return loadAllScores() because the file exists now
            //and it will enter the if statement (hopefully)
            return loadAllScores();
        }

    }
}
