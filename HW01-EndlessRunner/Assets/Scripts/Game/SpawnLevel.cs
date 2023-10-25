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
    private int mapsBetweenCollectable;
    private int lastCollectable;
    private int randomIndex;
    private int coinSpot;

    // Start is called before the first frame update
    void Start(){
        spawnedObj = Instantiate(grounds[0].gameObject, this.transform);
        spawnedHazards = false;
        mapsBetweenCollectable = 0;
        lastCollectable = -1; //no collectable at index -1 so it won't try and reroll for first time
    }

    // Update is called once per frame
    void Update(){
        spawnGround();
        if (!spawnedHazards) {
            spawnHazards();
            //put this in here because it needs to be done everytime
            spawnScoreCollectable();
        }

        if (mapsBetweenCollectable > 4) {
            spawnCollectables();
        }
    }

    private void spawnGround() {
        if (spawnedObj.transform.position.x < 25.5f) {
            randomIndex = Random.Range(0, grounds.Length);
            spawnedObj = Instantiate(grounds[randomIndex].gameObject, this.transform);

            spawnedHazards = false;
            mapsBetweenCollectable++;
        }
    }

    private void spawnScoreCollectable() {
        coinSpot = Random.Range(0, 3);

        GroundMovement groundScript; 
        groundScript = spawnedObj.GetComponent<GroundMovement>();

        //3 is score collectable
        Instantiate(collectables[3].gameObject, groundScript.collectableLocations[coinSpot].gameObject.transform); //pray to god
    }

    private void spawnCollectables() {
        //for ground locations
        GroundMovement groundScript;
        groundScript = spawnedObj.GetComponent<GroundMovement>();

        int rand_i;
        int rand_spawn_i;

        do {
            rand_i = Random.Range(0, 3);
        } while (rand_i == lastCollectable);
        lastCollectable = rand_i;

        //no spawning coin and power up on same spot
        do {
            rand_spawn_i = Random.Range(0, 3);
        } while (rand_spawn_i == coinSpot);

        Instantiate(collectables[rand_i].gameObject, groundScript.collectableLocations[rand_spawn_i].gameObject.transform);

        //reset map count
        mapsBetweenCollectable = 0;
    }

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
        switch (rand_selection){ // rand_selection
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
