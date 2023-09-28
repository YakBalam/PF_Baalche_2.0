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
        playerAnimator.SetBool("rapido", false);
    }

    // Update is called once per frame
    void Update()
    {
        // Cuando esta en Play puede hacer todas las acciones
        if(Time.timeScale>0)
        {
            // Solo cuando no este activo el menu de Poderes podra golpear
            if(Time.timeScale > .5f)
            {
                // Golpe ligero
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    playerAnimator.SetTrigger("golpeLigero");
                }

                // Golpe pesado
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    playerAnimator.SetTrigger("golpePesado");
                }
            }

            // Se activan los estados/aniamciones de correr
            playerAnimator.SetFloat("speed", Input.GetAxis("Vertical"));
            playerAnimator.SetFloat("direction", Input.GetAxis("Horizontal"));

            if (Input.GetKeyDown(KeyCode.S))
            {
                playerAnimator.SetTrigger("atras");
            }

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

            // Switch para correr rapido/lento
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                playerAnimator.SetBool("rapido", !playerAnimator.GetBool("rapido"));
            }



            // Esquivar
            //if(Input.GetKeyDown(KeyCode.A))
            //{
            //    playerAnimator.SetFloat("directionEsquivar", -1f);
            //}
            //else if (Input.GetKeyDown(KeyCode.D))
            //{
            //    playerAnimator.SetFloat("directionEsquivar", 1f);
            //}
            //else
            //{
            //    playerAnimator.SetFloat("directionEsquivar", 0f);
            //}

            // Esquivar/saltar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Control para cuando esquiva estando inmovil o corriendo a velocidad baja
                if (playerAnimator.GetFloat("direction") < 0f)
                {
                    playerAnimator.SetFloat("directionEsquivar", -1f);
                }
                else if (playerAnimator.GetFloat("direction") > 0f)
                {
                    playerAnimator.SetFloat("directionEsquivar", 1f);
                }
                else
                {
                    playerAnimator.SetFloat("directionEsquivar", 0f);
                }

                // Saltar/esquivar
                playerAnimator.SetTrigger("saltar");
            }
        }
        


    }
}
