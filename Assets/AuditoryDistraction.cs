using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class AuditoryDistraction : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject[] ADTextObjects;
    public static int Signal;
    private static int FlipFlop;
    private static int RandomAnimal;
    public static float clockResponse;

    void Start()
    {
        Signal = 0;
        FlipFlop = 0;
        RandomAnimal = 0;
        clockResponse = 0.0f;

        ADTextObjects = GameObject.FindGameObjectsWithTag("AudioDist");
        foreach (GameObject G in ADTextObjects){
            G.GetComponent<Text>().text ="";
            G.SetActive(false);
        }
        ADTextObjects[0].GetComponent<Text>().text = "Fish (A)";
        ADTextObjects[1].GetComponent<Text>().text = "Dog (B)";
        ADTextObjects[2].GetComponent<Text>().text = "Cat (X)";
        ADTextObjects[3].GetComponent<Text>().text = "Chicken (Y)";

    }

    // Update is called once per frame
    void Update()
    {
        clockResponse = Time.deltaTime;
        //Spawning
        if (Signal == 1){
            if (FlipFlop == 0){
                RandomAnimal = UnityEngine.Random.Range(0,4);
                clockResponse = 0.0f;
                // PlayerController.TaskList.Add("Visual Distraction - Sound");
                FlipFlop = 1;
            }
            for (int i = 0; i < ADTextObjects.Length; i++){
                if (RandomAnimal == i){
                    ADTextObjects[i].SetActive(true);
                }
            }
            Signal = 0;
        }
        else if (Signal == 0){
            FlipFlop = 0;
        }
        //Response
        if (Input.GetKey(KeyCode.JoystickButton0) && ADTextObjects[0].activeSelf == true){
            Debug.Log("A Pressed");
            TaskHandler.EventWriter("Button Response to Auditory Response",clockResponse.ToString(),Timer.GlobalClock.ToString());
            ADTextObjects[0].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton1) && ADTextObjects[1].activeSelf == true){
            Debug.Log("B Pressed");
            TaskHandler.EventWriter("Button Response to Auditory Response",clockResponse.ToString(),Timer.GlobalClock.ToString());
            ADTextObjects[1].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton2) && ADTextObjects[2].activeSelf == true){
            Debug.Log("X Pressed");
            TaskHandler.EventWriter("Button Response to Auditory Response",clockResponse.ToString(),Timer.GlobalClock.ToString());
            ADTextObjects[2].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton3) && ADTextObjects[3].activeSelf == true){
            Debug.Log("Y Pressed");
            TaskHandler.EventWriter("Button Response to Auditory Response",clockResponse.ToString(),Timer.GlobalClock.ToString());
            ADTextObjects[3].SetActive(false);
            Thread.Sleep(100);
        }
    }
}
 