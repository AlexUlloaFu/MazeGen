using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    Transform Player;
    Vector2 PlayerLastPos;
    Animator MyAnim;
    Collider2D MyCol;
    public Vector2 RelativePos;
    Rigidbody2D MyRB;
    public float MaxDistToChase;
    public float MaxDistToAttack;
    public float MaxDistToCharge;
    public float MoveSpeed;
    Vector2[] path;
    bool PathFindingActive;
    int targetIndex;
    public GameObject MeleeDamagePointGO;
    public GameObject Projectile;
    public float AttackCooldown;
    private float CurrCooldown;
    private Vector2 Dir;
    private bool Atacking;
    private HPController PlayerHP;
    public bool IsMelee;
    public bool DirectChase;
    public bool CanCharge;
    public bool IgnoreDir;
    public PFgrid grid;
    PathFindNode CurrentNode;
    HideMe MyHM;
    // Start is called before the first frame update
    private void Awake() {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        grid = GameObject.FindObjectOfType<PFgrid>();
        MyCol = GetComponent<Collider2D>();
        MyHM = GetComponent<HideMe>();
        
    }
    void Start()
    {
        MyAnim = GetComponent<Animator>();
        PlayerHP = Player.GetComponent<HPController>();
        MyRB = GetComponent<Rigidbody2D>();
        CurrCooldown = AttackCooldown;
    }

    void Update()
    {
        float CurrentDist = Vector2.Distance(transform.position,Player.position);

        if(CurrentDist <= MaxDistToChase ){
            if(CurrentDist <= MaxDistToAttack || Atacking){
                MyRB.velocity = Vector2.zero;
                MyAnim.SetBool("Move",false);

                StopCoroutine("FollowPath");
                CanCharge = false;
                PathFindingActive = false;

                if(CurrCooldown <=0){
                    if(Player.GetComponent<HPController>().Inmune == true) return;
                    
                    Attack();
                    MyAnim.SetBool("Move",false);
                    MyAnim.SetBool("Attack",true);
                }
                else{
                    if(!Atacking)
                        MyAnim.SetBool("Attack",false);
                } 
            }
            else if(CurrentDist <= MaxDistToCharge && IsMelee && CanCharge){
                StopCoroutine("FollowPath");

                if(Player.GetComponent<HPController>().Inmune == true) return;
                MyRB.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                transform.position = transform.position + (Player.position - transform.position) * 1.3f;
                CanCharge = false;
            }
            else{
                CanCharge = true;
                if(IsMelee)
                    if(CurrentDist <= MaxDistToCharge) return;
                //FindPath;
                if(!PathFindingActive){
                    PathFindingActive = true;
                    if(grid.grid != null)
                    PathRequestManager.RequestPath(transform.localPosition,Player.localPosition,OnPathFound);
                }
                else if(PlayerLastPos != (Vector2)Player.position) {
                    StopCoroutine(FollowPath());
                    PlayerLastPos = Player.position;
                    if(grid.grid != null)
                    PathRequestManager.RequestPath(transform.localPosition,Player.localPosition,OnPathFound);
                }
            }  //Chase();
            //}
        }
        else{
            //Patroll
            MyRB.velocity = Vector2.zero;
            MyAnim.SetBool("Move",false);
            MyAnim.SetBool("Attack",false);
        }
        CurrCooldown -= Time.deltaTime;
    }
    private void Attack(){
        MyRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        MyRB.velocity = Vector2.zero;
        Atacking = true;
        CurrCooldown = AttackCooldown;

        //Instanciar punto de danno en la direccion correcta
        Invoke("CreateDamagePoint",.2f);
    }
    public void OnPathFound(Vector2[] newPath, bool PathSuccesfull ){
        if(PathSuccesfull){
            StopCoroutine("FollowPath");
            path = newPath;
            StartCoroutine("FollowPath");
        }
        else DirectChase = true;
    }
    IEnumerator FollowPath(){
        Vector2 currentWaypoint;
        if(path.Length > 0)
        currentWaypoint =  path[0]; 
        else  currentWaypoint = transform.position;
        targetIndex = 0;
        while(true){
           /* RelativePos = MyCol.ClosestPoint(transform.position);
            RelativePos = grid.transform.InverseTransformPoint(RelativePos);
            PathFindNode node =  grid.GetNodeFromWorldPos( RelativePos);
            //grid.grid[node.GridPosX,node.GridPosY] = node;
            node.Walkable = false;
            if(node != CurrentNode){
                if(CurrentNode != null)
                    CurrentNode.Walkable = true;
                CurrentNode = node;
            }*/
            if(Vector2.Distance(transform.position,currentWaypoint) < .2f){
                if(path.Length > 0)
                    currentWaypoint = path[targetIndex];
                Dir = currentWaypoint - (Vector2)transform.position;
                targetIndex++;
                if(targetIndex >= path.Length){
                    PathFindingActive = false;
                    MyRB.velocity = ((Vector2)Player.position - (Vector2)transform.position).normalized * MoveSpeed;
                    yield break;
                }
            }
            //transform.position = Vector2.MoveTowards(transform.position,currentWaypoint,MoveSpeed* Time.deltaTime);
            MyRB.velocity = (currentWaypoint - (Vector2)transform.position).normalized * MoveSpeed;
            MyAnim.SetBool("Move",true);
            MyAnim.SetBool("Attack",false);
            yield return null;
        }
    }
    private void Chase(){
        if(!IgnoreDir) SetDirToPlayer();
        if(Dir.x < 0f){
                transform.localScale = new Vector3(-1 , transform.localScale.y,transform.localScale.z);
        }else if(Dir.x > 0f){
            transform.localScale = new Vector3(1, transform.localScale.y,transform.localScale.z);
        }
        MyRB.velocity = Dir * MoveSpeed;
        MyAnim.SetBool("Move",true);
        MyAnim.SetBool("Attack",false);
    }
    private void SetDirToPlayer(){
        Dir = (Player.position - transform.position).normalized;
        IgnoreDir = false;
    }

    private void CreateDamagePoint(){
        if(IsMelee){
            Vector2 Direction = Player.position - transform.position;
            GameObject created =  GameObject.Instantiate(MeleeDamagePointGO,((Vector2)transform.position + Direction),Quaternion.identity);
            created.GetComponent<DamagePoint>().IAmAnEnemy = true;
            Destroy(created,.5f);
        }
        else{
            GameObject created =  GameObject.Instantiate(Projectile,((Vector2)transform.position + Dir*.2f),Quaternion.identity);
            created.GetComponent<Projectile>().ImAEnemy = true;
            Vector2 Direction = Player.position - transform.position;
            float angle = Mathf.Atan2(Direction.y,Direction.x) * Mathf.Rad2Deg;
            created.transform.eulerAngles = new Vector3(created.transform.eulerAngles.x,created.transform.eulerAngles.y,angle);
        }
        Atacking = false;

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Player"){
            
            if(Player.GetComponent<MovementController>().RemainingdashDuration > 0) return;
            if(Player.GetComponent<HPController>().Inmune) return;
            Vector2 Dir = (transform.position - Player.position).normalized;
            Player.position -= (Vector3)Dir * .3f;
            //transform.position += (Vector3)Dir * .3f;
            PlayerHP.TakeDamage(1);
            PlayerHP.SetInmune();
        }
    }
    private void OnDrawGizmos() {
        if(path != null){
            for(int i = 0 ; i < path.Length; i++){
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i],Vector3.one * .25f);
            }
        }
    }
}
