using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public static string[] DistractionList = new string[81];
    public static string[] SingleTasks = new string[27];
    public static int TaskDeviation = 0;
    public static int TaskVisualDistraction = 0;
    public static int TaskAuditory  = 0;
    public static List<string> TaskList = new List<string>();
    private GameObject ListTextHolder;
    private GameObject[] TaskVisualDistractionTest;
    void Start()
    {
        ListTextHolder = GameObject.FindWithTag("ListText");
    }

    // Update is called once per frame
    void Update()
    {   
        //Updates the List
        ListDisplayUpdate();
        if (Input.GetKey(KeyCode.L)){
            ListTextHolder.GetComponent<Text>().text = "";
        }
        if (Input.GetKey(KeyCode.Delete)){
            TaskHandler.EventWriter("Button",Timer.TimerClock.ToString(),Timer.GlobalClock.ToString());
        }

    }

    public void ListDisplayUpdate(){
        ListTextHolder.GetComponent<Text>().text = ""; 
        foreach (string s in TaskList){
            ListTextHolder.GetComponent<Text>().text += "\n" + s;
        }
    }
}
