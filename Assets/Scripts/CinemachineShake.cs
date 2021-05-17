using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance {get; private set;}
    private CinemachineVirtualCamera MyVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float stratingIntensity;
    private CinemachineBasicMultiChannelPerlin MineCBMCP;

    private void Awake()
    {
        Instance = this;
        MyVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        MineCBMCP = MyVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        MineCBMCP.m_AmplitudeGain = 0;
    }

    public void ShakeCamera(float intensity , float time){
        MineCBMCP.m_AmplitudeGain = intensity;
        stratingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0){
            shakeTimer -= Time.deltaTime;
            MineCBMCP.m_AmplitudeGain = Mathf.Lerp(stratingIntensity,0, 1-(shakeTimer/shakeTimerTotal));
        }
    }
}
