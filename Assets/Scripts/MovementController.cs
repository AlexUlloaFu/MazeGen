using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Rigidbody2D MyRB;
    Animator MyAnim;
    Vector2 InputValues;
    public float MoveSpeed;
    private int MoveDir; // 1 LEFT 2 RIGHT 3 UP 4 DOWN
    
    public float dashSpeed;
    public float dashDuration;
    public float RemainingdashDuration;
    public float DashCooldown;
    private float RemaingTime;
    private bool ShouldMakeDash;
    private float DistBetwenImages = .1f;
    public GameObject DashVFXPrefab;
    public Animator VirtualCameraAnim;
    private GameObject CurrentDashVFX;
    private Vector2 lastPosition;
    private HPController MyHP;

    void Start()
    {
        MyRB = GetComponent<Rigidbody2D>();
        MyAnim = GetComponent<Animator>();
        MyHP = GetComponent<HPController>();
        RemaingTime = DashCooldown;
    }

    void Update()
    {
        InputValues.x = Input.GetAxisRaw("Horizontal");
        InputValues.y = Input.GetAxisRaw("Vertical");
        InputValues = InputValues.normalized;
        
        if(Input.GetKeyDown(KeyCode.Space) && RemaingTime <= 0){
            ShouldMakeDash = true;
            RemainingdashDuration = dashDuration;
            CinemachineShake.Instance.ShakeCamera(.1f,.5f);
            CurrentDashVFX = Instantiate(DashVFXPrefab,transform.position,Quaternion.identity,transform); 
            Destroy(CurrentDashVFX,dashDuration);
            VirtualCameraAnim.SetBool("ZoomIn",true);
            MyHP.SetInmune();
            
        }
        else 
            RemaingTime -= Time.deltaTime;

        
    }

    private void FixedUpdate() {
        if(ShouldMakeDash){
            if(RemainingdashDuration <= 0){
                RemaingTime = DashCooldown;
                ShouldMakeDash = false;
                VirtualCameraAnim.SetBool("ZoomIn",false);

            }
            else{
                MyRB.velocity = InputValues * dashSpeed;
                RemainingdashDuration -= Time.deltaTime;
                if(Vector2.Distance(transform.position,lastPosition) >= DistBetwenImages){
                    lastPosition = transform.position;
                    PlayerAfterImagePool.Instance.GetFromPool();
                }
            }
            
        }
        else{
            if(InputValues.x > 0) MoveDir = 2;
            else if(InputValues.x < 0) MoveDir = 1;
            else if(InputValues.y < 0) MoveDir = 4;
            else if(InputValues.y > 0) MoveDir = 3;
            else MoveDir = 0;
            MyAnim.SetInteger("MoveDir",MoveDir);
            MyRB.velocity = InputValues * MoveSpeed;
        }
    }
}
