using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMe : MonoBehaviour
{
    public GameObject[] Childs;
    public SpriteRenderer MySR;
    public Transform Player;
    public bool BeingFocus;
    void OnEnable()
    {
        Hide();
    }

    private void Awake() {
        MySR = GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }
    public void Hide(){
        MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,0);
        if(Childs.Length == 0) return;
        foreach(GameObject child in Childs)
            child.SetActive(false);
    }
    public void Show(){
        MySR.color = new Color(MySR.color.r,MySR.color.g,MySR.color.b,1);
        if(Childs.Length == 0) return;
        foreach(GameObject child in Childs)
            child.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!BeingFocus){
            if(Vector2.Distance(transform.position,Player.position) < 1)
                Show();
            else Hide();
        }
        
    }
}
