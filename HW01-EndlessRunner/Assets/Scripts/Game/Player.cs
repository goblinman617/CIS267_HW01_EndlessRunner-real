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
    }

    // Update is called once per frame
    void Update(){
        jump();
        slide();
        GameManager.addScore(Time.deltaTime);
    }

    private void jump() {
        if (Input.GetKeyDown(KeyCode.Space) && curJumps < maxJumps) {//inital jump
            curJumps++;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            grounded = false;

            //rotation and animation change
            //DIRTY SOLUTION
            //transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


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
                GameManager.slideStart(Time.deltaTime);

                
                animator.SetBool("PlayerSliding", true);
                //DIRTY SOLUTION
                //transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                //transform.position = new Vector3 (transform.position.x, -6.3341f, transform.position.y);

            } else if (Input.GetKey(KeyCode.LeftShift)){
                GameManager.slideContinued(Time.deltaTime);
                //maybe sliding animation
            } else {
                GameManager.setSpeed(Time.deltaTime);//this will update speed every grounded frame 
                animator.SetBool("PlayerSliding", false);

                //DIRTY ANIMATION SOLUTION
                /*if (transform.localRotation != Quaternion.Euler(0f, 0f, 0f)) {
                    transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    transform.position = new Vector3(transform.position.x, -5.475614f, transform.position.y);
                    animator.SetBool("PlayerSliding", false);
                }*/
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            curJumps = 0;
            rb.gravityScale = defaultGravity;
            grounded = true;
            animator.SetBool("PlayerAirborn", false);
        }else if (collision.gameObject.CompareTag("Enemy")) {
            //we need an if statement here for the power ups
            if (true) {
                //change this shit back
                GameManager.setGameOver(true);
            } else {

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {

        }
    }
}
