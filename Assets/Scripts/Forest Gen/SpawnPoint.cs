using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public GameObject[] objectsToSpawn;
    private LayerMask ToFind;
    private void Start()
    {
        int rand = Random.Range(0, objectsToSpawn.Length);
        GameObject instance = Instantiate(objectsToSpawn[rand], transform.position, Quaternion.identity,transform);
        if(gameObject.tag == "Props")
            Invoke("CheckIf",5);
        
        //instance.transform.parent = transform;
    }
    void CheckIf(){
        ToFind =  GameObject.FindGameObjectWithTag("Road").layer;
        if(Physics2D.OverlapCircle(transform.localPosition,2,ToFind)){
            
            Debug.Log("PLPLPLPLPL");
            Destroy(gameObject);
        } 
    }
}
