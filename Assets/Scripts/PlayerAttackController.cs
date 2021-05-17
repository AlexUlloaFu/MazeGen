using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    
    public GameObject Bullet;
    public GameObject Light;
    public float Cooldown;
    private float CurrentCooldown;
    void Start()
    {
        CurrentCooldown = Cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ShootDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        float angle = Mathf.Atan2(ShootDir.y,ShootDir.x) * Mathf.Rad2Deg;
        Light.transform.eulerAngles = new Vector3(Light.transform.eulerAngles.x,Light.transform.eulerAngles.y,angle - 90);
        if((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButton("Fire1")) && CurrentCooldown <= 0){
            CurrentCooldown = Cooldown;
            GameObject created =  GameObject.Instantiate(Bullet,((Vector2)transform.position + ShootDir*.2f),Quaternion.identity);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(),created.GetComponent<Collider2D>()) ;
            created.transform.eulerAngles = new Vector3(created.transform.eulerAngles.x,created.transform.eulerAngles.y,angle);
        }
        CurrentCooldown -= Time.deltaTime;
    }
    private void FixedUpdate() {
        Light.transform.position = (Vector2)transform.position + new Vector2(.2f,.2f);
    }
}
