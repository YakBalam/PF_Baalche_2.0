using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLoopBoss : MonoBehaviour
{
    public AudioSource bossAudioSource;
    public AudioClip[] bossClips;

    public static SoundLoopBoss Instance;

    private AudioClip respiracion;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        respiracion = bossClips[0];
    }

    public void BossRespiraOn()
    {
        
        bossAudioSource.clip = respiracion;
        bossAudioSource.Play();

    }
    public void BossRespiraOff()
    {
        bossAudioSource.Stop();
    }
}
