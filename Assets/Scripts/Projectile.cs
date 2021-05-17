using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D MyRB;
    Collider2D MyCol;
    [SerializeField]
    float MoveSpeed;
    public float ExsitanceDur;
    public bool ImAEnemy;
    public int Damage;
    private Vector2 TargetPos;
    private bool ShouldMove;
    HPController HP;
    public LayerMask Chocable;
    void Start()
    {
        MyRB = GetComponent<Rigidbody2D>();
        MyCol = GetComponent<Collider2D>();
        ShouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        ExsitanceDur-= Time.deltaTime;
        if(ExsitanceDur <= 0){
            Destroy(gameObject);
        }
    }
    private void FixedUpdate() {
        if(ShouldMove)
            MyRB.velocity = transform.right * MoveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == null) return;
        if(other.gameObject.layer == 10){

        }
        else if(ImAEnemy){
            if(other.gameObject.tag != "Player") return;

        }
        else if(other.gameObject.tag != "Enemy" && other.gameObject.tag != "Boss") return;
        Debug.Log("pimbata");
        TargetPos = other.transform.position;
        Destroy(MyRB);
        Destroy(MyCol);
        HP = other.gameObject.GetComponent<HPController>();
        if(other.gameObject.layer != 10)
            InflictDamage();
        ShouldMove = false;
        GetComponent<Animator>().SetBool("Destroy",true);
        ParticleSystem MyPS = GetComponentInChildren<ParticleSystem>();
        if(MyPS != null)
            Destroy(MyPS.gameObject);
        Destroy(gameObject,.6f);
    }
    void InflictDamage(){
        HP.TakeDamage(Damage);
        if(HP.EmpujeForce == 0) return;
        Vector2 Dir = ((Vector2)transform.position - TargetPos).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Dir,Chocable);
        if(hit.collider != null){
            HP.transform.position = hit.point - Dir * .1f;
        }
        else HP.transform.position += (transform.position - HP.transform.position).normalized * HP.EmpujeForce; 
    }
}
