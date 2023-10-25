using System;
using System.Collections;
using UnityEngine;

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

    //for power ups
    public GameObject shield;
    public GameObject[] boots;
    private bool shielded;
    private int spikeBoots; // will have 10 uses
    private float sComboTime;
    private bool doOnce;


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

        shielded = false;
        spikeBoots = 0; //max 10 
        doOnce = false;
    }

    // Update is called once per frame
    void Update(){
        jump();
        slide();
        GameManager.addScore(Time.deltaTime);

        CollisionDelay();

        superComboCountdown();
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
        } else if (Input.GetKey(KeyCode.LeftShift)) {
            //no change in velocity if they are holding shift
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

    private bool slidingWithBoots(Collision2D col) {
        if (Input.GetKey(KeyCode.LeftShift) && spikeBoots > 0) {
            spikeBoots--;

            GameManager.addCombo();
            Destroy(col.gameObject);

            if (spikeBoots == 0) {
                //remove boots
                for (int i = 0; i < 2; i++) {
                    boots[i].gameObject.SetActive(false);
                }
            }
            //return true to not lose game
            return true;
        } else {
            return false;
        }
    }



    //TRIGGER AND COLLISION
    //======================================================================================
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            //set speed on ground collision
            
            curJumps = 0;
            rb.gravityScale = defaultGravity;
            grounded = true;
            animator.SetBool("PlayerAirborn", false);
        }else if (collision.gameObject.CompareTag("Enemy") && !triggerCollisionHappened) {
            if (slidingWithBoots(collision)) {
                //boot logic handled in function
                //dont end the game
                //kill enemy
                //add combo
            } else if (shielded) {
                //all of shield's functionality

                //uses boots before shield
                shielded = false;
                shield.gameObject.SetActive(false);
                //no game over remove shield
            } else {
                GameManager.setGameOver(true);
            }
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
        } else if (collision.gameObject.CompareTag("Collectable")) {

            CollectableScript cScript = gameObject.GetComponent<CollectableScript>();
            int collectableType = cScript.getType();


            Destroy(collision.gameObject);
            
            switch (collectableType) {
                case 0:
                    collectBoots();
                    break;
                case 1:
                    collectShield();
                    break;
                case 2:
                    collectSuperCombo();
                    break;
                case 3:
                    collectScore();
                    break;
                default:
                    Debug.Log("Invalid Collectable Type");
                    break;
            }
            
        }
    }
    //============================================================================================



    //collectables
    //game object already destroyed
    private void collectBoots() {
        for(int i = 0; i < 2; i++) {
            boots[i].gameObject.SetActive(true);
        }
        spikeBoots = 10; //10 charges of spiked boots
    }
    private void collectShield() {
        shield.gameObject.SetActive(true);
        shielded = true;
    }
    private void collectSuperCombo() {
        GameManager.setSuperCombo(true);
        sComboTime = 15f;
    }
    private void collectScore() {

    }

    private void superComboCountdown() {
        if (sComboTime > 0) {
            doOnce = true;
            sComboTime -= Time.deltaTime;
        }else if (doOnce == true) {
            doOnce = false;
            GameManager.setSuperCombo(false);
        }
    }
}
