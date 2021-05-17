using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFgrid : MonoBehaviour
{
    public Transform player;
    public Vector2 gridWorldSize;
    public float NodeSize;
    public LayerMask UnWalkableMask;
    [HideInInspector]
    public PathFindNode[,] grid;
    public int gridSizeX,gridSizeY;
    public int TotalCells;
    public float TimeToCreateGrid = 1;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //encontrar de acuerdo al radio x nodo cuantos nodos caben en el grid
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / NodeSize);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / NodeSize);
        TotalCells = gridSizeX * gridSizeY;
        Invoke("CreateGrid",TimeToCreateGrid);
    }
    private void CreateGrid(){
        grid = new PathFindNode[gridSizeX,gridSizeY];
        Vector2 gridWorldBottomLeft = (Vector2)transform.position - new Vector2(gridWorldSize.x/2,gridWorldSize.y/2);
        for(int i = 0 ; i < gridSizeX ; i++){
            for(int j = 0 ; j < gridSizeY; j++){
                Vector2 worldPoint = gridWorldBottomLeft + new Vector2(i * NodeSize + NodeSize/2,j*NodeSize + NodeSize/2);
                bool IsWalkable = (!Physics2D.OverlapCircle(worldPoint,.1f,UnWalkableMask));
                grid[i,j] = new PathFindNode(IsWalkable,worldPoint ,i,j);
            }
        }
    }

    public PathFindNode GetNodeFromWorldPos(Vector2 worldPos){
        //Saco el porciento en base 1 de la grid en el q estoy
        float percentX = (worldPos.x + gridWorldSize.x/2)/ gridWorldSize.x;
        float percentY = (worldPos.y + gridWorldSize.y/2) / gridWorldSize.y; 
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        //y al yo multiplicar ese porciento por respectivo tamanno ya me da el index 
        int x = Mathf.RoundToInt(  (gridSizeX - 1) * percentX );
        int y = Mathf.RoundToInt( (gridSizeY - 1) * percentY );
        return grid[x,y];
    }
    public List<PathFindNode> GetVecinos(PathFindNode CurrentNode){
        List<PathFindNode> vecinos = new List<PathFindNode>();
        for(int i = -1 ; i <= 1; i++ ){
            for(int j = -1 ; j <= 1; j++){
                if(i == 0 && j == 0) continue;
                int checkX = (int)CurrentNode.GridPosX + i;
                int checkY = (int)CurrentNode.GridPosY + j;
                if(checkX > -1 && checkX < gridSizeX && checkY > -1 && checkY < gridSizeY){
                    PathFindNode Vecino = grid[checkX,checkY];
                    if(Vecino.Walkable)
                        vecinos.Add(Vecino);
                }
            }
        }
        return vecinos;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position,gridWorldSize);
        if( grid != null ){
            PathFindNode PlayerNode = GetNodeFromWorldPos(player.localPosition);
            foreach(PathFindNode N in grid){
                if(N == PlayerNode)
                    Gizmos.color = Color.magenta;
                else Gizmos.color = (N.Walkable) ? Color.green : Color.red;
                Gizmos.DrawCube(N.WorldPos, Vector2.one * (NodeSize - .1f));
            }
        }
    }
}
