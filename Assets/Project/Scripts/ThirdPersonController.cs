using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ThirdPersonController : MonoBehaviour
{
    public PlayerData playerData;
    private Animator playerAnimator;
    public CinemachineFreeLook camara;
    public GameObject machete;
    public GameObject sangreFx;

    public GameObject mainPausaPanel;
    public GameObject mainPoderPanel;

    public UnityEvent startIntroBossEvent;
    public UnityEvent endIntroBossEvent;
    public UnityEvent recogerItemEvent;

    private GameObject item;
    private bool flagDeath;
    private bool flagReinicio;

    private float timeMachete;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetBool("rapido", false);
        timeMachete = 0f;
        machete.SetActive(false);
        flagDeath = false;
        flagReinicio = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        // Recibir Daño
        if (other.gameObject.tag == "HitEnemy" && flagDeath == false)
        {
            Destroy(Instantiate(sangreFx, transform.position, Quaternion.identity), 1f);
            SoundFxManager.Instance.NohekDamage();
            
            Transform golpeTransform = other.GetComponent<Transform>();
            GameObject enemy;
            if (golpeTransform != null && golpeTransform.parent.parent != null)
            {
                enemy = golpeTransform.parent.parent.gameObject;
                string enemyTag = enemy.tag;

                
                if (enemyTag == "Coyote")
                {
                    if (playerData.vida - 16f < 0)
                    {
                        playerData.vida = 0f;
                        flagDeath = true;
                        playerAnimator.SetTrigger("dead");
                    }
                    else
                    {
                        playerData.vida -= 16f;
                        playerAnimator.SetTrigger("damage");
                    }
                }
            }
        }

        // Ahuizotl
        if (other.gameObject.tag == "HitAhuizotl")
        {
            Destroy(Instantiate(sangreFx, transform.position, Quaternion.identity), 1f);
            SoundFxManager.Instance.NohekDamage();
            Vector3 forceDirection = transform.up + other.transform.forward;
            forceDirection.Normalize();
            transform.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * 5f, ForceMode.Impulse);
            if (playerData.vida - 20f < 0)
            {
                playerData.vida = 0f;
                flagDeath = true;
                playerAnimator.SetTrigger("dead");
            }
            else
            {
                playerAnimator.SetTrigger("damage");
                playerData.vida -= 20f;
            }
        }

        // Recoger Item
        if (other.gameObject.tag == "Item")
        {
            item = other.gameObject;
            item.GetComponentInChildren<Canvas>().enabled = true;
        }

        if(other.gameObject.tag == "Agua")
        {
            SoundFxManager.Instance.NohekDamage();
            playerData.vida = 0f;
            flagDeath = true;
            playerAnimator.SetTrigger("dead");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == item.tag)
        {
            item.GetComponentInChildren<Canvas>().enabled = false;
            item =null;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "Tornado")
        {
            if (playerData.vida > 0)
                SoundFxManager.Instance.NohekDamage();

            if (playerData.vida - 1f < 0)
            {
                playerData.vida = 0f;
                flagDeath = true;
                playerAnimator.SetTrigger("dead");
            }
            else
            {
                playerAnimator.SetTrigger("damage");
                playerData.vida -= 1f;
            }
        }
    }

    private void ReiniciarLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Cuando esta en Play puede hacer todas las acciones
        if(mainPausaPanel.activeSelf == false)
        {
            // Solo cuando no este activo el menu de Poderes podra golpear
            if(mainPoderPanel.activeSelf == false)
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
                    recogerItemEvent.Invoke();
                    ItemDestroy();
                }
            }

            if(flagDeath==true && flagReinicio==false)
            {
                flagReinicio = true;
                Invoke("ReiniciarLevel", 8f);
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
