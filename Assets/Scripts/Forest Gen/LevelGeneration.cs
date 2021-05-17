using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour {

    public List <Transform> startingPositions = new List<Transform>();
    public GameObject[] rooms; // index 0 --> closed, index 1 --> LR, index 2 --> LRB, index 3 --> LRT, index 4 --> LRBT

    private int direction;
    private bool stopGeneration;
    private int downCounter;

    public float moveIncrement;
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;

    public LayerMask whatIsRoom;
    public int CellsInX;
    public int CellsInY;
    public Transform Player;
    public int MaxEnemies;
    public GameObject BigPapa;
    public GameObject[] Enemies;
    public GameObject VerticalBorder;
    public GameObject HorizontalBorder;
    public GameObject RoomPos;
    public GameObject FinalRoom;
    public SpriteShapeController MySSC;
    public List<Vector2> SplinePoints = new List<Vector2>();
    bool once = false;
    private void Start()
    {
        transform.position = Vector2.zero;
        for(int i = 0 ; i < CellsInX;i++){
            for(int j = 0 ; j < CellsInY; j++){
            GameObject Current = Instantiate(RoomPos,(Vector2)transform.position + new Vector2(i*10,j*-10),Quaternion.identity,BigPapa.transform);
            RoomPos CurrentRoomPos = Current.GetComponent<RoomPos>();
            CurrentRoomPos.CellsInX = CellsInX;
            CurrentRoomPos.CellsInY = CellsInY;
            CurrentRoomPos.VerticalBorder = VerticalBorder;
            CurrentRoomPos.HorizontalBorder = HorizontalBorder;
            if(j == 0)
                startingPositions.Add(Current.transform);
            }
        }
        int randStartingPos = Random.Range(0, startingPositions.Count);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity,BigPapa.transform);
        Player.position = transform.position;
        direction = Random.Range(1, 6);
    }

    private void Update()
    {
        if (timeBtwSpawn <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else {
            timeBtwSpawn -= Time.deltaTime;
        }
        if(stopGeneration && once == false){
            SetPointsToSpline();
            Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
            Destroy(previousRoom.gameObject);
            Instantiate(FinalRoom,transform.position,Quaternion.identity,BigPapa.transform);
            once = true;
        }
    }

    private void Move()
    {

        if (direction == 1 || direction == 2)
        { // Move right !
          
            if (transform.position.x < CellsInX * 10 -20)
            {
                DrawBorders();
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);
                transform.position = pos;
                SplinePoints.Add(new Vector2(pos.x - moveIncrement/2,pos.y ));
                //SplinePoints.Add(new Vector2(pos.x - 2.5f,pos.y ));
                SplinePoints.Add(pos);

                int randRoom = Random.Range(1, 4);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity,BigPapa.transform);
                SpawnEnemies(transform.position);
                // Makes sure the level generator doesn't move left !
                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 1;
                }
                else if (direction == 4)
                {
                    direction = 5;
                }
            }
            else {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4)
        { // Move left !
           
            if (transform.position.x > 0)
            {
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
                transform.position = pos;
                SplinePoints.Add(new Vector2(pos.x + moveIncrement/2,pos.y ));
                SplinePoints.Add(pos);


                int randRoom = Random.Range(1, 4);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity,BigPapa.transform);
                SpawnEnemies(transform.position);
                DrawBorders();
                direction = Random.Range(3, 6);
            }
            else {
                direction = 5;
            }
           
        }
        else if (direction == 5)
        { // MoveDown
            downCounter++;
            if (transform.position.y > CellsInY * -10 + 20)
            {
                // Now I must replace the room BEFORE going down with a room that has a DOWN opening, so type 3 or 5
                Collider2D previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
                Debug.Log(previousRoom);
                if (previousRoom.GetComponent<Room>().roomType != 4 && previousRoom.GetComponent<Room>().roomType != 2)
                {

                    // My problem : if the level generation goes down TWICE in a row, there's a chance that the previous room is just 
                    // a LRB, meaning there's no TOP opening for the other room ! 

                    if (downCounter >= 2)
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        Instantiate(rooms[4], transform.position, Quaternion.identity,BigPapa.transform);
                        SpawnEnemies(transform.position);
                        DrawBorders();
                    }
                    else
                    {
                        previousRoom.GetComponent<Room>().RoomDestruction();
                        int randRoomDownOpening = Random.Range(2, 5);
                        if (randRoomDownOpening == 3)
                        {
                            randRoomDownOpening = 2;
                        }
                        Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity,BigPapa.transform);
                        SpawnEnemies(transform.position);
                        DrawBorders();


                    }

                }
                
               
  
                Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrement);
                transform.position = pos;
                SplinePoints.Add(new Vector2(pos.x ,pos.y + moveIncrement/2));
                SplinePoints.Add(pos);


                // Makes sure the room we drop into has a TOP opening !
                int randRoom = Random.Range(3, 5);
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity,BigPapa.transform);
                SpawnEnemies(transform.position);
                DrawBorders();
                direction = Random.Range(1, 6);
            }
            else {
                stopGeneration = true;
            }
            
        }
    }
    public void SpawnEnemies(Vector2 position){
        int Number = Random.Range(0,MaxEnemies);
        for(int i = 0; i < Number; i++){
            int index = Random.Range(0,Enemies.Length);
            int RanX = Random.Range(-4,5);
            int RanY = Random.Range(-4,5);
            Vector2 Pos = new Vector2(position.x + RanX, position.y + RanY);
            Instantiate(Enemies[index],Pos,Quaternion.identity,BigPapa.transform);
        }
    } 

    private void DrawBorders(){
            Quaternion alfonso = Quaternion.Euler(0,0,-90);

            if(transform.position.x == (CellsInX) * 5)
                Instantiate(VerticalBorder,new Vector3(transform.position.x + 5,transform.position.y),Quaternion.identity,BigPapa.transform);
            if(transform.position.y == (CellsInY) * -5){
                Instantiate(HorizontalBorder,new Vector3(transform.position.x,transform.position.y - 5),alfonso,BigPapa.transform);
            }
            if(transform.position.y == 0) {
                Instantiate(HorizontalBorder,new Vector3(transform.position.x,transform.position.y + 5),alfonso,BigPapa.transform);
            }
            if(transform.position.x == 0){
                Instantiate(VerticalBorder,new Vector3(transform.position.x - 5,transform.position.y),Quaternion.identity,BigPapa.transform);
            }

    }
    private void SetPointsToSpline(){
        int count = MySSC.spline.GetPointCount();
        MySSC.spline.SetPosition(0,Player.position);
        MySSC.spline.SetPosition(1,SplinePoints[0]);
        for(int i = 2 ; i <= SplinePoints.Count ; i++){
            MySSC.spline.InsertPointAt(i,SplinePoints[i-1]);
           

            if(MySSC.spline.GetPosition(i-2).y !=  SplinePoints[i-1].y){
                MySSC.spline.SetTangentMode(i-2,ShapeTangentMode.Continuous);
                MySSC.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                
            }
            /*if(i%3 == 0){
                Vector2 PreviuosPos = MySSC.spline.GetPosition(i-1);
                Vector2 CurrentPos = MySSC.spline.GetPosition(i);
                Vector2 Offset =  new Vector2(PreviuosPos.x - CurrentPos.x , PreviuosPos.x - CurrentPos.x); 
                MySSC.spline.SetLeftTangent(i,new Vector2(CurrentPos.x + Offset.x,CurrentPos.y + Offset.y) );
                MySSC.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                MySSC.spline.SetRightTangent(i,new Vector2(CurrentPos.x - Offset.x,CurrentPos.y - Offset.y) );
            }    */   
        }
    }   

}
