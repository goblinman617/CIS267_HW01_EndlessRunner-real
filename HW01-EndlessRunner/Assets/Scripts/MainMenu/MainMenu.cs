using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    // Start is called before the first frame update
    void Start(){

    }

    public void playButton() {
        SceneManager.LoadScene("Game");
    }
    public void exitButton() {
        Application.Quit();
    }
}
