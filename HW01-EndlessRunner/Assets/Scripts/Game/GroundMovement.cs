using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour{
    public GameObject[] spawnLocations; //0,1=ground     //2=air
    private float localSpeed;
    // Start is called before the first frame update
    void Start(){
        localSpeed = GameManager.getSpeed();
    }

    // Update is called once per frame
    void Update(){
        moveGround();
        checkToDestroy();
    }
    private void moveGround() {
        localSpeed = GameManager.getSpeed();
        transform.Translate(Vector2.left * Time.deltaTime * localSpeed);
    }
    private void checkToDestroy() {
        if (transform.position.x < -35f) {
            Destroy(gameObject);
        }
    }
}
