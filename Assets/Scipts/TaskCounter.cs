using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Text;
public class TaskCounter
{
    private List<TaskObject> _taskList;

    private TaskObject _singleTaskVisual;
    private TaskObject _singleTaskLaneDeviation;
    private TaskObject _singleTaskAuditory;
    private TaskObject _singleTaskEmergencyBraking;

    private TaskObject _dualTaskLaneDeviationAuditoryForward;
    private TaskObject _dualTaskLaneDeviationAuditoryBackward;
    private TaskObject _dualTaskLaneDeviationVisualForward;
    private TaskObject _dualTaskLaneDeviationVisualBackward;
    private TaskObject _dualTaskEmergencyBrakingAuditoryForward;
    private TaskObject _dualTaskEmergencyBrakingAuditoryBackward;
    private TaskObject _dualTaskEmergencyBrakingVisualForward;
    private TaskObject _dualTaskEmergencyBrakingVisualBackward;

    private TaskObject _dualTaskLaneDeviationAuditorySimultaneous;
    private TaskObject _dualTaskLaneDeviationVisualSimultaneous;
    private TaskObject _dualTaskEmergencyBrakingAuditorySimultaneous;
    private TaskObject _dualTaskEmergencyBrakingVisualSimultaneous;

    private bool AllTaskHasFinished;

    public TaskCounter()
    {
        _taskList = new List<TaskObject>();

        //32 Tasks
        _singleTaskVisual = new TaskObject("Visual",TaskEnum.Single);
        _singleTaskLaneDeviation = new TaskObject("Lane Deviation",TaskEnum.Single);
        _singleTaskAuditory = new TaskObject("Auditory",TaskEnum.Single);
        _singleTaskEmergencyBraking= new TaskObject("Emergency Braking",TaskEnum.Single);

        //104 Tasks
        _dualTaskLaneDeviationAuditoryForward = new TaskObject("LaneDeviationAuditoryForward", TaskEnum.Double);
        _dualTaskLaneDeviationAuditoryBackward = new TaskObject("LaneDeviationAuditoryBackward", TaskEnum.Double);
        _dualTaskLaneDeviationVisualForward = new TaskObject("LaneDeviationVisualForward", TaskEnum.Double);
        _dualTaskLaneDeviationVisualBackward = new TaskObject("LaneDeviationVisualBackward", TaskEnum.Double);
        _dualTaskEmergencyBrakingAuditoryForward = new TaskObject("EmergencyBrakingAuditoryForward", TaskEnum.Double);
        _dualTaskEmergencyBrakingAuditoryBackward = new TaskObject("EmergencyBrakingAuditoryBackward", TaskEnum.Double);
        _dualTaskEmergencyBrakingVisualForward = new TaskObject("EmergencyBrakingVisualForward", TaskEnum.Double);
        _dualTaskEmergencyBrakingVisualBackward = new TaskObject("EmergencyBrakingVisualBackward", TaskEnum.Double);

        //52 Tasks
        _dualTaskLaneDeviationAuditorySimultaneous = new TaskObject("LaneDeviationAuditorySimultaneous",TaskEnum.Double);
        _dualTaskLaneDeviationVisualSimultaneous = new TaskObject("LaneDeviationVisualSimultaneous",TaskEnum.Double);
        _dualTaskEmergencyBrakingAuditorySimultaneous = new TaskObject("EmergencyBrakingAuditorySimultaneous",TaskEnum.Double);
        _dualTaskEmergencyBrakingVisualSimultaneous = new TaskObject("EmergencyBrakingVisual",TaskEnum.Double);

        _taskList.Add(_singleTaskVisual);
        _taskList.Add(_singleTaskLaneDeviation);
        _taskList.Add(_singleTaskAuditory);
        _taskList.Add(_singleTaskEmergencyBraking);

        _taskList.Add(_dualTaskLaneDeviationAuditoryForward);
        _taskList.Add(_dualTaskLaneDeviationAuditoryBackward);
        _taskList.Add(_dualTaskLaneDeviationVisualForward);
        _taskList.Add(_dualTaskLaneDeviationVisualBackward);
        _taskList.Add(_dualTaskEmergencyBrakingAuditoryForward);
        _taskList.Add(_dualTaskEmergencyBrakingAuditoryBackward);
        _taskList.Add(_dualTaskEmergencyBrakingVisualForward);
        _taskList.Add(_dualTaskEmergencyBrakingVisualBackward);

        _taskList.Add(_dualTaskLaneDeviationAuditorySimultaneous);
        _taskList.Add(_dualTaskLaneDeviationVisualSimultaneous);
        _taskList.Add(_dualTaskEmergencyBrakingAuditorySimultaneous);
        _taskList.Add(_dualTaskEmergencyBrakingVisualSimultaneous);
    }   

    public TaskObject AssignTask(){
        while (true){
            var randomAssignment = UnityEngine.Random.Range(0,16);
            TaskObject assignedTask = new TaskObject();
            if (randomAssignment == 0){
                assignedTask = _singleTaskVisual;
            }
            else if (randomAssignment == 1){
                assignedTask = _singleTaskLaneDeviation;
            }
            else if (randomAssignment == 2){
                assignedTask = _singleTaskAuditory;
            }
            else if (randomAssignment == 3){
                assignedTask = _singleTaskEmergencyBraking;
            }
            else if (randomAssignment == 4){
                assignedTask = _dualTaskLaneDeviationAuditoryForward;
            }
            else if (randomAssignment == 5){
                assignedTask = _dualTaskLaneDeviationAuditoryBackward;
            }
            else if (randomAssignment == 6){
                assignedTask=  _dualTaskLaneDeviationVisualForward;
            }
            else if (randomAssignment == 7){
                assignedTask = _dualTaskLaneDeviationVisualBackward;
            }
            else if (randomAssignment == 8){
                assignedTask = _dualTaskEmergencyBrakingAuditoryForward;
            }
            else if (randomAssignment == 9){
                assignedTask = _dualTaskEmergencyBrakingAuditoryBackward;
            }
            else if (randomAssignment == 10){
                assignedTask = _dualTaskEmergencyBrakingVisualForward;
            }
            else if (randomAssignment == 11){
                assignedTask = _dualTaskEmergencyBrakingVisualBackward;
            }
            else if (randomAssignment == 12){
                assignedTask = _dualTaskLaneDeviationAuditorySimultaneous;
            }
            else if (randomAssignment == 13){
                assignedTask = _dualTaskLaneDeviationVisualSimultaneous;
            }
            else if (randomAssignment == 14){
                assignedTask = _dualTaskEmergencyBrakingAuditorySimultaneous;
            }
            else if (randomAssignment == 15){
                assignedTask = _dualTaskEmergencyBrakingVisualSimultaneous;
            }

            if(_taskList.Count(x=> x.TaskCountCompleted() != true)!=0){
                if (!assignedTask.TaskCountCompleted()){
                    assignedTask.TaskCount++;
                    Debug.Log(assignedTask.TaskCount);
                    return assignedTask;
                }
            }
            else    
            {
                return null;
            }
        }
    }
    public TaskObject SingleTaskVisual
    {
        get => _singleTaskVisual;
        set => _singleTaskVisual = value;
    }

    public TaskObject SingleTaskLaneDeviation
    {
        get => _singleTaskLaneDeviation;
        set => _singleTaskLaneDeviation = value;
    }

    public TaskObject SingleTaskAuditory
    {
        get => _singleTaskAuditory;
        set => _singleTaskAuditory = value;
    }

    public TaskObject SingleTaskEmergencyBraking
    {
        get => _singleTaskEmergencyBraking;
        set => _singleTaskEmergencyBraking = value;
    }

    public TaskObject DualTaskLaneDeviationAuditoryForward
    {
        get => _dualTaskLaneDeviationAuditoryForward;
        set => _dualTaskLaneDeviationAuditoryForward = value;
    }

    public TaskObject DualTaskLaneDeviationAuditoryBackward
    {
        get => _dualTaskLaneDeviationAuditoryBackward;
        set => _dualTaskLaneDeviationAuditoryBackward = value;
    }

    public TaskObject DualTaskLaneDeviationVisualForward
    {
        get => _dualTaskLaneDeviationVisualForward;
        set => _dualTaskLaneDeviationVisualForward = value;
    }

    public TaskObject DualTaskLaneDeviationVisualBackward
    {
        get => _dualTaskLaneDeviationVisualBackward;
        set => _dualTaskLaneDeviationVisualBackward = value;
    }

    public TaskObject DualTaskEmergencyBrakingAuditoryForward
    {
        get => _dualTaskEmergencyBrakingAuditoryForward;
        set => _dualTaskEmergencyBrakingAuditoryForward = value;
    }

    public TaskObject DualTaskEmergencyBrakingAuditoryBackward
    {
        get => _dualTaskEmergencyBrakingAuditoryBackward;
        set => _dualTaskEmergencyBrakingVisualBackward = value;
    }

    public TaskObject DualTaskEmergencyBrakingVisualForward
    {
        get => _dualTaskEmergencyBrakingVisualForward;
        set => _dualTaskEmergencyBrakingVisualForward = value;
    }

    public TaskObject DualTaskEmergencyBrakingVisualBackward
    {
        get => _dualTaskEmergencyBrakingAuditoryBackward;
        set => _dualTaskEmergencyBrakingAuditoryBackward = value;
    }

    public TaskObject DualTaskLaneDeviationAuditorySimultaneous
    {
        get => _dualTaskLaneDeviationAuditorySimultaneous;
        set => _dualTaskLaneDeviationAuditorySimultaneous = value;
    }

    public TaskObject DualTaskLaneDeviationVisualSimultaneous
    {
        get => _dualTaskLaneDeviationVisualSimultaneous;
        set => _dualTaskLaneDeviationVisualSimultaneous = value;
    }

    public TaskObject DualTaskEmergencyBrakingAuditorySimultaneous
    {
        get => _dualTaskEmergencyBrakingAuditorySimultaneous;
        set => _dualTaskEmergencyBrakingAuditorySimultaneous = value;
    }

    public TaskObject DualTaskEmergencyBrakingVisualSimultaneous
    {
        get => _dualTaskEmergencyBrakingVisualSimultaneous;
        set => _dualTaskEmergencyBrakingVisualSimultaneous = value;
    }

}
