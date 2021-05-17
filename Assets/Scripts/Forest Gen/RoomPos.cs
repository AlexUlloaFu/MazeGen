using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPos : MonoBehaviour {

    public float waitTime;
    public LayerMask whatIsRoom;
    bool hasRoom;

    public GameObject closedRoom;
    public GameObject VerticalBorder;
    public GameObject HorizontalBorder;
    public int CellsInX;
    public int CellsInY;

	void Update () {

        Collider2D room = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom);
        if (room != null) {
            hasRoom = true;
        }

        if (waitTime <= 0)
        {
            if (hasRoom == false) {
                Instantiate(closedRoom, transform.position, Quaternion.identity);
                Quaternion alfonso = Quaternion.Euler(0,0,-90);

                if(transform.position.x == (CellsInX-1) * 10)
                    Instantiate(VerticalBorder,new Vector3(transform.position.x + 6,transform.position.y),Quaternion.identity);
                if(transform.position.y == (CellsInY-1) * -10){
                    Instantiate(HorizontalBorder,new Vector3(transform.position.x,transform.position.y - 6),alfonso);
                }
                if(transform.position.y == 0) {
                    Instantiate(HorizontalBorder,new Vector3(transform.position.x,transform.position.y + 6),alfonso);
                }
                if(transform.position.x == 0){
                    Instantiate(VerticalBorder,new Vector3(transform.position.x - 6,transform.position.y),Quaternion.identity);
                }
                GameObject.FindGameObjectWithTag("LVL Generator").GetComponent<LevelGeneration>().SpawnEnemies(transform.position);
                Destroy(this);
            }
        }
        else {
            waitTime -= Time.deltaTime;
        }
	}
}
