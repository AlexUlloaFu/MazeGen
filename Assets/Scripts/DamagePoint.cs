using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePoint : MonoBehaviour
{   
    private Animator MyAnim;
    public LayerMask Chocable;
    public int Damage = 1;
    public float Time = 1;
    public float Force = 1;
    private HPController HP;
    public float DamageOffset;
    public bool IAmAnEnemy;

    private Vector2 TargetPos;
    private void Awake() {
        Chocable.value = 10;
        MyAnim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == null) return;

        TargetPos = other.transform.position;

        if(IAmAnEnemy){ 
            if(other.gameObject.tag != "Player") return;
            HP = other.gameObject.GetComponent<HPController>();

            if(HP.Inmune != true)
                InflictDamage();
                Debug.Log("LOL");
        }
        else{
            if(other.gameObject.tag != "Enemy") return;
            HP = other.gameObject.GetComponent<HPController>();
            InflictDamage();

        } 

            if(MyAnim != null)
                MyAnim.SetBool("Activate",true);
            
            if(MyAnim != null)
                Invoke("SavePikes",Time);
        
    }

    void InflictDamage(){
        HP.TakeDamage(Damage);
        Vector2 Dir = (TargetPos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Dir,Chocable);
        if(hit.collider != null){
            HP.transform.position = hit.point - Dir * .1f;
        }
        else HP.transform.position -= (transform.position - HP.transform.position).normalized * Force; 
    }
    void SavePikes(){
        MyAnim.SetBool("Activate",false);
    }
}
