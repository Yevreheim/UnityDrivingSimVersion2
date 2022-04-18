using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;
using System;
using System.Threading;
using System.Linq;

public class Respawn : MonoBehaviour
{
    //Variables
    private static float x = 0;
    private static float y = 0;
    private static float z = 0;
    private static float initialX;
    private static float initialY;
    private static float initialZ;
    int[] Show = new int[2];
    public static GameObject[] CellArray;
    private List<GameObject> Car = new List<GameObject>();
    private System.Random Rand = new System.Random();
    int CountTwo = 0;
    private static GameObject[] CarArray = new GameObject[4];
    private GameObject TestObject;
    private static Respawn Instance;
    //LaneDeviationSpeed
    private float LaneDeviationSpeed;
    public static int RespawningCarDeviation;
    public static int LeftRightCarMove = 0;
    private static int TaskActiveHolder = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        CellArray = CellCount(CellArray);
        foreach (GameObject G in CellArray){
            if (G.tag == "Car"){
                CarArray[CountTwo] = G;
                G.SetActive(false);
                CountTwo++;
            }
        }
        //RandomAllocation();
        TestObject = GameObject.FindWithTag("MainCamera");
        RespawningCarDeviation = 0;
    }
    
    void Awake(){
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        float LaneTimer = Timer.TimerClock;
        x = GameObject.FindWithTag("MainCamera").transform.position.x;
        y = GameObject.FindWithTag("MainCamera").transform.position.y;
        z = GameObject.FindWithTag("MainCamera").transform.position.z;
         

        if (Input.GetKey(KeyCode.X)){
            Debug.Log(Timer.TimerClock.ToString());
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.H)){
            //CarDeviation();
            TaskActiveHolder = 0;
        }
        //Car Deviation Distraction
        if (RespawningCarDeviation == 1){
            if (CarArray[LeftRightCarMove].activeSelf == true){
                if (LeftRightCarMove == 0 && CarArray[LeftRightCarMove].transform.position.x < 6){
                    CarArray[LeftRightCarMove].transform.Translate(new Vector3(0,0,-2*Time.deltaTime));
                }
                else if (LeftRightCarMove == 2 && CarArray[LeftRightCarMove].transform.position.x > 6){
                    CarArray[LeftRightCarMove].transform.Translate(new Vector3(0,0,2*Time.deltaTime));
                }
                else {
                    RespawningCarDeviation = 0;
                }
            }
        }

        //Respawning Process
        if (z > transform.position.z + 5)
        {
            // Modify by 10 for each plane
            transform.position = new Vector3(0,0,(transform.position.z + 40));
            //Changing this timer variable can taken in via cases
            if (LaneTimer > 8){
                //List of tasks generated
                int RandomSelection = UnityEngine.Random.Range(0,1);
                TaskHandler.TaskProcessor(1);
                // TaskActiveHolder = 1;
                Timer.TimerClock = 0.0f;
                //TaskSelection(RandomSelection);
            }
            //Reset Section
            else if (TaskActiveHolder == 0){
                foreach(GameObject G in CarArray){
                    if (G.tag == "Car"){
                        G.SetActive(false);
                    }
                }
            }
            Debug.Log("New Position: " + transform.position.z);
        }
    }
    private void CarDeviation(){
        int NegPos = UnityEngine.Random.Range(0,2);
        if (NegPos == 0) {
            LaneDeviationSpeed = -2;
        }
        else if (NegPos == 1){
            LaneDeviationSpeed = 2;
        }
        if (true){
            TestObject.transform.Translate(new Vector3(LaneDeviationSpeed*Time.deltaTime,0,0));
        }
    }

    //Respawn Algorithm
    public static void RandomAllocation(int Check, float PlanePosition)
    {
        var randomInt = UnityEngine.Random.Range(0,2);
        int[] PathRandom = new int[]{0,0,0,0};
        int count = 0;
        //This is different from PathRandom, as it looks at the time + state
        int[] TaskCountHolder = new int[]{0,0,0,0};
        //Clock
        float HolderFloat = Timer.TimerClock;
        //Randomly Allocates all slots in the Variable to be 2, 1s and 0s
        //Pass through Int value
        while (true){
            foreach (int i in PathRandom){
                if (i == 1){
                    count++;
                }   
            }
            //Force
            //Ensures that only 2 objects can be spawned at a time
            // ! checks for True/False, 1 = exists, 2 = not exists.
            if (Check == 1){
                if (count != 2 || PathRandom[1] != 1) {
                count = 0;
                // Rand = new System.Random();
                    for (int j = 0; j < PathRandom.Length; j++){
                        if (randomInt == 0){
                            PathRandom[j] = 0;
                        }
                        else if (randomInt == 1){
                            PathRandom[j] = 1;
                        }
                        randomInt = UnityEngine.Random.Range(0,2);
                    }
                }
                else {
                    break;
                }
            }
            //Car Deviations
            else if (Check == 0 || Check == 2){
                if (count != 2 || PathRandom[1] != 0 || PathRandom[Check] != 1) {
                count = 0;
                // Rand = new System.Random();
                    for (int j = 0; j < PathRandom.Length; j++){
                        if (randomInt == 0){
                            PathRandom[j] = 0;
                        }
                        else if (randomInt == 1){
                            PathRandom[j] = 1;
                        }
                        randomInt = UnityEngine.Random.Range(0,2);
                    }
                }
                else {
                    break;
                }
            }
            
        }
        //Checks Time + State to activate or deactive Cars
        for (int i = 0; i < PathRandom.Length; i++)
        {
            //Could have the functiom below return a value when True is done
            //For loop to check an array
            if (PathRandom[i] == 1){
                CarArray[i].SetActive(true);
            }
            else if (PathRandom[i] == 0){
                CarArray[i].SetActive(false);
            }
            //CarArray[i].transform.position = new Vector3(i*6f,0,(PlanePosition));
        }
    }
    

    private GameObject[] CellCount(GameObject[] G){
        G = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            G[i] = transform.GetChild(i).gameObject;
        }
        return G;
    }
}
