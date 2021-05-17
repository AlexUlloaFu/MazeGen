using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreInternalCollision : MonoBehaviour
{
    public Collider2D collider1;
    public Collider2D collider2;
    void Start()
    {
        Physics2D.IgnoreCollision(collider1,collider2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
