using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundShootBoss : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioClip[] sFXClips;

    public static SoundShootBoss Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Attack()
    {
        sfxAudioSource.PlayOneShot(sFXClips[0]);
    }
    public void Damage()
    {
        sfxAudioSource.PlayOneShot(sFXClips[1]);
    }
    public void Death()
    {
        sfxAudioSource.PlayOneShot(sFXClips[2]);
    }
}
