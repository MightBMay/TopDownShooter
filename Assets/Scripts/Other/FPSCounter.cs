using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f;
    private float accum = 0;
    private int frames = 0;
    private float timeleft;

    void Start()
    {
        timeleft = updateInterval;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;
        if (timeleft <= 0.0)
        {
            float fps = accum / frames;
            string format = System.String.Format("{0:F2} FPS", fps);
            timeleft = updateInterval;
            Debug.Log(format);
            accum = 0.0F;
            frames = 0;
        }
        
    }
}

