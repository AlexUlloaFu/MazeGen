using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeGameplayController : MonoBehaviour
{
    public int TimeToCompleteMaze;
    public TextMeshProUGUI texto;
    public GameObject Onda;
    public Transform final;
    public GameObject MazeInstanciador;

    void Start()
    {
        texto.text = TimeToCompleteMaze.ToString();
        StartCoroutine("RegresiveCount");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            //comprobar si le quedan cargas
            Destroy(Instantiate(Onda,final.position,Quaternion.identity),13.3f);
            
        }
    }
    IEnumerator RegresiveCount(){
        while(TimeToCompleteMaze > 0){
        yield return new WaitForSeconds(1);
        TimeToCompleteMaze--;
        texto.text = TimeToCompleteMaze.ToString();
        }
        //Se acabo el tiempo y game over
    }
    /*public void GenerateMaze(){
        int altura = 0;
        int anchura = 0;
        int cont = 1;
        int[] Digits = alto.text.ToIntArray();
        for(int i = Digits.Length - 1 ; i > -1 ; i--){
            altura += Digits[i] * cont;
            cont *= 10;
        }
        cont = 1;
        Digits = ancho.text.ToIntArray();
        for(int i = Digits.Length - 1 ; i > -1 ; i--){
            anchura += Digits[i] * cont;
            cont *= 10;
        }
        altura -= 48;
        anchura -= 48;
        Debug.Log(altura);
        Debug.Log(anchura);
        MazeRenderer MyMR = Instantiate(MazeInstanciador,transform.position,Quaternion.identity).GetComponent<MazeRenderer>();
        MyMR.height = altura;
        MyMR.width = anchura;
    }*/
}
