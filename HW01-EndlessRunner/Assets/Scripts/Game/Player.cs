using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour{
    private Rigidbody2D rb;
    private int maxJumps;
    private int curJumps;
    public int jumpForce;
    public float spaceHeldGravityScale;
    public float defaultGravity;
    public float quickFallSpeed;
    private bool grounded;
    private Animator animator;

    public float slideWaitTime;
    private float slideWaitDefaultTime;

    private bool triggerCollisionHappened;
    private int frameCount;


    // Start is called before the first frame update
    void Start(){
        //to set game over to false
        //actually we do this in the hudScript on restart
        //GameManager.setGameOver(false);
        rb = GetComponent<Rigidbody2D>();
        maxJumps = 1;
        curJumps = 0;
        rb.gravityScale = defaultGravity;
        grounded = false;
        animator = GetComponent<Animator>();
        slideWaitDefaultTime = slideWaitTime;
        triggerCollisionHappened = false;
        frameCount = 0;
    }

    // Update is called once per frame
    void Update(){
        jump();
        slide();
        GameManager.addScore(Time.deltaTime);

        CollisionDelay();
    }

    private void CollisionDelay() { //should give like 
        if (triggerCollisionHappened) {
            frameCount++;
            if (frameCount > 1) {
                frameCount = 0;
                triggerCollisionHappened = false;
            }
        }
    }

    private void bounce() {
        //set velocity to 0
        rb.velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.Space)) {
            //apply jump velocity
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            //hold space gravity shenanagins
            if (rb.velocity.y > 0 && rb.velocity.y < 5) { //y velocity between 0-5
                if (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)) {
                    rb.gravityScale = spaceHeldGravityScale;
                }
            } else if (rb.gravityScale != defaultGravity) {
                rb.gravityScale = defaultGravity;
            }

            //not holding space do a small bounce
        } else {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * .333f);
        }


    }
    private void jump() {
        if (Input.GetKeyDown(KeyCode.Space) && curJumps < maxJumps) {//inital jump
            curJumps++;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            grounded = false;

            animator.SetBool("PlayerAirborn", true);
            animator.SetBool("PlayerSliding", false);
        }

        //gravity hold space shenanigans
        if (rb.velocity.y > 0 && rb.velocity.y < 5) { //y velocity between 0-5
            if (Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)) {
                rb.gravityScale = spaceHeldGravityScale;
            }
        }else if (rb.gravityScale != defaultGravity) {
            rb.gravityScale = defaultGravity;
        }
    }
    private void slide() {
        //  stops slide spam
        if (slideWaitTime > 0) { 
            slideWaitTime -= Time.deltaTime;
        }

        if (!grounded){
            //quick fall
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                //probably add an animation for the quick fall
                rb.velocity = new Vector2(rb.velocity.x, quickFallSpeed);
                rb.gravityScale = defaultGravity;
            }
        }

        //ground slide
        if (grounded) {

            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                if (slideWaitTime <= 0) {

                    slideWaitTime = slideWaitDefaultTime;
                    GameManager.slideStart(Time.deltaTime);
                }
                animator.SetBool("PlayerSliding", true);
            } else if (Input.GetKey(KeyCode.LeftShift)){

                animator.SetBool("PlayerSliding", true);
                GameManager.slideContinued(Time.deltaTime);
            } else {

                GameManager.setSpeed(Time.deltaTime);//this will update speed every grounded frame 
                animator.SetBool("PlayerSliding", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            //set speed on ground collision

            curJumps = 0;
            rb.gravityScale = defaultGravity;
            grounded = true;
            animator.SetBool("PlayerAirborn", false);
        }else if (collision.gameObject.CompareTag("Enemy") && !triggerCollisionHappened) {

            Debug.Log("enemy collision: " + Time.frameCount);
            GameManager.setGameOver(true);
        }
    }
    //if trigger and collision happen on same frame.
    //both will trigger
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {

            triggerCollisionHappened = true;
            Debug.Log("trigger collsion: " + Time.frameCount);
            Destroy(collision.gameObject);
            GameManager.addCombo();
            bounce();

        }
    }
}
