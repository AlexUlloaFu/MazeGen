using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CreateRoad : MonoBehaviour
{
    SpriteShapeController MySSC;
    int NumberOfPoints;
    void Start()
    {
        MySSC = GetComponent<SpriteShapeController>();
        NumberOfPoints =  MySSC.spline.GetPointCount();
    }

    void Update()
    {
        
    }
}
