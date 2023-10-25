using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour{
    public float offset;
    public float speed;
    private float distanceTraveled;
    private bool goLeft;

    private bool comboBroken;

    //spider will just walk back and forth i guess

    // Start is called before the first frame update
    void Start(){
        comboBroken = false;
        if (Random.Range(0, 2) == 1) {
            goLeft = true;
        } else {
            goLeft= false;
        }
        distanceTraveled = 0;
    }

    // Update is called once per frame
    void Update(){
        walkCycle();
        comboBreak();
    }

    private void walkCycle() {
        if (goLeft && distanceTraveled > -offset) {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            distanceTraveled -= (speed * Time.deltaTime); 
        }else if (!goLeft && distanceTraveled < offset) {

            transform.Translate(Vector2.right * speed * Time.deltaTime);
            distanceTraveled += (speed * Time.deltaTime);
        } else {
            goLeft = !goLeft;
        }
    }

    private void comboBreak() {
        if (transform.position.x < -12 && !comboBroken) {
            GameManager.resetCombo();
        }
    }
}
