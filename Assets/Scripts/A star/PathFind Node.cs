using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindNode : IHeapItem<PathFindNode>
{
    public bool Walkable;
    public  Vector2 WorldPos;
    public int GridPosX;
    public int GridPosY;

    public int GCost;
    public int HCost;
    public int FCost{get{return HCost + GCost;}}   
    public PathFindNode ParentGridPos;
   public int _heapIndex;

    public PathFindNode(bool walkable,Vector2 worldPos,int GridPosX , int gridPosY){
        this.Walkable = walkable;
        this.WorldPos = worldPos;
        this.GridPosY = gridPosY;
        this.GridPosX = GridPosX;

    }

    public int HeapIndex{
        get{
            return _heapIndex;
        }
        set{
            _heapIndex = value;
        }
    }


    // este es el metodo q me dice quien tiene mas prioridad siendo 1 mas prioridad que, 0 igual prioridad que y -1 menos prioridad que
    public int CompareTo(object ToCompare){
        PathFindNode NodeToCompare = (PathFindNode)ToCompare;
        int compare = FCost.CompareTo(NodeToCompare.FCost);
        if(compare == 0){
            //si son iguales hago el desempate con el costo hacia el destino
            compare = HCost.CompareTo(NodeToCompare.HCost);
        }
        return -compare;
    }

}
