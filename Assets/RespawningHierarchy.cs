using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class RespawningHierarchy : MonoBehaviour
{
    //Variable Declaration
    public static GameObject CameraObject;
    public static GameObject[] PlaneArray;
    private static GameObject[] HolderArray;
    public static GameObject[] CarArrayListing;
    private static GameObject[] holderCarArray;
    public static GameObject[] Capsule;
    private static GameObject[] arrayCapsule;
    public static int RespawningCarDeviation;
    public static int CarMovement;
    public static int CarMovementPlaneReference;
    public static int GlobalPlaneReference;
    public static int buttonVisualDisplay;
    private int valueStorage;


    void Start(){
        //Assignment
        CameraObject = GameObject.FindWithTag("MainCamera");
        PlaneArray = GameObject.FindGameObjectsWithTag("Respawn");
        Capsule = GameObject.FindGameObjectsWithTag("VisualDistraction");
        foreach (GameObject Z in Capsule){
            Z.SetActive(false);
        }
        //This is just a fix
        HolderArray = new GameObject[5];
        HolderArray[0] = PlaneArray[3];
        HolderArray[1] = PlaneArray[2];
        HolderArray[2] = PlaneArray[1];
        HolderArray[3] = PlaneArray[0];
        HolderArray[4] = PlaneArray[4];
        PlaneArray = HolderArray;
        //Capsules
        arrayCapsule = new GameObject[5];
        arrayCapsule[0] = Capsule[3];
        arrayCapsule[1] = Capsule[2];
        arrayCapsule[2] = Capsule[1];
        arrayCapsule[3] = Capsule[0];
        arrayCapsule[4] = Capsule[4];
        Capsule = arrayCapsule;
        foreach(GameObject G in PlaneArray){
            //Reordering CAUSE WHY, 43215
            Debug.Log(G.name);
        }

        CarArrayListing = GameObject.FindGameObjectsWithTag("Car");
        RespawningCarDeviation = 0;
        CarMovement = 1;


        //MORE DEBUG AND CAR FIXING
        holderCarArray = new GameObject[20];
        for (int i = 0; i < CarArrayListing.Length; i++){
            //Plane 4(1) old to Plane 4 new
            if (i == 0 || i == 1 || i == 2 || i ==3 ){

                holderCarArray[12 + i] = CarArrayListing[i];
            }
            //Plane 3(2) old to Plane 3 new
            else if (i == 4 || i == 5 || i == 6 || i == 7 ){
                holderCarArray[4 + i] = CarArrayListing[i];
            }
            //Plane 2(3) old to Plane 2 new
            else if (i == 8 || i == 9 || i == 10 || i == 11 ){
                holderCarArray[i-4] = CarArrayListing[i];
            }
            //Plane 1(4) to Plane 1 new
            else if (i == 12 || i == 13 || i == 14 || i == 15){
                //holderCarArray[12-i] = CarArrayListing[i];
                if (i == 12){
                    holderCarArray[0] = CarArrayListing[i];
                }
                else if (i == 13){
                    holderCarArray[1] = CarArrayListing[i];
                }
                else if (i == 14){
                    holderCarArray[2] = CarArrayListing[i];
                }
                else if (i == 15){
                    holderCarArray[3] = CarArrayListing[i];
                }
            }
            //Plane 5 to Plane 5
            else if (i > 15){
                holderCarArray[i] = CarArrayListing[i];
            }
        }
        CarArrayListing = holderCarArray;
        //CarDisappearance
        foreach (GameObject G in CarArrayListing){
            G.SetActive(false);
        }
        // for(int j = 0; j < CarArrayListing.Length;j++){
        //     if (j == 0 || j == 3){
        //         CarArrayListing[j].SetActive(false);
        //     }
        // }
        

    }
    
    void Update(){
        //Global Variable Updates
        float LaneTimer = Timer.TimerClock;
        //Respawning Planes
        for (int G = 0; G < PlaneArray.Length; G ++){
            //Activated when the Car travels halfway verticaly across a plane
            if (CameraObject.transform.position.z > PlaneArray[G].transform.position.z + 6){
                //Respawns path
                PlaneArray[G].transform.position = new Vector3(PlaneArray[G].transform.position.x,0,(PlaneArray[G].transform.position.z + 50));
                CarActivator(G,false);
                // foreach (GameObject C in Capsule){
                //     C.SetActive(false);
                // }
                //Control 
                if (LaneTimer > 8){
                    Debug.Log("G:" + G);
                    valueStorage = G;
                    RespawnInitiator(G);
                }
                else {
                    for (int i = 0; i < CarArrayListing.Length;i++){
                        if (i < valueStorage*4 || i > valueStorage*4 + 3){
                            CarArrayListing[i].SetActive(false);
                        }
                    }
                }
            }
        }

        //Car Deviation
        if (RespawningCarDeviation == 1){
            int LaneCount = 1;
            for (int i = CarMovementPlaneReference*4;i < CarMovementPlaneReference*4 + 4; i++,LaneCount++){
                //Checks if Car Exists
                if (CarArrayListing[i].activeSelf == true && CameraMovement.speedEnhancer>0){
                    if (CarMovement == 0 && LaneCount == 1 && CarArrayListing[i].transform.position.x < 6){
                        //Moving Right
                        CarArrayListing[i].transform.Translate(new Vector3(0,0,-1.5f*Time.deltaTime));
                    }
                    else if (CarMovement == 2 && LaneCount == 3&& CarArrayListing[i].transform.position.x > 6){
                        //Moving Left
                        CarArrayListing[i].transform.Translate(new Vector3(0,0,1.5f*Time.deltaTime));
                    }
                
                }
                //2nd Deviation
                else if (CarArrayListing[i].activeSelf == true && CameraMovement.speedEnhancer<=0){
                    if (CarMovement == 0 && LaneCount == 1 && CarArrayListing[i].transform.position.x < 12){
                        //Moving Right
                        CarArrayListing[i].transform.Translate(new Vector3(0,0,-3f*Time.deltaTime));
                    }
                    else if (CarMovement == 2 && LaneCount == 3&& CarArrayListing[i].transform.position.x > 0){
                        //Moving Left
                        CarArrayListing[i].transform.Translate(new Vector3(0,0,3f*Time.deltaTime));
                    }
                    else{
                        Timer.TimerClock = 0;
                        RespawningCarDeviation = 0;
                    }
                }
                
            }
            
        }

        //Button Response for Visual Display
        if (buttonVisualDisplay == 1){
            if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Keypad0)){
                buttonVisualDisplay = 0;
                TaskHandler.EventWriter("Button Response to Visual",LaneTimer.ToString(),Timer.GlobalClock.ToString());
                Debug.Log("Button Response Accepted");
            }
        }


        //Debug
        if (Input.GetKey(KeyCode.M)){
            foreach (GameObject G in PlaneArray){
                Debug.Log("Plane Position: " + G.transform.position.z);
            }
            Debug.Log("Car Array Length: " + CarArrayListing.Length);
            Thread.Sleep(100);
        }

    }

    private void CarActivator(int PlaneNumber, bool State){
        int LaneCount = 1;
        string Lane;
        for(int i = PlaneNumber*4; i < PlaneNumber*4 + 4; i++,LaneCount++){
            CarArrayListing[i].SetActive(State);
            Lane = "Lane" + LaneCount.ToString();
            Vector3 Position = new Vector3(GameObject.FindWithTag(Lane).transform.position.x,0,PlaneArray[PlaneNumber].transform.position.z);
            CarArrayListing[i].transform.position = Position;
        }
        Capsule[PlaneNumber].SetActive(false);
    }
    public static void SelectiveCarActivator(int[] Array, int PlaneNumber){
        int Count = 0;
        //Capsule[PlaneNumber].SetActive(true);
        for(int i = PlaneNumber*4;i< PlaneNumber*4 + 4;i++,Count++){
            if (Array[Count] == 1){
                CarArrayListing[i].SetActive(true);
            }
        }
        Capsule[PlaneNumber].SetActive(true);
    }
    private void RespawnInitiator(int PlaneNumber){
        //Timer Reset
        Timer.TimerClock = 0.0f;
        //TaskHandler
        TaskHandler.TaskProcessor(PlaneNumber);
        
    }
     public static int[] RandomAllocation(int Check)
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
                //Display Button Response
                buttonVisualDisplay = 1;
                //Filter
                if (count != 1 || PathRandom[3] != 1) {
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
                if (count != 1 || PathRandom[1] != 0 || PathRandom[Check] != 1) {
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
        return PathRandom;
    }
}