using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour{
    //fly back and forth over the area in the middle of the 2 spawns.
    public float horizontalOffset;
    public float verticalOffset;
    public float horizontalSpeed;
    public float verticalSpeed;
    private bool flyLeft;
    private bool flyUp;
    private Vector3 startingPos;
    private float distanceHorizontal;
    private float distanceVertical;
    
    // Start is called before the first frame update
    void Start(){
        distanceHorizontal = 0;
        distanceVertical = 0;

        if (Random.Range(0, 2) == 1) {
            flyLeft = true;
        } else {
            flyLeft = false;
        }

        if (Random.Range(0, 2) == 1) {
            flyUp = true;
        } else {
            flyUp = false;
        }

        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update(){
        flyHorizontal();
        flyVertical();
    }

    private void flyHorizontal() {
        //I'm gonna need a relative position
        if (flyLeft && distanceHorizontal > -horizontalOffset) { 
            distanceHorizontal -= horizontalSpeed *Time.deltaTime;
            transform.Translate(Vector2.left * Time.deltaTime * horizontalSpeed);

        } else if (!flyLeft && distanceHorizontal < horizontalOffset) {
            distanceHorizontal += horizontalSpeed * Time.deltaTime;
            transform.Translate(Vector2.right * Time.deltaTime * horizontalSpeed);

        } else {
            flyLeft = !flyLeft;
        }
    }
    private void flyVertical() {
        if (flyUp && distanceVertical < verticalOffset) { 
            distanceVertical += verticalSpeed *Time.deltaTime;
            transform.Translate(Vector2.up * Time.deltaTime * verticalSpeed);
        } else if (!flyUp && distanceVertical > -verticalOffset) { 
            distanceVertical -= verticalSpeed *Time.deltaTime;
            transform.Translate(Vector2.down * Time.deltaTime * verticalSpeed);
        } else {
            flyUp = !flyUp;
        }
    }
}
