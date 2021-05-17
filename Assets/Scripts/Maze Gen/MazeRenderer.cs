using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject WallHorizontalPerfab;
    public GameObject WallVerticalPerfab;
    public GameObject FloorPrefab;
    public GameObject[] TrapsPrefab;
    public GameObject[] EnemiesPrefab;
    public float TrapsMultiplier;
    public float EnemiesMultiplier;
    private int MaxTraps;
    private int MaxEnemies;
    [SerializeField]
    private int TrapsCount;
    [SerializeField]
    private int EnemiesCount;

    public Transform Player;
    public Transform End;
    public Transform Path;
    public float CellSize;
    public Position LastPos;
    public Transform Parent;
    void Start()
    {
        MaxTraps = Mathf.RoundToInt(height * width * TrapsMultiplier);
        MaxEnemies = Mathf.RoundToInt(height * width * EnemiesMultiplier);
        var maze = MazeGenerator.Generate(width,height);
        Parent = gameObject.transform;
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        End = GameObject.FindGameObjectWithTag("END").transform;
        Player.position = new Vector3((-width/2) * CellSize , (-height/2) * CellSize,0);
        DrawMaze(maze);
        //StartCoroutine(DrawPath());
    }

    IEnumerator DrawPath(){
        while(MazeGenerator.Previous[LastPos.x,LastPos.y].x != -1 && MazeGenerator.Previous[LastPos.x,LastPos.y].y != -1){
            Instantiate(Path,new Vector2((-width/2 + MazeGenerator.Previous[LastPos.x,LastPos.y].x ) * CellSize ,(-height/2 + MazeGenerator.Previous[LastPos.x,LastPos.y].y) * CellSize ),Quaternion.identity,Parent);
            LastPos = MazeGenerator.Previous[LastPos.x,LastPos.y];
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    private void DrawMaze(WallState[,] maze){
        int biggestDist = 0;
        int EndPosX = 0 ,EndPosY = 0;
        for(int i = 0 ; i < width ; i++){
            for(int j = 0 ; j < height ; j++){
                var cell = maze[i,j];
                var gridPosition = new Vector3((-width/2 + i) * CellSize , (-height/2 + j) * CellSize,0);
                if(biggestDist < MazeGenerator.Distance[i,j]){
                    biggestDist = MazeGenerator.Distance[i,j];
                    EndPosX = i;
                    EndPosY = j;
                }
                GameObject floor = Instantiate(FloorPrefab,Parent);
                floor.transform.position = gridPosition;
                floor.transform.localScale = new Vector3(CellSize/3.75f,CellSize/3.75f,floor.transform.localScale.z);

                //spawnTrap
                int chance = Random.Range(0,3);
                if(chance == 0 && MaxTraps > TrapsCount ){
                    if(i == 0 && j == 0)
                    {
                        
                    }
                    else
                    {
                    int index = Random.Range(0,TrapsPrefab.Length);
                    GameObject Trap = Instantiate(TrapsPrefab[index],Parent);
                    Trap.transform.position =  gridPosition;
                    TrapsCount++;
                    }
                }

                chance = Random.Range(0,6);
                if(chance == 0 && MaxEnemies > EnemiesCount ){
                    if(i == 0 && j == 0)
                    {
                        
                    }
                    else
                    {
                    int index = Random.Range(0,EnemiesPrefab.Length);
                    GameObject Enemy = Instantiate(EnemiesPrefab[index],Parent);
                    Enemy.transform.position =  gridPosition;
                    EnemiesCount++;
                    }
                }

                float offset = (CellSize/2);
                if(cell.HasFlag(WallState.Up)){
                    GameObject TopWall = Instantiate(WallHorizontalPerfab,Parent);
                    TopWall.transform.position = gridPosition + new Vector3(0,offset,0);
                    //TopWall.transform.eulerAngles = new Vector3(0,0,90);
                    //TopWall.transform.localScale = new Vector3(TopWall.transform.localScale.x,CellSize,TopWall.transform.localScale.z);
                }
                if(cell.HasFlag(WallState.Left)){
                    GameObject LeftWall = Instantiate(WallVerticalPerfab,Parent);
                    LeftWall.transform.position = gridPosition + new Vector3(-offset,0,0);
                    //LeftWall.transform.localScale = new Vector3(LeftWall.transform.localScale.x,CellSize,LeftWall.transform.localScale.z);
                }
                //Solo instanciar la de la derecha en el borde derecho , sino no necesario
                if(i == width - 1){
                    if(cell.HasFlag(WallState.Right)){
                    GameObject RightWall = Instantiate(WallVerticalPerfab,Parent);
                    //RightWall.transform.localScale = new Vector3(RightWall.transform.localScale.x,CellSize,RightWall.transform.localScale.z);
                    RightWall.transform.position = gridPosition + new Vector3(offset,0,0);
                    }
                }
                if(j == 0){
                    GameObject BottomWall = Instantiate(WallHorizontalPerfab,Parent);
                    //BottomWall.transform.localScale = new Vector3(BottomWall.transform.localScale.x,CellSize,BottomWall.transform.localScale.z);
                    BottomWall.transform.position = gridPosition + new Vector3(0,-offset,0);
                    //BottomWall.transform.eulerAngles = new Vector3(0,0,90);
                }
            }
        }
        LastPos = new Position{x = EndPosX , y = EndPosY};
        End.position = new Vector3((-width/2 + EndPosX) * CellSize , (-height/2 + EndPosY) * CellSize,0);
    }
}
