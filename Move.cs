using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script to move the camera as 1st person through the area
 */
public class Move : MonoBehaviour
{
    public float speed = 5.0f;

    // Update is called once per frame
    public void FixedUpdate()
    {
        //when a key is pressed, get its position and add to it the speed to move in a direction
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z - speed);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z + speed);
        }
    }
}
