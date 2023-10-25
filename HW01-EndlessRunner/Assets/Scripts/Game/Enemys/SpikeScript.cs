using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour{
    private bool gaveCombo;
    void Start(){
        gaveCombo = false;
    }

    // Update is called once per frame
    void Update(){
        if (!gaveCombo && transform.position.x < -11) {
            gaveCombo = true;
            GameManager.addCombo();
        }
    }

}
