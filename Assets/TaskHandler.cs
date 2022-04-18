using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Text;


public class TaskHandler : MonoBehaviour {
    private static float GlobalTimer;
    private static float TaskTimerController;
    private static float SecondTaskTimer;
    private static float SecondTaskReference;
    private static int PlaneHolder;

    //Array to count both variations
    public static int TaskLaneDeviation = 0;
    public static int TaskEmergencyBraking = 0;
    public static int TaskAuditoryDistraction = 0;
    public static int TaskVisualCarDistraction = 0;
    public static int[] TaskLDA = new int[]{0,0};
    public static int[] TaskLDV = new int[]{0,0};
    public static int[] TaskEBA = new int[]{0,0};
    public static int[] TaskEBV = new int[]{0,0};
    private static int[] TaskArray = new int[]{0,0,0,0,0,0,0,0,0,0,0,0};
    private static string[] taskNames = new string[]{"Visual","LD","Auditory","EB","LD/A","A/LD","LD/V","V/LD","EB/A","A/EB","EB/V","V/EB"};

    //File Handling
    public static string pathText;
    public static string pathExcel;
    public static string fileTextName;
    public static string fileExcelName;
    public static string directoryDesktop =  Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
    public static StreamWriter excelSW;
    public static StreamReader excelSR;
    

    void Start()    
    {
        GlobalTimer = 0;
        SecondTaskTimer = 0;
        SecondTaskReference = 0;
        fileExcelName = "/ObservationLog_" + System.DateTime.Now.ToString("MM-dd-yy_hh-mm-ss") + ".csv";
        pathExcel = Application.persistentDataPath + fileExcelName;
        excelSW = new StreamWriter(pathExcel);
        var line = String.Format("{0},{1},{2}","Global Time: ","Reaction Time: ","Activity: ");
        Debug.Log(pathExcel);
        using (excelSW){
            excelSW.WriteLine(line);
        }
        Debug.Log(Application.persistentDataPath);

    }

    void Update(){
        GlobalTimer = Timer.GlobalClock;
        SecondTaskTimer = Timer.GlobalClock;
        TaskVisualCarDistraction = TaskArray[0];
        TaskLaneDeviation = TaskArray[1];
        TaskAuditoryDistraction = TaskArray[2];
        TaskEmergencyBraking = TaskArray[3];
        TaskLDA[0] = TaskArray[4];
        TaskLDA[1] = TaskArray[5];
        TaskLDV[0] = TaskArray[6];
        TaskLDV[1] = TaskArray[7];
        TaskEBA[0] = TaskArray[8];
        TaskEBA[1] = TaskArray[9];
        TaskEBV[0] = TaskArray[10];
        TaskEBV[1] = TaskArray[11];

        //Second Task Recognition
        if (SecondTaskReference != 0){
            if (SecondTaskTimer > 0.4){
                //Visual
                if (SecondTaskReference == 1){
                    RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(1),PlaneHolder);
                }
                //Lane Deviation
                else if (SecondTaskReference == 2){
                    TaskDeviation.Signal = 1;
                }
                //Auditory
                else if (SecondTaskReference == 3){
                    AuditoryDistraction.Signal = 1;
                }
                //Emergency Braking
                else if (SecondTaskReference == 4){
                    int QuickRandom = UnityEngine.Random.Range(0,2);
                    if (QuickRandom == 0){
                        RespawningHierarchy.CarMovement = 0;
                    }
                    else if (QuickRandom == 1){
                        RespawningHierarchy.CarMovement = 2;
                    }
                    RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(RespawningHierarchy.CarMovement),PlaneHolder);
                    RespawningHierarchy.RespawningCarDeviation = 1;
                    RespawningHierarchy.CarMovementPlaneReference = PlaneHolder;
                }
                SecondTaskReference = 0;
            }
        }
    }
    public static void EventWriter(string Activity,string timeDuration, string timeGlobal){
        //Excel
        string Holder = timeGlobal;
        using (StreamWriter Writer = new StreamWriter(pathExcel, true)){
            Holder += "," + timeDuration;
            Holder += "," + Activity;
            Writer.WriteLine(Holder);
            Writer.Close();
        }
    }
    public static int ArrayCheck(int Check){
        while (true){
            if (TaskArray[Check] <= 8){
                return Check;
            }
            else if (TaskArray[Check] > 8){
                Check = UnityEngine.Random.Range(0,11);
            }
        }
    }

    public static void TaskProcessor(int PlaneNumber){
        //This thing throw back out into RH
        RespawningHierarchy.RespawningCarDeviation = 0;
        //Main
        int RandomSelection = UnityEngine.Random.Range(0,12);
        
        //Filtering
        RandomSelection = ArrayCheck(RandomSelection);
        //Event Handler
        TaskArray[RandomSelection]++;
        PlayerController.TaskList.Add(taskNames[RandomSelection]);
        EventWriter(taskNames[RandomSelection],Timer.TimerClock.ToString(),Timer.GlobalClock.ToString());
        
        //Visual Car Distraction
        if (RandomSelection == 0){
            Debug.Log("Visual");
            //Respawn.RandomAllocation(1, PlanePosition);
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(1),PlaneNumber);
        }
        //Lane Deviation
        else if (RandomSelection == 1){
            Debug.Log("LD");
            TaskDeviation.Signal = 1;
        }
        //Sound and Select -- Auditory Distraction
        else if (RandomSelection == 2){
            Debug.Log("Auditory");
            AuditoryDistraction.Signal = 1;
        }
        //Emergency Braking
        else if (RandomSelection == 3){
            Debug.Log("EB");
            int QuickRandom = UnityEngine.Random.Range(0,2);
            if (QuickRandom == 0){
                RespawningHierarchy.CarMovement = 0;
            }
            else if (QuickRandom == 1){
                RespawningHierarchy.CarMovement = 2;
            }
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(RespawningHierarchy.CarMovement),PlaneNumber);
            RespawningHierarchy.RespawningCarDeviation = 1;
            RespawningHierarchy.CarMovementPlaneReference = PlaneNumber;
            RespawningHierarchy.GlobalPlaneReference = PlaneNumber;
            Debug.Log("Plane Number:" + PlaneNumber);
            EventWriter("LANE" + PlaneNumber,0.ToString(),0.ToString());
        }
        //Lane Deviation -> Auditory
        else if (RandomSelection == 4){
            Debug.Log("LD/A");
            TaskDeviation.Signal = 1;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 3;
        }
        //Auditory -> Lane Deviation
        else if (RandomSelection == 5){
            Debug.Log("A/LD");
            AuditoryDistraction.Signal = 1;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 2;
        }
        //Lane Deviation -> Visual
        else if (RandomSelection == 6){
            Debug.Log("LD/V");
            TaskDeviation.Signal = 1;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 1;
        }
        //Visual -> Lane Deviation
        else if (RandomSelection == 7){
            Debug.Log("V/LD");
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(1),PlaneNumber);
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 2;
        }
        //Emergency Braking -> Auditory
        else if (RandomSelection == 8){
            Debug.Log("EB/A");
            int QuickRandom = UnityEngine.Random.Range(0,2);
            if (QuickRandom == 0){
                RespawningHierarchy.CarMovement = 0;
            }
            else if (QuickRandom == 1){
                RespawningHierarchy.CarMovement = 2;
            }
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(RespawningHierarchy.CarMovement),PlaneNumber);
            RespawningHierarchy.RespawningCarDeviation = 1;
            RespawningHierarchy.CarMovementPlaneReference = PlaneNumber;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 3;
        }
        //Auditory -> Emergency Braking
        else if (RandomSelection == 9){
            Debug.Log("A/EB");
            AuditoryDistraction.Signal = 1;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 4;
        }
        //Emergency Braking -> Visual
        else if (RandomSelection == 10){
            Debug.Log("EB/V");
            int QuickRandom = UnityEngine.Random.Range(0,2);
            if (QuickRandom == 0){
                RespawningHierarchy.CarMovement = 0;
            }
            else if (QuickRandom == 1){
                RespawningHierarchy.CarMovement = 2;
            }
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(RespawningHierarchy.CarMovement),PlaneNumber);
            RespawningHierarchy.RespawningCarDeviation = 1;
            RespawningHierarchy.CarMovementPlaneReference = PlaneNumber;
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 1;
        }
        //Visual -> Emergency Braking
        else if (RandomSelection == 11){
            Debug.Log("V/EB");
            RespawningHierarchy.SelectiveCarActivator(RespawningHierarchy.RandomAllocation(1),PlaneNumber);
            PlaneHolder = PlaneNumber;
            SecondTaskReference = 4;
        }
    }
}