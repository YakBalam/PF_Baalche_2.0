using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemyRun : MonoBehaviour
{
    public AudioSource runAudioSource;
    public AudioClip[] runClips;

    public static SoundEnemyRun Instance;

    private AudioClip run;
    private AudioClip runFast;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        run = runClips[0];
        runFast = runClips[1];
    }

    public void EnemyRunOn(bool rapido)
    {
        if (rapido == false)
        {
            runAudioSource.clip = run;
        }
        else
        {
            runAudioSource.clip = runFast;
        }


        runAudioSource.Play();

    }
    public void EnemyRunOff()
    {
        runAudioSource.Stop();
    }
}
