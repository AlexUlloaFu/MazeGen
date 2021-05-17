using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class SetLayerToTileMap : MonoBehaviour
{
    STETilemap MyTileMap;
    public int Multiplier = -1;
    public Transform lala;
    // Start is called before the first frame update
    void Start()
    {
        MyTileMap = GetComponentInChildren<STETilemap>();
        MyTileMap.OrderInLayer = (int)((lala.position.y * 100 )* Multiplier );
    }
    private void OnEnable() {
        if(lala == null) lala = GetComponent<Transform>();
        
    }

    // Update is called once per frame
}
