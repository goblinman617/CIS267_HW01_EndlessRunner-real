using System;
using System.Collections;
using UnityEngine;

public class AxeScript : MonoBehaviour{
    public float length;
    private float rotationVel;
    private float rotationZ;
    //public float rotationMinSpeed;

    public float maxRotation; //75
    private bool rotateClockwise;
    // Start is called before the first frame update
    void Start(){
        rotationZ = maxRotation;
    }

    // Update is called once per frame
    void Update(){
        setRotationSpeed();
        swing();
    }

    private void swing() {
        transform.eulerAngles = new Vector3(0, 0, rotationZ);
    }
    private void setRotationSpeed() {
        rotationVel += -(9.81f / length) * Mathf.Sin(transform.eulerAngles.z * (3.14f / 180f)) * Time.deltaTime;
        Debug.Log("rotation velocity change" + rotationVel + "/n rotationZ" + rotationZ);
        rotationZ += rotationVel;
    }
}
