using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public PlaneController controller;
    public AudioSource m_MyAudioSource;
    public bool soundPlaying = false;
    public bool playSound = false;

    // Start is called before the first frame update
    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        m_MyAudioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.engineRPM > 0)
        {
            playSound = true;
        }
        else
        {
            playSound = false;
        }



        if (playSound == true && soundPlaying == false)
        {
            m_MyAudioSource.Play();
            soundPlaying = true;
        }
        else if (playSound == false && soundPlaying == true)
        {
           m_MyAudioSource.Stop();
            soundPlaying = false;
        }
    }
}
