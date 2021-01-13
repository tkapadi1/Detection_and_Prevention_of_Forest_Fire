using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To destroy the fire when a water particle collides with it to show the containment
 */
public class DestroyFire : MonoBehaviour
{
    private InstantiationExample algoObj;
    static private int counter = 0;
    void OnTriggerEnter(Collider col)
    {
        
            if (col.gameObject.tag == "Fire") //Find object by tag that is fire
            {

                Debug.Log("TRIGGER");
                if (counter % 10 == 0) //wait for 10 ms to destroy fire
                {
                    Destroy(col.gameObject); //destroy fire 
                    counter++;
                }
                else
                    counter++;

            }
        
    }
}
