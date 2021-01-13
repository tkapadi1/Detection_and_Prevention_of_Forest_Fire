using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Lane detection algorithm part 1
 */
public class LaneDetection : MonoBehaviour
{
    public bool Water = true;
    public InstantiationExample algoObj;
    public GameObject waterpole;
    public GameObject line;

    //if collision, with the sensors, detect the lane where the fire crosses and initiate the water sprinklers 
    void OnTriggerEnter(Collider col)
    {
        if (algoObj.algo == 2)
        {
            if (col.gameObject.tag == "Fire")
            {
                Vector3 linePos = col.transform.position;
                float linePosX = col.transform.position.x;
                float linePosY = col.transform.position.y;
                float linePosZ = col.transform.position.z;
                if (Water)
                {
                    Instantiate(waterpole, new Vector3(linePosX + 95, linePosY, linePosZ), Quaternion.identity);
                    Instantiate(waterpole, new Vector3(linePosX - 95, linePosY, linePosZ), Quaternion.identity);
                    Instantiate(waterpole, new Vector3(linePosX, linePosY, linePosZ + 95), Quaternion.identity);
                    Instantiate(waterpole, new Vector3(linePosX, linePosY, linePosZ - 95), Quaternion.identity);
                }
            }
        }
    }
}
