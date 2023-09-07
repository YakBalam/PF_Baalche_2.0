using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{

    private Animator playerAnimator;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetFloat("speed", Input.GetAxis("Vertical"));
        playerAnimator.SetFloat("direction", Input.GetAxis("Horizontal"));

    }
}
