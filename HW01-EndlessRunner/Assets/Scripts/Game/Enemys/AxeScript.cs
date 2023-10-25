using System;
using System.Collections;
using UnityEngine;

public class AxeScript : MonoBehaviour{
    public float length;
    private float rotationVel;
    private float rotationZ;
    //public float rotationMinSpeed;
    private bool gaveCombo;

    public float maxRotation; //75
    private bool rotateClockwise;
    // Start is called before the first frame update
    void Start(){
        gaveCombo = false;
        rotationZ = maxRotation;
    }

    // Update is called once per frame
    void Update(){
        setRotationSpeed();
        swing();

        if (!gaveCombo && transform.position.x < -11) {
            gaveCombo = true;
            GameManager.addCombo();
        }
    }

    private void swing() {
        transform.eulerAngles = new Vector3(0, 0, rotationZ);
    }
    private void setRotationSpeed() {
        rotationVel += -(9.81f / length) * Mathf.Sin(transform.eulerAngles.z * (3.14f / 180f)) * Time.deltaTime;
        rotationZ += rotationVel;
    }
}
