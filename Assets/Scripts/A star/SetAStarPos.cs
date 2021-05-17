using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAStarPos : MonoBehaviour
{
    public LevelGeneration MyLVLG;
    private PFgrid MyGrid;
    void Awake()
    {
        MyGrid = GetComponent<PFgrid>();
        MyGrid.TimeToCreateGrid = 4;
        MyGrid.gridWorldSize = new Vector2(MyLVLG.CellsInX * 10 , MyLVLG.CellsInY * 10);
        transform.position = new Vector2((MyLVLG.CellsInX - 1) * 5,(MyLVLG.CellsInY - 1) * -5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
