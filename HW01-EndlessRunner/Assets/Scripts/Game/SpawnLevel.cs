using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour{
    public GameObject[] grounds;
    public GameObject[] groundHazards;
    public GameObject[] airHazards;
    public GameObject[] collectables;

    private GameObject hazard;
    private GameObject spawnedObj;
    private bool spawnedHazards;
    private int randomIndex;

    // Start is called before the first frame update
    void Start(){
        spawnedObj = Instantiate(grounds[0].gameObject, this.transform);
        spawnedHazards = false;
    }

    // Update is called once per frame
    void Update(){
        spawnGround();
        if (!spawnedHazards) {
            spawnHazards();
        }
    }

    private void spawnGround() {
        if (spawnedObj.transform.position.x < 25.5f) {
            randomIndex = Random.Range(0, grounds.Length);
            spawnedObj = Instantiate(grounds[randomIndex].gameObject, this.transform);
            spawnedHazards = false;
        }
    }
    //hazards will be spike trap to jump over
    //goomba to stomp on
    //bird to slide under/stomp
    //swinging axe

    //Rocket will be a timed event handled seperently in its own script
    //rocket that flies in at player's y cord AND one that flies so they have to slide under/jump over
    
    private void spawnHazards() {
        /* 0 = ground spawn (goomba or spike)
         * 1 = air spawn (bird or axe)
         * 3 = 2 ground 
         * 4 = ground and air
         * 5 = nada
         * 6 = nada
         * 7 = nada
         */
        const int air = 2;
        GroundMovement objScript;
        objScript = spawnedObj.GetComponent<GroundMovement>();

        int rand_selection = Random.Range(0, 4); //(0,8) FOR NOW WE SET TO 4 so no empty maps
        int rand_hazard = Random.Range(0, 2); //spike, goomba or bird, axe
        int rand_spawn_pos = Random.Range(0, 2);
        switch (3){ // rand_selection
            case 0:
                //make GameObject of rand hazard at one of ground spawn locations
                hazard = Instantiate(groundHazards[rand_hazard].gameObject, objScript.spawnLocations[rand_spawn_pos].gameObject.transform);
                break;
            case 1:
                hazard = Instantiate(airHazards[rand_hazard].gameObject, objScript.spawnLocations[air].gameObject.transform);
                break;
            case 2:
                hazard = Instantiate(groundHazards[rand_hazard].gameObject, objScript.spawnLocations[rand_spawn_pos].gameObject.transform);
                //flip flop rand_hazard and rand_spawn_pos
                rand_hazard = flipNum(rand_hazard);
                rand_spawn_pos = flipNum(rand_spawn_pos);
                //make new hazard with new nums
                hazard = Instantiate(groundHazards[rand_hazard].gameObject, objScript.spawnLocations[rand_spawn_pos].gameObject.transform);
                break;
            case 3:
                hazard = Instantiate(groundHazards[rand_hazard].gameObject, objScript.spawnLocations[rand_spawn_pos].gameObject.transform);
                //reroll for the air hazard
                rand_hazard = Random.Range(0, 2);
                hazard = Instantiate(airHazards[rand_hazard].gameObject, objScript.spawnLocations[air].gameObject.transform);
                break;
            case 4:
                break;
            case 5:
                //no hazards
                break;
            case 6:
                //no hazards
                break;
            case 7:
                //no hazards
                break;
            default: 
                //for the lolz
                break;
        }
        spawnedHazards = true;
    }
    private int flipNum(int num) {
        if (num == 0) {
            return 1;
        } else {
            return 0;
        }
    }
}
