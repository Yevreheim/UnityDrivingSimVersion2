using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private const float MaximumAcceleration = 18f;
    private const float MinimumAcceleration = 0.5f;
    private const float AccelerationGain = 0.1f;
    private const float RightBarricade = 20f;
    private const float LeftBarricade = -3f;
    private const float ManualInputVelocity = 0.3f;

    private int currentSpeed;
    private float currentAcceleration;
    public static bool IsObjectAccelerating;
    
    // Start is called before the first frame update
    public void Start()
    {
        currentSpeed = 0;
        currentAcceleration = 3;
    }

    // Update is called once per frame
    public void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
        {
            if (transform.position.x <= RightBarricade){
                transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0,0));
                transform.Translate(new Vector3(ManualInputVelocity * Time.deltaTime, 0, 0));
            }
        }

        // if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A) || (Input.GetAxis("Horizontal")>0)))
        if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
        {
            //Make "Input.GetAxis("Horizontal")>0" as a case as above
            if (transform.position.x >= LeftBarricade){
                transform.Translate(new Vector3(Input.GetAxis("Horizontal"),0,0));
                transform.Translate(new Vector3(-ManualInputVelocity * Time.deltaTime, 0, 0));
            }
        }

        //Accelerating
        if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W))|| Input.GetAxis("Vertical")>0)
        {
            if (currentAcceleration <= MaximumAcceleration){
                currentAcceleration = currentAcceleration + AccelerationGain;
                if (currentAcceleration > MinimumAcceleration){
                    IsObjectAccelerating = true;
                }
            }
        }

        //Decceleratting
        if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)) || Input.GetAxis("Vertical")<0)
        {
            if (currentAcceleration >= 0){
                currentAcceleration = currentAcceleration - AccelerationGain;
            }
        }


        if((Input.GetKey(KeyCode.Space))){
            if (!IsObjectAccelerating){
                Debug.Log("Accelerating");
                Thread.Sleep(100);
                IsObjectAccelerating = true;
                currentAcceleration = 3f;
            }
            else{
                Debug.Log("Breaking");
                Thread.Sleep(100);
                IsObjectAccelerating = false;
                currentAcceleration = 0;
            }
        }

        CurrentPlayerMovement();
    }

    private void CurrentPlayerMovement(){
        if (IsObjectAccelerating){
            transform.Translate(new Vector3(0, 0, currentAcceleration * Time.deltaTime));
        }
        else if (currentAcceleration <= 0){
            IsObjectAccelerating = false;
        }
    }
}
