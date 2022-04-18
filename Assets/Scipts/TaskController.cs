using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class TaskController : MonoBehaviour
{
    private const string BlankText = "";
    private const string AButtonPressed = "A Pressed";
    private const string BButtonPressed = "B Pressed";
    private const string XButtonPressed = "X Pressed";
    private const string YButtonPressed = "Y Pressed";
    private const string AuditoryButtonResponse = "Button Response to Auditory Distraction";
    private const string VisualButtonResponse = "Button Response to Visual Distraction";
    private const string PlayerTag = "MainCamera";
    private const float PositionTransformation = 50f;
    
    private float responseTime;
    private float globalActivityTimer;

    private string fileExportName;
    private string fileExportLocation;

    private StreamWriter fileStreamWriter;
    private StreamReader fileStreamReader;
    private List<Planes> horizontalPlaneList;

    private GameObject[] textDistractions;
    private GameObject[] lanes;
    private GameObject mainPlayer;
    private GameObject clockUiObject;
    private GameObject instructionObject;

    // Start is called before the first frame update
    public void Start()
    {
        SetupPlayerInformation();
        SetupInstructionObject();
        SetupTimerInformation();
        SetupFileRelease();
        SetupTextDistractions();
        SetupLaneInformation();
    }

    // Update is called once per frame
    public void Update()
    {
        UpdateTimer();
        UpdateInstructions();
        UpdatePlanePositionFromPlayer();
        TextDistractionPlayerResponse();
    }

    private void EnableAudioDistraction()
    {
        var randomAnimalDesignation = UnityEngine.Random.Range(0,4);
        responseTime = 0.0f;
        textDistractions[randomAnimalDesignation].SetActive(true);
    }

    private void EnableVisualBalloonDistraction(Planes horizontalPlane)
    {
        horizontalPlane.Balloon.SetActive(true);
        horizontalPlane.BalloonIsShowing = true;
        responseTime = 0.0f;
    }

    private void SetupPlayerInformation()
    {
        mainPlayer = GameObject.FindGameObjectWithTag(PlayerTag);
    }    
    
    private void SetupInstructionObject()
    {
        instructionObject = GameObject.FindGameObjectWithTag("Instructions");
        instructionObject.SetActive(false);
    }

    private void SetupTimerInformation()
    {
        globalActivityTimer = 0.0f;
        responseTime = 0.0f;
        clockUiObject = GameObject.FindWithTag("Clock");
    }

    private void SetupFileRelease()
    {
        fileExportName = "/ObservationLog_" + System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss") + ".csv";
        fileExportLocation = Application.persistentDataPath + fileExportName;
        fileStreamWriter = new StreamWriter(fileExportLocation);
        var categoryLine = String.Format("{0},{1},{2}","Global Time: ","Reaction Time: ","Activity: ");
        Debug.Log("File written to :" + fileExportLocation);
        using (fileStreamWriter){
            fileStreamWriter.WriteLine(categoryLine);
        }
    }

    private void SetupTextDistractions()
    {
        responseTime = 0.0f;

        textDistractions = GameObject.FindGameObjectsWithTag("AudioDistraction");
        foreach (GameObject textDistraction in textDistractions){
            textDistraction.GetComponent<Text>().text = BlankText;
            textDistraction.SetActive(false);
        }
        textDistractions[0].GetComponent<Text>().text = "CarHonk (A)";
        textDistractions[1].GetComponent<Text>().text = "Dog (B)";
        textDistractions[2].GetComponent<Text>().text = "Cat (X)";
        textDistractions[3].GetComponent<Text>().text = "Chicken (Y)";

        TaskWriter("AudioDistraction Started", responseTime.ToString(), globalActivityTimer.ToString());
    }

    private void SetupLaneInformation(){
        horizontalPlaneList = new List<Planes>();
        lanes = GameObject.FindGameObjectsWithTag("Planes");
        foreach (GameObject gameObject in lanes){
            var newPlane = new Planes(gameObject);
            horizontalPlaneList.Add(newPlane);
        }
    }

    private void TaskWriter(string activity, string timeDuration, string timeGlobal){
        var exportLine = timeGlobal;
        using (StreamWriter writer  = new StreamWriter(fileExportLocation, true)){
            exportLine += "," + timeDuration;
            exportLine += "," + activity;
            writer.WriteLine(exportLine);
            writer.Close();
        }
    }

    private void UpdateTimer()
    {
        clockUiObject.GetComponent<Text>().text = globalActivityTimer.ToString("0.00") + " seconds";

        if (MainController.IsObjectAccelerating){
            globalActivityTimer += Time.deltaTime;
            responseTime += Time.deltaTime;
        }
    }

    private void UpdateInstructions(){
        if (Input.GetKey(KeyCode.I)){
            if (instructionObject.activeSelf)
            {
                instructionObject.SetActive(false);
                Thread.Sleep(100);
            }
            else 
            {
                instructionObject.SetActive(true);
                Thread.Sleep(100);
            }
        }
    }

    private void UpdatePlanePositionFromPlayer(){
        foreach(Planes planeObject in horizontalPlaneList)
        {
            if (mainPlayer.transform.position.z > (planeObject.HorizontalPlane.transform.position.z + 2.5)) {
                planeObject.HorizontalPlane.transform.position = new Vector3(0,0,(planeObject.HorizontalPlane.transform.position.z + 50));
            }
        }
    }

    private void BalloonVisualDistractionResponse()
    {
        foreach (Planes p in horizontalPlaneList){
            if (p.BalloonIsShowing){
                if (Input.GetKey(KeyCode.Joystick1Button0) ||
                    Input.GetKey(KeyCode.Joystick1Button1) ||
                    Input.GetKey(KeyCode.Joystick1Button2) ||
                    Input.GetKey(KeyCode.Joystick1Button3)){
                        p.Balloon.SetActive(false);
                        p.BalloonIsShowing = false;
                        TaskWriter(VisualButtonResponse, responseTime.ToString(), globalActivityTimer.ToString());
                        Thread.Sleep(100);
                    }
            }
        }
    }

    private void TextDistractionPlayerResponse()
    {
        //Edit Task Handler
        //Response
        if (Input.GetKey(KeyCode.JoystickButton0) && textDistractions[0].activeSelf == true){
            Debug.Log(AButtonPressed);
            TaskWriter(AuditoryButtonResponse,responseTime.ToString(),globalActivityTimer.ToString());
            textDistractions[0].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton1) && textDistractions[1].activeSelf == true){
            Debug.Log(BButtonPressed);
            TaskWriter(AuditoryButtonResponse,responseTime.ToString(),globalActivityTimer.ToString());
            textDistractions[1].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton2) && textDistractions[2].activeSelf == true){
            Debug.Log(XButtonPressed);
            TaskWriter(AuditoryButtonResponse,responseTime.ToString(),globalActivityTimer.ToString());
            textDistractions[2].SetActive(false);
            Thread.Sleep(100);
        }
        if (Input.GetKey(KeyCode.JoystickButton3) && textDistractions[3].activeSelf == true){
            Debug.Log(YButtonPressed);
            TaskWriter(AuditoryButtonResponse,responseTime.ToString(),globalActivityTimer.ToString());
            textDistractions[3].SetActive(false);
            Thread.Sleep(100);
        }
    }
}
