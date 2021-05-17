using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LuzLatiendo : MonoBehaviour
{
    public float MinOuterRadius;
    public float MaxOuterRadius;
    public float frecuencia;
    public float speed;
    private float time;
    private float currtime;
    private Light2D MyLuz;
    private bool ShouldAdd;
    
    void Start()
    {
        ShouldAdd = true;
        MyLuz = GetComponent<Light2D>();
        currtime = speed;
        time = speed/frecuencia;
        
    }

    // Update is called once per frame
    void Update()
    {
        currtime -= Time.deltaTime;
        if(currtime <= 0){
            if(ShouldAdd)
                MyLuz.pointLightOuterRadius += time;
            else  MyLuz.pointLightOuterRadius -= time;
            if(MyLuz.pointLightOuterRadius >= MaxOuterRadius || MyLuz.pointLightOuterRadius <= MinOuterRadius)
                ShouldAdd = !ShouldAdd;
            currtime = speed;
        }
    }
}
