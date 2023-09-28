using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsRun : MonoBehaviour
{
    public AudioSource runAudioSource;
    public AudioClip[] runClips;

    public static SoundsRun Instance;
    private string escenaActiva;

    private AudioClip run;
    private AudioClip runFast;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        escenaActiva = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        switch (escenaActiva)
        {
            case "Templo":
                run = runClips[0];
                runFast = runClips[1];
                break;
            case "Nivel 01":
                run = runClips[2];
                runFast = runClips[3];
                break;
        }
    }

    public void NohekRunOn(bool rapido)
    {
        if(rapido == false)
        {
            runAudioSource.clip = run;
        }
        else
        {
            runAudioSource.clip = runFast;
        }        

        
        runAudioSource.Play();
        
    }
    public void NohekRunOff()
    {
        runAudioSource.Stop();
    }

}
