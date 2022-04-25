using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Linq;
using System.IO;
using System.Text;

public class TaskObject
{
    private const int SingleTaskCount = 8;
    private const int DualTaskCount = 13;
    private string _taskName;
    private string _taskCategory;
    private int _taskMaximum;
    private int _taskCount;
    private TaskEnum _taskType;

    public TaskObject(){
        
    }

    public TaskObject(string TaskName, TaskEnum TaskType)
    {
        _taskName = TaskName;
        _taskType = TaskType;
        if (TaskType == TaskEnum.Single){
            _taskMaximum = SingleTaskCount;
        }
        else if (TaskType == TaskEnum.Double){
            _taskMaximum = DualTaskCount;
        }
    }

    public bool TaskCountCompleted()
    {
        if (_taskType == TaskEnum.Single){
            if (_taskCount >= SingleTaskCount){
                return true;
            }
            else {
                return false;
            }
        }
        else 
        {
            if (_taskCount >= DualTaskCount){
                return true;
            }
            else {
                return false;
            }
        }
    }

    public string TaskName
    {
        get => _taskName;
        set => _taskName = value;
    }

    public TaskEnum TaskType{
        get => _taskType;
        set => _taskType = value;
    }

    public int TaskMaximum{
        get => _taskMaximum;
        set => _taskMaximum = value;
    }

    public int TaskCount{
        get => _taskCount;
        set => _taskCount = value;
    }
}   
