using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class TaskDeviation : MonoBehaviour
{   

    public static int Signal = 0;
    private GameObject PlayerObject;
    public static int RandomDirection;
    private int FlipFlopOne = 0;
    private float DirectionSpeed = 0;
    private float x;
    

    void Start(){
        RandomDirection = UnityEngine.Random.Range(0,2);
        PlayerObject = GameObject.FindWithTag("MainCamera");
        
    }
    void Awake(){

    }
    void Update(){ 

        if (Signal == 1){
            if (FlipFlopOne == 0){
                RandomDirection = UnityEngine.Random.Range(0,2);
                // PlayerController.TaskList.Add("Visual Distraction - Lane");
                x = PlayerObject.transform.position.x;
                FlipFlopOne = 1;
            }
            if (RandomDirection == 0){
                DirectionSpeed = -0.8f;
                if (PlayerObject.transform.position.x > x){
                    Debug.Log("Stopping Deviation");
                    Signal = 0;
                }
            }
            else if (RandomDirection == 1){
                DirectionSpeed = 0.8f;
                if (PlayerObject.transform.position.x < x){
                    Debug.Log("Stopping Deviation");
                    Signal = 0;
                }
            }
            PlayerObject.transform.Translate(new Vector3(DirectionSpeed * Time.deltaTime,0,0));
        }
        else if (Signal == 0){
            FlipFlopOne = 0;
        }
    }
} 