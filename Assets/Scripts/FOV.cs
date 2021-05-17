using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{   
    private void Start() {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(),GetComponent<Collider2D>());
    }
    public LayerMask ToIgnore;
    public List<HideMe> ToLook =  new List<HideMe>();
    private void OnTriggerEnter2D(Collider2D other) {
        HideMe Candidate = other.gameObject.GetComponent<HideMe>();
        if(Candidate != null){
            ToLook.Add(Candidate);
            Candidate.BeingFocus = true;

        }
    }
    private void Update() {
        for(int i = 0 ; i < ToLook.Count; i++){
            if(ToLook[i] == null){
                ToLook.Remove(ToLook[i]);
                continue;
            } 
            HideMe Current = ToLook[i];
            RaycastHit2D hit;
            Current.gameObject.layer = 9;
            Vector2 Dir = (Current.GetComponent<Collider2D>().ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
            if(hit = Physics2D.Raycast(transform.position,Dir,100,~ToIgnore)){
                Debug.DrawRay(transform.position,hit.point-(Vector2)transform.position);
                if(hit.collider.gameObject != Current.gameObject){
                    if(hit.collider.GetComponent<HideMe>() != null){
                        hit.collider.gameObject.layer = 0;
                        i--;
                        continue;
                    }
                    Current.Hide();
                }
                else{
                    Current.Show();
                    Current.gameObject.layer = 0;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        HideMe TargetHM = other.GetComponent<HideMe>();
        if(TargetHM == null) return;
        if(TargetHM.isActiveAndEnabled){
            TargetHM.Hide();
            ToLook.Remove(TargetHM);
            TargetHM.BeingFocus = false;
        }
    }
}
