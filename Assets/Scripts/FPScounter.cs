using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPScounter : MonoBehaviour
{
    public float averageFps;
    public float currentFps;
    public float minFPS = 0;
    public float maxFPS = 0;
    public float totalFrames;
    public float totalduration;
    float currentDuration;
    float currentFrames;


    // Start is called before the first frame update
    void Start()
    {
        minFPS = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        totalFrames += 1;
        totalduration += frameDuration;
        currentDuration += frameDuration;
        currentFrames += 1;


        if (currentDuration >= 1.0f)
        {
            currentFps = currentFrames / currentDuration;
            currentFrames = 0;
            currentDuration = 0f;
        }

        if (currentFps > maxFPS)
            maxFPS = currentFps;

        if (currentFps < minFPS && currentFps >= 1)
           minFPS = currentFps;

        averageFps = totalFrames / totalduration;

    }
}
