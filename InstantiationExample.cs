using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine.UI;
using System.IO;

public class InstantiationExample : MonoBehaviour
{
    private int nextActionTime = 24;
    public float period = 0.1f;
    public GameObject myPrefab;
    [Tooltip("Prediction Algorithm 1: All surrounding 8; 2:Lane detection Algorithm ")]
    public int algo;
    private GameObject[] sen;
    private Vector3 ini = new Vector3(455, 10, 455); //[Tooltip("Vector to store the position where the fire takes place ")]
    private int y ;
    private int x ;
    private int number;
    static private int counter = 0;
    IList<Posi> list = new List<Posi>();
    private string[,] map = new string[1000, 1000];
    int z = 0;
    private readonly ArrayList intno = new ArrayList();
    ArrayList num = new ArrayList();
    System.Random rnd = new System.Random();
    private bool read = true;
    int counter1 = 1;
    public string path = "C://Users//candi//Desktop//DHT sensor//for output//output.txt"; //Path where the file for sensors is generate
    void Start()
    {

        Debug.Log("Welcome to Detection and Prevention of Forest Fire Hud Space");
        num = ReadTextFile(path); //Read from file, which sensor is triggered.
        for (int i = 0; i < num.Count; i++)
        {
            Debug.Log("START: " + num[i]);
            int numb = (int)num[i]; //get that number
            Vector3 position = Selectsensor(numb);
            x = (int)position[0];
            y = (int)position[2]; //get the position of that number in x,y,z
            Debug.Log("Initiate fire");
            Instantiate(myPrefab, new Vector3(x, z, y), Quaternion.identity); //start a fire at that position
            Posi seedPosi = new Posi(x, y); //mark that position as seeen position
            list.Add(seedPosi); //add that to the list to already present fire positions
            map[x, y] = "F"; //mark the map as fire present 
        }
    }

    
    /*
     * Code to read from file if open and exists.
     * */
    ArrayList ReadTextFile(string file_path)
    {
        StreamReader inp_stm = new StreamReader(file_path);
        int i = 0;
        while (!inp_stm.EndOfStream) //till not end of file
        {
            string inp_ln = inp_stm.ReadLine();
            number = int.Parse(inp_ln);
            intno.Add(number);
            i++;
        }

        inp_stm.Close();
        return intno;
    }
    
    /*
     * Update the grid every 5ms
     */
    private void Update()
    {
        /*
        Debug.Log(Time.time);
        int timed = (int)Time.time;
        Debug.Log(timed);
        if (timed >= nextActionTime)
        {
            if (read)
            {
                num = ReadTextFile(path); //Read from file, which sensor is triggered.
                for (int i = 0; i < num.Count; i++)
                {
                    Debug.Log("START: " + num[i]);
                    int numb = (int)num[i]; //get that number
                    Vector3 position = Selectsensor(numb);
                    x = (int)position[0];
                    y = (int)position[2]; //get the position of that number in x,y,z
                    Debug.Log("Initiate fire");
                    Instantiate(myPrefab, new Vector3(x, z, y), Quaternion.identity); //start a fire at that position
                    Posi seedPosi = new Posi(x, y); //mark that position as seeen position
                    list.Add(seedPosi); //add that to the list to already present fire positions
                    map[x, y] = "F"; //mark the map as fire present 
                }
                read = false;
            }
            else
            {

                if (counter % 10 == 0)
                {
                    DrawFire();
                    counter++;
                }
                else
                    counter++;
            }
        }*/
        if (counter % 10 == 0)
        {
            DrawFire();
            counter++;
        }
        else
            counter++;

    }
        

    /*
     * show spread of fire based on random select algorithm.
     */
    private void DrawFire()
    {
        while (true)
        {
            int listIndex = rnd.Next(list.Count); //read random start position
            int option = rnd.Next(8) + 1; //choose a random option from 0 to 8
            Posi posi = list[listIndex];//get position of selected fire
            int i = posi.getI();
            int j = posi.getJ();
            switch (option)//start next fire based on the selected option
            {
                case 1:
                    i -= 10;
                    j += 10;
                    break;
                case 2:
                    i -= 10;
                    break;
                case 3:
                    i -= 10;
                    j -= 10;
                    break;
                case 4:
                    j -= 10;
                    break;
                case 5:
                    i += 10;
                    j -= 10;
                    break;
                case 6:
                    i += 10;
                    break;
                case 7:
                    i += 10;
                    j += 10;
                    break;
                case 8:
                    j += 10;
                    break;
            }
            Vector3 there = new Vector3(i, z, j);
            if (CheckIfPosEmpty(there)) //if fire not present
            {
                Instantiate(myPrefab, there, Quaternion.identity); //startfire at that position
                list.Add(new Posi(i, j)); //add new position to list
                map[i, j] = "F";
                break;
            }
        }
    }

    /*
     * Check if fire is present at selected position.
     */
    public bool CheckIfPosEmpty(Vector3 targetPos)
    {
        if (map[(int)targetPos[0], (int)targetPos[2]] == "F")
            return false;
        else
            return true;
    }

    /*
     * Select the sensor number and get its position.
     */
    public Vector3 Selectsensor(int number)
    {
        sen = GameObject.FindGameObjectsWithTag(number.ToString());
        return sen[0].transform.position;
    }

    /*
     * Class to create a list
     */
    class Posi
    {
        private int i, j;
        public Posi(int i, int j)
        {
            this.i = i;
            this.j = j;
        }
        public int getI()
        {
            return i;
        }
        public int getJ()
        {
            return j;
        }
    }

}
