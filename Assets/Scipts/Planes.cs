using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class Planes
{
    private const string CarTag = "Car";
    private const string BalloonTag = "Balloon";
    private const string Car1Name = "Car1";
    private const string Car2Name = "Car2";
    private const string Car3Name = "Car3";
    private const string Car4Name = "Car4";
    public List<GameObject> Cars = new List<GameObject>();
    public GameObject HorizontalPlane;
    public GameObject Balloon;

    private float Car1DefaultPositionX;
    private float Car2DefaultPositionX;
    private float Car3DefaultPositionX;
    private float Car4DefaultPositionX;

    public Planes(GameObject HorizontalPlane){
        Cars.Clear();
        GetCarsObject(HorizontalPlane);
    }
    
    public void ResetCarPosition(){
        var positionZ = HorizontalPlane.transform.position.z;
        foreach(GameObject car in Cars){
            if (car.name == Car1Name){
                car.transform.position = new Vector3(Car1DefaultPositionX,0,positionZ);
            }
            else if (car.name == Car2Name){
                car.transform.position = new Vector3(Car2DefaultPositionX,0,positionZ);
            }
            else if (car.name == Car3Name){
                car.transform.position = new Vector3(Car3DefaultPositionX,0,positionZ);
            }
            else if (car.name == Car4Name){
                car.transform.position = new Vector3(Car4DefaultPositionX,0,positionZ);
            }
        }
    }

    private void GetCarsObject(GameObject horizontalPlane){
        Transform parent = horizontalPlane.transform;
        HorizontalPlane = horizontalPlane.gameObject;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.tag == CarTag){
                Cars.Add(child.gameObject);
                child.gameObject.SetActive(false);
                if (child.name == Car1Name){
                    Car1DefaultPositionX = child.transform.position.x;
                }
                else if (child.name == Car2Name){
                    Car2DefaultPositionX = child.transform.position.x;
                }
                else if (child.name == Car3Name){
                    Car3DefaultPositionX = child.transform.position.x;
                }
                else if (child.name == Car4Name){
                    Car4DefaultPositionX = child.transform.position.x;
                }
            }
            if (child.tag == BalloonTag){
                Balloon = child.gameObject;
                child.gameObject.SetActive(false);
            }
        }
    }
}
