using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{

    private Animator playerAnimator;
    public CinemachineFreeLook camara;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Se activan los estados/aniamciones de correr
        playerAnimator.SetFloat("speed", Input.GetAxis("Vertical"));
        playerAnimator.SetFloat("direction", Input.GetAxis("Horizontal"));

        // Desactivacion del movimiento de camara con mouse
        // (se desactiva cuando esta activa la rueda de poderes)
        if (Input.GetKey(KeyCode.Q))
        {
            camara.m_XAxis.m_MaxSpeed = 0;
            camara.m_YAxis.m_MaxSpeed = 0;
        }
        else
        {
            // Se reactiva el movimiento de camara con mouse
            camara.m_XAxis.m_MaxSpeed = 200;
            camara.m_YAxis.m_MaxSpeed = 1;
        }

    }
}
