using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroBigBoss : MonoBehaviour
{
    public PlayableDirector timeline;
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            timeline.Play();
            animator.enabled = true;
        }
    }
}
