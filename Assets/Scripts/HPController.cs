using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HPController : MonoBehaviour
{
    public float HP;
    public float CurrentHP;
    //[HideInInspector]
    public bool Inmune = false;
    private Animator MyAnim;
    private SpriteRenderer MySR;
    private Material defaultMat;
    public Material whiteMat;
    public TextMeshProUGUI texto;
    public GameObject Barra;
    public RectTransform BarraUI;
    public float EmpujeForce;
    void Awake()
    {
        CurrentHP = HP;
        MyAnim = GetComponent<Animator>();
        MySR = GetComponent<SpriteRenderer>();
        defaultMat = MySR.material;
        if(gameObject.tag == "Player")
            texto.text = HP.ToString();
    }

    public void TakeDamage(int amount){
        if(Inmune) return;
        if(gameObject.tag == "Player"){
            SetInmune();
            CinemachineShake.Instance.ShakeCamera(1,.5f);
        }
        MySR.material = whiteMat;
        Invoke("ResetMat",.1f);
        CurrentHP -= amount;
        if(CurrentHP <= 0){
            CurrentHP = 0;
            if(gameObject.tag == "Player")
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            else {
                Destroy(gameObject);
                //Poner efecto de muerte
            }
        }
        if(gameObject.tag == "Player")
            texto.text = CurrentHP.ToString();
        else if(gameObject.tag == "Boss"){
            BarraUI.localScale = new Vector3((float)(3f/HP) * CurrentHP,BarraUI.localScale.y,BarraUI.localScale.z);
        }
        else if(Barra != null) {

            Debug.Log((1f/HP) * CurrentHP);
            Barra.transform.localScale = new Vector3((float)(1f/HP) * CurrentHP,Barra.transform.localScale.y,Barra.transform.localScale.z);
        }
    }

    public void SetInmune(){
        Inmune = true;
        Invoke("ResetInmune",1f);
    }
    private void ResetInmune(){
        Inmune = false;
    }
    private void ResetMat(){
        MySR.material = defaultMat;
    }
}
