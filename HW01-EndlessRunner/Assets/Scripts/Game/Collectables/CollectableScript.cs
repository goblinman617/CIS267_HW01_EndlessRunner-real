using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour{
    public int whatCollectable; // same as array -- 0_boot, 1_shield, 2_superCombo, 3_score
    public int getType() {
        return whatCollectable;
    }
    //thats it


}
