using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroBigBoss : MonoBehaviour
{
    public PlayableDirector timeline;
    public Animator animator;
    public GameObject barrera;
    private float timeIntro = 11.5f;
    private float timeBarrera = 2f;
    private bool intro = false;
    private ThirdPersonController playerController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController = other.GetComponent<ThirdPersonController>();
            if (playerController != null)
            {
                playerController.startIntroBossEvent.Invoke();
                intro = true;
            }
            timeline.Play();
            animator.enabled = true;
        }
    }

    private void Update()
    {
        if (intro)
        {
            timeIntro -= Time.deltaTime;
            timeBarrera -= Time.deltaTime;
            if(timeBarrera <= 0)
            {
                barrera.SetActive(true);
            }
            if(timeIntro <=0) 
            { 
                playerController.endIntroBossEvent.Invoke();
                intro= false;
                Destroy(gameObject);
            }
        }
    }
}
