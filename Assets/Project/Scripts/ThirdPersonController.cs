using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    public PlayerData playerData;
    private Animator playerAnimator;
    public CinemachineFreeLook camara;
    public GameObject machete;

    private GameObject item;
    private bool flagDeath;

    private float timeMachete;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetBool("rapido", false);
        timeMachete = 0f;
        machete.SetActive(false);
        flagDeath = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Recibir Daño
        if (other.gameObject.tag == "HitEnemy" && flagDeath == false)
        {
            playerAnimator.SetTrigger("damage");
            Debug.Log("Flag_1");
            Transform golpeTransform = other.GetComponent<Transform>();
            GameObject enemy;
            if (golpeTransform != null && golpeTransform.parent.parent != null)
            {
                Debug.Log("Flag_2");
                enemy = golpeTransform.parent.parent.gameObject;
                string enemyTag = enemy.tag;

                // Solo se recibira daño si el atacante es el jugador
                if (enemyTag == "Coyote")
                {
                    Debug.Log("Flag_3");
                    if (playerData.vida - 16f < 0)
                    {
                        playerData.vida = 0f;
                        flagDeath = true;
                        playerAnimator.SetBool("death", true);
                    }
                    else
                    {
                        playerData.vida -= 16f;
                    }
                }
            }

        }

        // Recoger Item
        if (other.gameObject.tag == "Item")
        {
            item = other.gameObject;
            item.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == item.transform)
        {
            item.GetComponentInChildren<Canvas>().enabled = false;
            item =null;
        }
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
                    machete.tag = "MacheteLigero";
                    timeMachete = 15f;
                    if (!machete.activeSelf)
                        machete.SetActive(true);
                    playerAnimator.SetTrigger("golpeLigero");
                }

                // Golpe pesado
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    machete.tag = "MachetePesado";
                    timeMachete = 15f;
                    if (!machete.activeSelf)
                        machete.SetActive(true);
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

            // Recoger item
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(item != null && Vector3.Distance(transform.position,item.transform.position) <= 1.5f)
                {
                    playerAnimator.SetTrigger("item");
                    ItemDestroy();
                    item =null;
                }
            }
        }

        // Si pasan 15 segundos sin atacar con el machete, este desaparecera
        if (timeMachete > 0)
            timeMachete-= Time.deltaTime;
        else if (machete.activeSelf)
            machete.SetActive(false);


    }

    public void EnableColliderMachete()
    {
        machete.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void DisableColliderMachete()
    {
        machete.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void ItemDestroy()
    {
        Destroy(item, 2.5f);
        item = null;
    }
}
