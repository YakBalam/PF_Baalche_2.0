using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioClip[] sFXClips;

    public static SoundFxManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Nohek
    public void NohekAttackLigero()
    {
        sfxAudioSource.PlayOneShot(sFXClips[0]);
    }

    public void NohekAttackPesado()
    {
        sfxAudioSource.PlayOneShot(sFXClips[1]);
    }
    public void NohekAttackLigeroFast()
    {
        sfxAudioSource.PlayOneShot(sFXClips[2]);
    }

    public void NohekAttackPesadoFast()
    {
        sfxAudioSource.PlayOneShot(sFXClips[3]);
    }
    public void NohekEsquivar()
    {
        sfxAudioSource.PlayOneShot(sFXClips[4]);
    }
    public void NohekJump()
    {
        sfxAudioSource.PlayOneShot(sFXClips[5]);
    }

    // Menu Poder
    public void ShowMenuPower()
    {
        sfxAudioSource.PlayOneShot(sFXClips[6]);
    }

    // Menu Pausa
    public void ShowMenuPausa()
    {
        sfxAudioSource.PlayOneShot(sFXClips[7]);
    }
}
