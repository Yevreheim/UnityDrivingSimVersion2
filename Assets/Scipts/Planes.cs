using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.TextCore;

public class Planes 
{
    public List<GameObject> Cars = new List<GameObject>();
    public GameObject HorizontalPlane;
    public GameObject Balloon;
    private const string CarTag = "Car";
    private const string BalloonTag = "Balloon";

    public Planes(GameObject HorizontalPlane){
        Cars.Clear();
        GetCarsObject(HorizontalPlane, CarTag);
    }

    private void GetCarsObject(GameObject horizontalPlane, string tag){
        Transform parent = horizontalPlane.transform;
        HorizontalPlane = horizontalPlane.gameObject;
        for (int i = 0; i < parent.childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.tag == tag){
                Cars.Add(child.gameObject);
                child.gameObject.SetActive(false);
            }
            if (child.tag == BalloonTag){
                Balloon = child.gameObject;
                child.gameObject.SetActive(false);
            }
        }
    }
}
