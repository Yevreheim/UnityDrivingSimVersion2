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
    private const string MovingCarBreakResponse = "User has braked to MovingCarDistraction";
    private const string MovingCarCrashResponse = "User has crashed to MovingCarDistraction";
    private const string LaneDeviationResponse = "Control response to LaneDeviationDistraction";
    private const string AuditoryDistractionStartLabel = "Started AuditoryDistraction";
    private const string VisualDistractionStartLabel = "Started VisualDistractionDistraction";
    private const string LaneDeviationDistractionStartLabel = "Started LaneDeviationDistraction";
    private const string MovingCardDistractionStartLabel = "Started MovingCarDistraction";
    private const string PlayerTag = "MainCamera";
    private const string TimesCrashedLabel = "Times Crashed: ";

    //Control Time of Tasks
    private const float TimeBetweenTasks = 4; //4 seconds
    private const float TimeBetweenIndividualTaskOfDualTasks =  0.4f; //400ms
    private const float TimeBetweenSimultaneousTasks = 0; //0 Seconds

    private const float PositionTransformation = 50f;
    private const float TaskDeviationSpeed = 0.8f;
    private const float TaskCarMovementSpeed = 1.5f;
    private const float LaneThirdMiddlePositionX = 12f;
    private const float LaneFirstMiddlePositionX = 0f;
    private const float TimeToOpenEyes = 5f;
    
    private float responseTime;
    private float globalActivityTimer;
    private float defaultCurrentPlayerPositionX;

    private string fileExportName;
    private string fileExportLocation;

    private int randomDevitationDirection;
    private int timesUserHasCrashed;

    private bool enableTaskDeviationMovement;
    private bool enableTaskMovingCar;
    private bool enableVisualBalloonTask;
    private bool enableProgramToStart;
    private bool userHasCrashed;
    private bool userHasBraked;

    private StreamWriter fileStreamWriter;
    private StreamReader fileStreamReader;
    private List<Planes> horizontalPlaneList;

    private TaskCounter taskCounterController;

    private GameObject[] textDistractions;
    private GameObject[] lanes;
    private GameObject[] audioSources;
    private GameObject mainPlayer;
    private GameObject instructionCrashText;
    private GameObject movingCar;
    private GameObject clockUiObject;
    private GameObject instructionObject;
    private GameObject balloonVisualDistractionObject;


    // Start is called before the first frame update
    public void Start()
    {
        SetupTaskInformation();

        SetupPlayerInformation();
        SetupInstructionObject();
        SetupTimerInformation();
        SetupSoundObjects();
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
        UpdateTaskDeviationMovement();
        UpdateUICrashInformation();
        UpdateTaskMovingCar();
        TaskCarMovementPlayerResponse();
        TextDistractionPlayerResponse();
        BalloonVisualDistractionResponse();

        CommenceProgram();
    }

    private void CommenceProgram()
    {
        if (Input.GetKey(KeyCode.Space)){
            clockUiObject.SetActive(true);
            globalActivityTimer = 0.0f;
            responseTime = 0.0f;

            var closeEyes = GameObject.FindGameObjectWithTag("CloseEyes");
            var openEyes = GameObject.FindGameObjectWithTag("OpenEyes");
            closeEyes.GetComponent<AudioSource>().Play();
            TaskWriter("Closing Eyes", 0.ToString(),globalActivityTimer.ToString());
            openEyes.GetComponent<AudioSource>().PlayDelayed(TimeToOpenEyes);
            TaskWriter("Opening Eyes", 0.ToString(),(globalActivityTimer + TimeToOpenEyes).ToString());

            enableProgramToStart = true;
        }

        //13 seconds has passed
        if (enableProgramToStart){
            if (globalActivityTimer > 13)
            {
                TaskScheduler();
            }
        }
    }

    private void TaskScheduler()
    {
        var task = taskCounterController.AssignTask();
        if (task != null)
        {
            //do thing
        }
        else{
            Debug.Log("Program Finished");
            Application.Quit();
        }
    }

    private void SetupTaskInformation()
    {
        taskCounterController = new TaskCounter();
    }

    private void EnableMovingCarDistraction(Planes horizontalPlane)
    {
        var randomCarDesignation = UnityEngine.Random.Range(0,1);
        //Car 1/Left -> Right
        if (randomCarDesignation == 0)
        {   
            foreach (GameObject car in horizontalPlane.Cars){
                if (car.name == "Car1"){
                    car.SetActive(true);
                    movingCar = car;
                }
            }
        }
        //Car 3/Right -> Left
        else 
        {
            foreach (GameObject car in horizontalPlane.Cars){
                if (car.name == "Car3"){
                    car.SetActive(true);
                    movingCar = car;
                }
            }
        }   

        TaskWriter(MovingCardDistractionStartLabel, 0.ToString(), globalActivityTimer.ToString());
        enableTaskMovingCar = true;
        responseTime = 0.0f;
    }

    private void EnableAudioDistraction()
    {
        var randomAnimalDesignation = UnityEngine.Random.Range(2,3);
        textDistractions[randomAnimalDesignation].SetActive(true);
        if (audioSources != null){
            audioSources[randomAnimalDesignation].gameObject.GetComponent<AudioSource>().Play();
        }

        TaskWriter(AuditoryDistractionStartLabel, 0.ToString(), globalActivityTimer.ToString());
        responseTime = 0.0f;
    }

    private void EnableVisualBalloonDistraction(Planes horizontalPlane)
    {
        if (horizontalPlane != null){
            if (horizontalPlane.Balloon != null)
            {
                horizontalPlane.Balloon.SetActive(true);
                balloonVisualDistractionObject = horizontalPlane.Balloon;
                enableVisualBalloonTask = true;

                TaskWriter(VisualDistractionStartLabel, 0.ToString(), globalActivityTimer.ToString());
                responseTime = 0.0f;
            }
        }
    }

    private void EnableLaneDeviationDistraction()
    {
        var player = GameObject.FindGameObjectWithTag("MainCamera");
        randomDevitationDirection = UnityEngine.Random.Range(0,2);
        defaultCurrentPlayerPositionX = player.transform.position.x;
        enableTaskDeviationMovement = true;
        userHasBraked = false;

        TaskWriter(LaneDeviationDistractionStartLabel, 0.ToString(), globalActivityTimer.ToString());
        responseTime = 0.0f;
    }

    private void UpdateTaskDeviationMovement()
    {
        if (enableTaskDeviationMovement == true){
            var player = GameObject.FindGameObjectWithTag("MainCamera");
            float deviatonDirection;
            //Left
            if (randomDevitationDirection == 0)
            {
                deviatonDirection = -TaskDeviationSpeed;
                if (player.transform.position.x > defaultCurrentPlayerPositionX){
                    enableTaskDeviationMovement = false;
                    Debug.Log("Stopping Deviation");
                    TaskWriter(LaneDeviationResponse, responseTime.ToString(),globalActivityTimer.ToString());
                }
            }
            //Right
            else 
            {
                deviatonDirection = TaskDeviationSpeed;
                if (player.transform.position.x < defaultCurrentPlayerPositionX){
                    enableTaskDeviationMovement = false;
                    Debug.Log("Stopping Deviation");
                    TaskWriter(LaneDeviationResponse, responseTime.ToString(),globalActivityTimer.ToString());
                }
            }
            player.transform.Translate(new Vector3((deviatonDirection * Time.deltaTime),0,0));
        }
    }

    private void SetupPlayerInformation()
    {
        mainPlayer = GameObject.FindGameObjectWithTag(PlayerTag);
        instructionCrashText = GameObject.FindGameObjectWithTag("CrashedTextTag");
        instructionCrashText.SetActive(userHasCrashed);
    }    
    
    private void SetupInstructionObject()
    {
        instructionObject = GameObject.FindGameObjectWithTag("Instructions");
        instructionObject.SetActive(false);
    }

    private void SetupSoundObjects()
    {
        audioSources = GameObject.FindGameObjectsWithTag("SoundEffects");
    }   

    private void SetupTimerInformation()
    {
        globalActivityTimer = 0.0f;
        responseTime = 0.0f;
        clockUiObject = GameObject.FindWithTag("Clock");
        clockUiObject.SetActive(false);
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
        // textDistractions[0].GetComponent<Text>().text = "CarHonk (A)";
        // textDistractions[1].GetComponent<Text>().text = "Dog (B)";
        // textDistractions[2].GetComponent<Text>().text = "Cat (X)";
        // textDistractions[3].GetComponent<Text>().text = "Chicken (Y)";

        textDistractions[0].GetComponent<Text>().text = "";
        textDistractions[1].GetComponent<Text>().text = "";
        textDistractions[2].GetComponent<Text>().text = "";
        textDistractions[3].GetComponent<Text>().text = "";
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
        globalActivityTimer += Time.deltaTime;
        responseTime += Time.deltaTime;
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
                planeObject.ResetCarPosition();
                //Could Insert task here
            }
        }
    }

    private void BalloonVisualDistractionResponse()
    {
        if (enableVisualBalloonTask)
        {
            if (balloonVisualDistractionObject != null)
            {
                if (balloonVisualDistractionObject.activeSelf){
                    if (Input.GetKey(KeyCode.Joystick1Button0) ||
                        Input.GetKey(KeyCode.Joystick1Button1) ||
                        Input.GetKey(KeyCode.Joystick1Button2) ||
                        Input.GetKey(KeyCode.Joystick1Button3) ||
                        Input.GetKey(KeyCode.R)){
                            balloonVisualDistractionObject.SetActive(false);
                            enableVisualBalloonTask = false;
                            TaskWriter(VisualButtonResponse, responseTime.ToString(), globalActivityTimer.ToString());
                            Debug.Log("Response to Balloon");
                            Thread.Sleep(100);
                    }
                }
            }
        }
        
    }

    private void UpdateUICrashInformation()
    {
        if (userHasCrashed)
        {
            instructionCrashText.SetActive(userHasCrashed);
            instructionCrashText.GetComponent<Text>().text = TimesCrashedLabel + timesUserHasCrashed;
        }
    }

    private void UpdateTaskMovingCar(){
        if (enableTaskMovingCar){
            if (movingCar != null)
            {
                //Moving Left -> Right
                if (movingCar.name == "Car1")
                {
                    if (movingCar.transform.position.x < LaneThirdMiddlePositionX)
                    {
                        movingCar.transform.Translate(new Vector3(0,0,-TaskCarMovementSpeed*Time.deltaTime));
                    }
                    else 
                    {
                        enableTaskMovingCar = false;
                    }
                }
                //Moving Right -> Left
                else if (movingCar.name == "Car3")
                {
                    if (movingCar.transform.position.x > LaneFirstMiddlePositionX)
                    {
                        movingCar.transform.Translate(new Vector3(0,0,TaskCarMovementSpeed*Time.deltaTime));
                    }
                    else 
                    {
                        enableTaskMovingCar = false;
                    }
                }
            }
        }        
    }

    private void TaskCarMovementPlayerResponse()
    {
        if (enableTaskMovingCar)
        {
            if (movingCar != null)
            {
                if (!userHasBraked)
                {
                    if (mainPlayer.transform.position.z < movingCar.transform.position.z)
                    {
                        if (!MainController.IsObjectAccelerating){
                            userHasBraked = true;
                            TaskWriter(MovingCarBreakResponse,responseTime.ToString(),globalActivityTimer.ToString());
                        }
                    }
                    else
                    {
                        if (MainController.IsObjectAccelerating){
                            userHasCrashed = true;
                            timesUserHasCrashed++;
                            TaskWriter(MovingCarCrashResponse,responseTime.ToString(),globalActivityTimer.ToString());
                            enableTaskMovingCar = false;
                        }
                    }
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
