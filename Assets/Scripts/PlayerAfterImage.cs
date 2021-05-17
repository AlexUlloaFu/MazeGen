using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImage : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer MySR;
    private SpriteRenderer PlayerSR;
    private Color color;
    public float ActiveTime = .1f;
    private float TimeActivated;
    private float alpha;
    public float aplhaSet = .8f;
    public float aplhaMultipler = .05f;

    private void OnEnable() {
        MySR = GetComponent<SpriteRenderer>();
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        PlayerSR = playerGO.GetComponent<SpriteRenderer>();
        player = playerGO.transform;
        alpha = aplhaSet;
        MySR.sprite = PlayerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        TimeActivated = Time.time;
    }
    private void Update() {
        alpha *= aplhaMultipler;
        color = new Color(1f,1f,1f,alpha);
        MySR.color = color;
        if(Time.time >= (TimeActivated + ActiveTime)){
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }

}
