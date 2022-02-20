using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShakeScript : MonoBehaviour
{
    public static CameraShakeScript shake;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTime;
    private float startingIntensity;
    private float totalShakeTime;

    private Vector3 ogPosition;

    private void Awake()
    {
        if (shake == null)
            shake = this;

        ogPosition = this.transform.position;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = 0f;
    }

    public void shakeCamera(float intensity, float timer)
    {
        perlin.m_AmplitudeGain = intensity;
        startingIntensity = intensity;
        totalShakeTime = timer;
        shakeTime = timer;
    }

    private void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            if (shakeTime <= 0f)
            {
                perlin.m_AmplitudeGain = 0f;
                Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTime / totalShakeTime));
            }
        }
    }
}