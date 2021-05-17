using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOrderInRealTime : MonoBehaviour
{
    SpriteRenderer MySpriteRenderer;

    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        MySpriteRenderer.sortingOrder = (int)((transform.position.y * 100 )* -1);
        
    }
}
