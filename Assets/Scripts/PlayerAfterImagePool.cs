using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    public GameObject afterImagePrefab;
    private Queue<GameObject> AviableObjects = new Queue<GameObject>();
    public static PlayerAfterImagePool Instance {get; private set;}

    private void Awake() {
        Instance = this; 
        GrowPool();
    }

    private void GrowPool(){
        for(int i = 0 ; i < 10 ; i++){
            var InstanceToAdd = Instantiate(afterImagePrefab);
            InstanceToAdd.transform.SetParent(transform);
            AddToPool(InstanceToAdd);
        }
    }

    public void AddToPool(GameObject CurrInstance){
        CurrInstance.SetActive(false);
        AviableObjects.Enqueue(CurrInstance);
    }

    public GameObject GetFromPool(){
        if(AviableObjects.Count == 0 ){
            GrowPool();
        }
        var instance = AviableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
