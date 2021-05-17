using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStarPathFinding : MonoBehaviour
{
    PFgrid grid;
    PathRequestManager requestManager;
    private void Awake() {
        grid = GetComponent<PFgrid>();
        requestManager = GetComponent<PathRequestManager>();
    }
    public void StartFindPath(Vector2 stratPos , Vector2 endPos){
        StartCoroutine(FindPath(stratPos,endPos));
    }
    IEnumerator FindPath(Vector2 StartPos, Vector2 TargetPos){
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector2[] waypoints = new Vector2[0];
        bool PathSuccess = false;
        PathFindNode StartNode = grid.GetNodeFromWorldPos(StartPos);
        PathFindNode TargetNode = grid.GetNodeFromWorldPos(TargetPos);
        Heap<PathFindNode> openSet = new Heap<PathFindNode>(grid.TotalCells);
        HashSet<PathFindNode> closeSet = new HashSet<PathFindNode>();
        openSet.Add(StartNode);
        if(TargetNode.Walkable && StartNode.Walkable)
        {
            while(openSet.Count > 0){
                PathFindNode ActNode = openSet.RemoveFirst();
                closeSet.Add(ActNode);
                if(ActNode.WorldPos == TargetNode.WorldPos){
                    sw.Stop();
                    UnityEngine.Debug.Log("Camino encontrado en" + sw.ElapsedMilliseconds + "ms");
                    PathSuccess = true;
                    break;
                }
                List<PathFindNode> vecinos = grid.GetVecinos(ActNode);
                for(int i = 0 ; i < vecinos.Count; i++){
                    PathFindNode vecino = vecinos[i];
                    if(closeSet.Contains(vecino)) continue;
                    
                    int NewMovCostToVecino = ActNode.GCost + GetDistance(ActNode,vecino);

                    if(NewMovCostToVecino < vecino.GCost || !openSet.ContainsItem(vecino)){
                        vecino.GCost = NewMovCostToVecino;
                        vecino.HCost = GetDistance(vecino,TargetNode);
                        vecino.ParentGridPos = ActNode;
                        if(!openSet.ContainsItem(vecino))
                            openSet.Add(vecino);
                    }
                }
            }
        }
       
        yield return null;
        if(PathSuccess)
          waypoints =  RetracePath(StartNode,TargetNode);
        requestManager.FinishProssesingPath(waypoints,PathSuccess);
        
    }

    //14 es el peso para un movimiento diagonal y 10 para horizontal o vertical
    int GetDistance(PathFindNode NodeA , PathFindNode NodeB){
        int distX = Mathf.Abs((int)NodeA.GridPosX - (int)NodeB.GridPosX);
        int distY = Mathf.Abs((int)NodeA.GridPosY - (int)NodeB.GridPosY);

        if(distX > distY){
            return 14 * distY + 10 * (distX - distY);
        }
        else{
            return 14 * distX + 10 * (distY - distX);
        }
    }

    Vector2[] RetracePath(PathFindNode StartNode , PathFindNode TargetNode){
        List<PathFindNode> path = new List<PathFindNode>();
        PathFindNode CurrentNode = TargetNode;
        while(CurrentNode != StartNode){
            path.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentGridPos;
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector2[] SimplifyPath(List<PathFindNode> path){
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 OldDir = Vector2.zero;
        for(int i = 1 ; i < path.Count; i++){
            Vector2 NewDir = new Vector2(path[i-1].GridPosX - path[i].GridPosX,path[i-1].GridPosY - path[i].GridPosY );
            /*if(NewDir != OldDir){
                waypoints.Add(path[i].WorldPos);
                OldDir = NewDir;
            }*/
                waypoints.Add(path[i].WorldPos);

        }
        return waypoints.ToArray();
    }
}
