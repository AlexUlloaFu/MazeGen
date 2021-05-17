using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrdenInLayer : MonoBehaviour
{
    SpriteRenderer MySpriteRenderer;
    void Start()
    {
        MySpriteRenderer = GetComponent<SpriteRenderer>();
        MySpriteRenderer.sortingOrder = (int)((transform.position.y * 100 )* -1 );
        if(gameObject.tag == "Props") MySpriteRenderer.sortingOrder -= 3050;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
