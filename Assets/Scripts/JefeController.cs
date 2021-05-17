using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JefeController : MonoBehaviour
{
    Transform Player;
    HPController PlayerHP;
    HPController MyHp;
    Animator MyAnim;
    public int LifeToPhaseTwo;
    public int LifeToPhaseThree;
    public LayerMask ToCollide;
    public LayerMask ToImpact;
    public LineRenderer Positive;

    float LineaSpeed;
    public float MaxLineaSpeed;
    float LineaDistance;
    float CurrLineaDistance;
    float multi = .2f;
    float now;
    float second = 2f;
    public float AttackCooldown;
    public float AttackDur;
    float CurrAttackDur;
    float CurrAttackCooldown;
    Vector2 Dir;

    State CurrentState;

    public GameObject[] Enemies;
    public float SpawnEnemiesTime;
    float EnemiesSpawnTimer;
    PFgrid grid;

    public List<GameObject> Shields = new List<GameObject>();
    public GameObject ShieldGO;
    private Vector2 OffsetPoint = new Vector2(.2f,-.1f);
    void Start()
    {
        grid = GameObject.FindObjectOfType<PFgrid>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        MyAnim = GetComponent<Animator>();
        MyHp = GetComponent<HPController>();
        PlayerHP = Player.GetComponent<HPController>();
        now = Time.time;
        CurrAttackCooldown = AttackCooldown;
        Positive.SetPosition(0,transform.position);
        CurrentState = State.Fase1;
        StartCoroutine(CheckForLive());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 Dir = Player.position - transform.position;
        if(Dir.x < 0f){
            transform.localScale = new Vector3(1 , transform.localScale.y,transform.localScale.z);
            OffsetPoint.x = -.2f;
        }else if(Dir.x > 0f){
            transform.localScale = new Vector3(-1, transform.localScale.y,transform.localScale.z);
            OffsetPoint.x = .2f;
        }
        //ESTO ES SI QUIERO Q NO ATAQUE CUANDO NO ME VEA

        /*RaycastHit2D hit = Physics2D.Raycast(transform.position,Player.position - transform.position,1000,~gameObject.layer);
        if(hit.collider.gameObject.tag == "Player"){
            return;
        }*/

        AttackProcess();
        if(CurrentState == State.Fase2 || CurrentState == State.Fase3){
           SpawnEnemiesProcess();
        }
        if(CurrentState == State.Fase3){
            ShieldProcess();
        }
        
        
    }
    private void AttackProcess(){
        if(CurrAttackCooldown <= 0){
            MyAnim.SetBool("Attack",true);
            Positive.gameObject.SetActive(true);
            Positive.SetPosition(0,transform.position + (Vector3)OffsetPoint);
            if(CurrAttackDur <= 0){
                CurrAttackCooldown = AttackCooldown;
                CurrLineaDistance = 0;
                LineaSpeed = 0;
                multi = .2f;
                second = 2;
                MyAnim.SetBool("Attack",false);
                Positive.gameObject.SetActive(false);
                return;
            }
            if(AttackDur - CurrAttackDur < (float)(AttackDur/3)){
                Dir = (Player.position-transform.position).normalized;
                
            }
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + Dir,Dir * CurrLineaDistance,1000,ToCollide);
            RaycastHit2D DamageHit = Physics2D.Raycast((Vector2)transform.position + Dir,Dir * CurrLineaDistance,CurrLineaDistance,ToImpact);
            if(DamageHit.collider != null){
                if(DamageHit.collider.gameObject == Player.gameObject)
                    PlayerHP.TakeDamage(1);
            }
            Positive.SetPosition(1,Dir * CurrLineaDistance);
            LineaDistance = Vector2.Distance(transform.position, hit.point);
                if(CurrLineaDistance < LineaDistance){
                    CurrLineaDistance += Time.deltaTime * LineaSpeed;
                    LineaSpeed += Time.deltaTime * multi;
                    if(Time.time - now >= second / 1.5f){
                        second /= 2;
                        multi*= 2;
                        now = Time.time;
                    }
                    if(LineaSpeed >= MaxLineaSpeed){
                        LineaSpeed = MaxLineaSpeed;
                    }
                }
                else {
                    CurrLineaDistance = LineaDistance;
                    LineaSpeed = 0;
                    multi = 0;
                }
                Positive.SetPosition(1,(Vector2)transform.position + Dir * CurrLineaDistance);
            CurrAttackDur -= Time.deltaTime;
        }
        else{
            CurrAttackCooldown -= Time.deltaTime;
            CurrAttackDur = AttackDur;
        }
    }   

    private void SpawnEnemiesProcess(){
        EnemiesSpawnTimer -= Time.deltaTime;
        if(EnemiesSpawnTimer <= 0){
            EnemiesSpawnTimer = SpawnEnemiesTime;
            int IndexOfEnemy = Random.Range(0,Enemies.Length);
            PathFindNode SelectedNode;
            do{
            int XRandomPosIndex = Random.Range(0,grid.gridSizeX);
            int YRandomPosIndex = Random.Range(0,grid.gridSizeY);
            SelectedNode = grid.grid[XRandomPosIndex,YRandomPosIndex];
            }while(Physics2D.OverlapCircle(SelectedNode.WorldPos,.05f));
            Instantiate(Enemies[IndexOfEnemy],SelectedNode.WorldPos,Quaternion.identity,grid.transform);
        }
    }

    private void ShieldProcess(){
        if(Shields.Count > 0){
            for(int i = 0 ; i < Shields.Count; i++){
                if(Shields[i] != null){
                    GameObject shield = Shields[i];
                    shield.SetActive(true);
                }
                else {
                    Shields.RemoveAt(i);
                    return;
                }

            }
            ShieldGO.SetActive(true);
            MyHp.Inmune = true;
        }
        else {
            ShieldGO.SetActive(false);
            MyHp.Inmune = false;
        }
    }

    IEnumerator CheckForLive(){
        while(1 == 1){
            yield return new WaitForSeconds(1);
            float LifePercent = MyHp.CurrentHP * 100 / MyHp.HP;
            if(LifePercent <= LifeToPhaseThree ){
                CurrentState = State.Fase3;
            }
            else if(LifePercent <= LifeToPhaseTwo){
                CurrentState = State.Fase2;
            }
        }
    }
    enum State
    {
        Looking,
        Fase1,
        Fase2,
        Fase3
    }
}
