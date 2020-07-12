using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    private CinemachineVirtualCamera camera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float timerDeadlineS;
    private float elapsedTimeS;
    private float startIntensity;

    private void Awake()
    {
      Instance = this;
      camera = GetComponent<CinemachineVirtualCamera>();
      noise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
      timerDeadlineS = 0.0f;
      elapsedTimeS = 0.0f;
      startIntensity = 0.0f;
    }

    public void ShakeCamera(float intensity, float timeS)
    {
      
      noise.m_AmplitudeGain = intensity;
      startIntensity = intensity;
      timerDeadlineS = timeS;
      elapsedTimeS = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
      if (timerDeadlineS > 0.0f)
      {
        elapsedTimeS += Time.deltaTime;
        noise.m_AmplitudeGain = Mathf.Lerp(
            startIntensity, 0.0f, Mathf.Clamp(elapsedTimeS / timerDeadlineS, 0.0f, 1.0f));
        // Timer reached.
        if (elapsedTimeS >= timerDeadlineS)
        {
          // Reset the timer values to disable the noise update.
          elapsedTimeS = 0.0f;
          timerDeadlineS = 0.0f;
          startIntensity = 0.0f;
        }
      }
    }
}
