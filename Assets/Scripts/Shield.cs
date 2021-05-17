using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float RotationSpeed;
    public float MinAlpha;
    public float MaxAlpha;
    public float VariationSpeed;
    SpriteRenderer MySR;
    bool ShouldAdd;
    void Start()
    {
        MySR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.z + Time.deltaTime * RotationSpeed );
        
        if(ShouldAdd)
            MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,MySR.color.a + Time.deltaTime * VariationSpeed);
        else  MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,MySR.color.a - Time.deltaTime * VariationSpeed);
        
        if(MySR.color.a >= MaxAlpha || MySR.color.a <= MinAlpha){
            ShouldAdd = !ShouldAdd;
            if(MySR.color.a >= MaxAlpha)
                MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,MaxAlpha);
            else MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,MinAlpha);
        }
    }
}
