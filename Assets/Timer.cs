using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class Timer : MonoBehaviour
{
    //Variables
    private int speedGo;
    private GameObject TextObject;
    private GameObject PlayerObject;
    private GameObject[] RespawnObject;
    private GameObject[] LaneOne;
    private GameObject[] LaneTwo;
    private GameObject[] LaneThree;
    private GameObject[] LaneFour;
    private static Timer Instance;
    public static float TimerClock;
    public static float GlobalClock;
    public static float clockResponse;
    public GameObject GlobalClockMonitor;
    private string LaneChanged;

    // Start is called before the first frame update
    void Start()
    {

        speedGo = 0;
        TimerClock = 0.0f;
        GlobalClock = 0.0f;
        clockResponse = 0.0f;
        //Assignment
        GlobalClockMonitor = GameObject.FindWithTag("GlobalClock");
        PlayerObject = GameObject.FindWithTag("Player");
        TextObject = GameObject.FindWithTag("UIText");
        RespawnObject = GameObject.FindGameObjectsWithTag("Respawn");
        LaneOne = GameObject.FindGameObjectsWithTag("Lane1");
        LaneTwo = GameObject.FindGameObjectsWithTag("Lane2");
        LaneThree = GameObject.FindGameObjectsWithTag("Lane3");
        LaneFour = GameObject.FindGameObjectsWithTag("Lane4");

    }
    void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Z)){
            LaneChanged = LaneAssignemt();
            Debug.Log(LaneChanged);
            Thread.Sleep(100);
        }
        if ((Input.GetKey(KeyCode.Space)))
        {
            switch (speedGo)
            {
                case 0:
                    speedGo = 1;
                    break;
                case 1:
                    speedGo = 0;
                    break;
            }
        }  
        switch (speedGo)
        {
            case 0:
                break;
            case 1:
                TimerClock += Time.deltaTime;
                // TextObject.GetComponent<Text>().text = "X: " + PlayerObject.transform.position.x;
                // TextObject.GetComponent<Text>().text += "\nY: " + PlayerObject.transform.position.y;
                // TextObject.GetComponent<Text>().text += "\nZ: " + PlayerObject.transform.position.z;
                TextObject.GetComponent<Text>().text = "\nLane: " + PLCQuick();
                TextObject.GetComponent<Text>().text += "\nTimeSpent in Lane: " + TimerClock.ToString("0.00");
                
                GlobalClock += Time.deltaTime;
                GlobalClockMonitor.GetComponent<Text>().text = GlobalClock.ToString("0.00") + " seconds";
                
                break;
        }
    }
    
    private void LaneChangeAssignment(){
        LaneChanged = LaneAssignemt();
    }
    
    //Returns Lane Assignment 
    private string LaneAssignemt(){
        string[] TagReturn = new string[4];
        string Holder = null;
        TagReturn[0] = PositionLaneCheck(LaneOne);
        TagReturn[1] = PositionLaneCheck(LaneTwo);
        TagReturn[2] = PositionLaneCheck(LaneThree);        
        TagReturn[3] = PositionLaneCheck(LaneFour);
        foreach (string s in TagReturn){
            Holder += s;
        }
        return Holder;
    }
    //Initial Start up 
    private string PLCQuick(){
        string[] TagReturn = new string[4];
        TagReturn[0] = PositionLaneCheck(LaneOne);
        TagReturn[1] = PositionLaneCheck(LaneTwo);
        TagReturn[2] = PositionLaneCheck(LaneThree);        
        TagReturn[3] = PositionLaneCheck(LaneFour);
        foreach (string s in TagReturn){
            if (s != null){
                //Checks when lane has been changed
                if (s != LaneChanged){
                    TimerClock = 0.0f;
                    LaneChangeAssignment();
                    Debug.Log("Lane has been changed");
                }
                return s;
            }
        }
        return null;
    }
    //Checks if Object is between Lanes
    private string PositionLaneCheck(GameObject[] Lane){
        foreach (GameObject G in Lane){
            if (PlayerObject.transform.position.x < G.transform.position.x)
            {
                if (PlayerObject.transform.position.x > G.transform.position.x - 3f){
                    return G.tag;
                }
            }
            if (PlayerObject.transform.position.x > G.transform.position.x){
                if (PlayerObject.transform.position.x < G.transform.position.x + 3f){
                    return G.tag;
                }
            }
        }
        return null;
    }


}
