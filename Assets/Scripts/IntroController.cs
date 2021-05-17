using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public float IntroDur;
    private float TranscurredTime;
    public GameObject MainTitle;
    public AudioSource MainTitleSFX;
    void Start()
    {
        TranscurredTime = IntroDur;

    }

    // Update is called once per frame
    void Update()
    {
        TranscurredTime -= Time.deltaTime;
        if(TranscurredTime <= 0){
            MainTitle.SetActive(true);
            MainTitleSFX.Play();
            Invoke("LoadFirstScene",3);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            LoadFirstScene();
        }
    }
    private void LoadFirstScene(){
        SceneManager.LoadScene(1);
    }
}
