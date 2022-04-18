using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryCameraFollow : MonoBehaviour
{    //Variables
    private int speedGo;
    private float speedEnhancer = 3;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
        {
            speedEnhancer = speedEnhancer + 0.1f;
        }
        if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
        {
            if (speedEnhancer > 3){
                speedEnhancer = speedEnhancer - 0.1f;
            }
        }
        if ((Input.GetKey(KeyCode.Space)))
        {
            switch (speedGo)
            {
                case 0:
                    speedGo = 1;
                    break;
                case 1:
                    speedEnhancer = 3;
                    speedGo = 0;
                    break;
            }
        }
        switch (speedGo)
        {
            case 0:
                break;
            case 1:
                transform.Translate(new Vector3(-speedEnhancer * Time.deltaTime,0,0));
                break;
        }
    }
}
