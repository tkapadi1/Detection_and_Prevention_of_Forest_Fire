#pragma strict
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

/*
 * Boundary Detection algorithm and change color of sensors
 */
public class CollisionColorChange : MonoBehaviour
{
    public InstantiationExample algoObj;
    public Color Original;
    public Color Possible;
    public Color Triggered; //change this to desired color, when triggered
    public GameObject sensor_main;
    public GameObject waterpole;
    int counter = 0;
    public string count = "0";
    void OnTriggerEnter(Collider col) //detect collision with the fire
     {

        if (col.gameObject.tag == "Fire")
        {
            counter += 1;
            count = counter.ToString();
            
            Debug.Log("SENSOR COLLISION WITH Fire ");
            sensor_main.transform.GetComponent<Renderer>().material.color = Triggered; // when collided with fire, chnage the color to show triggered
            if (algoObj.algo == 1) // boundary detection, activate sprinklers
            {
                Instantiate(algoObj.myPrefab, sensor_main.transform.position, Quaternion.identity);
                
                Vector3 position_main = sensor_main.transform.position; //get the transform of current fire position
                int x = (int)position_main[0];
                int z = (int)position_main[1];
                int y = (int)position_main[2];
                Instantiate(algoObj.myPrefab, new Vector3(x + 10, z, y), Quaternion.identity); //triggere the surrounding sensors
                Instantiate(algoObj.myPrefab, new Vector3(x - 10, z, y), Quaternion.identity);
                Instantiate(algoObj.myPrefab, new Vector3(x, z, y+10), Quaternion.identity);
                Instantiate(algoObj.myPrefab, new Vector3(x, z, y-10), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x + 95, z, y), Quaternion.identity);//trigger water sprinklers to show the containment in surrpunding areas
                //Instantiate(waterpole, new Vector3(x - 95, z, y), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x, z, y - 95), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x, z, y + 95), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x + 95, z, y - 95), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x - 95, z, y - 95), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x + 95, z, y + 95), Quaternion.identity);
                //Instantiate(waterpole, new Vector3(x - 95, z, y + 95), Quaternion.identity);
            }
        }
    }
    //If no collision, no sensor is triggered, change color to original color of sensors
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            sensor_main.transform.GetComponent<Renderer>().material.color = Original;
        }
    }
}