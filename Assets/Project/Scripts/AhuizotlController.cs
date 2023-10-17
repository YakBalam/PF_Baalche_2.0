using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class AhuizotlController : MonoBehaviour
{
    public Image barraVida;
    public GameObject pataFD;
    public GameObject manoFD;
    public GameObject tornado;

    private Animator bossController;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private AhuizotlState currentState;

    private float sqDistance2Player;
    private float distanceIdleToAttack;
    private float distanceChaseToAttack;
    private float distanceIdleToRun;
    private float distanceRunToAtack;


    private float timeIdle;
    private float timeChase;
    private float timeRun;
    private float timePower;
    private float timeAttack;    

    private float timeSeePlayer;
    private float timeOffsetPower;
    private bool seePlayerBool;
    private bool tornadoB;

    private bool powerB;
    private Vector3 destinoPowerV = new Vector3 (-160.50f, 10.80f, 66.29f);
    private bool attackBool;

    private float vidaMax = 500;
    private float vidaActual;
    private float damageMacheteL;
    private float damageMacheteP;

    void Start()
    {
        bossController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;

        distanceIdleToAttack = 13f;
        distanceChaseToAttack = 30f;
        distanceIdleToRun = 300f;
        distanceRunToAtack = 30f;

        vidaActual = vidaMax;
        barraVida.fillAmount = 1;
        damageMacheteL = 8f;
        damageMacheteP = 12f;

        currentState = AhuizotlState.NONE;
        ChangeState(AhuizotlState.IDLE);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MacheteLigero")
        {
            vidaActual -= damageMacheteL;
            barraVida.fillAmount -= damageMacheteL / vidaMax;
        }
        else if (other.gameObject.tag == "MachetePesado")
        {
            vidaActual -= damageMacheteL;
            barraVida.fillAmount -= damageMacheteP / vidaMax;
        }
    }
    void ChangeState(AhuizotlState newState)
    {
        if (currentState == newState) return;

        switch (newState)
        {
            case AhuizotlState.IDLE:
                if (currentState == AhuizotlState.CHASE)
                {
                    agent.SetDestination(transform.position);
                    StopCoroutine("UpdatePlayerDestination");
                }
                agent.speed = 5.5f;
                bossController.SetBool("run", false);
                timeIdle = Random.Range(4f, 5f);
                currentState = newState;
                break;
            case AhuizotlState.TWOATTACK:
                StopCoroutine("UpdatePlayerDestination");
                agent.SetDestination(transform.position + transform.forward * 5f);
                currentState = newState;
                break;
            case AhuizotlState.CHASE:
                timeChase = 5f;
                if (currentState == AhuizotlState.IDLE)
                {
                    StartCoroutine("UpdatePlayerDestination");
                }
                currentState = newState;
                break;
            case AhuizotlState.POWER:
                powerB = false;
                seePlayerBool = false;
                tornadoB = false;
                timePower = 6f;
                timeSeePlayer = 1f;
                timeOffsetPower = .8f;

                agent.SetDestination(destinoPowerV);
                currentState = newState;
                break;
            case AhuizotlState.RUN:
                timeRun = 6.5f;
                bossController.SetBool("run", true);
                agent.speed = 6f;
                StartCoroutine("UpdatePlayerDestination");
                currentState = newState;
                break;
            case AhuizotlState.ATTACK:
                StopCoroutine("UpdatePlayerDestination");
                agent.SetDestination(transform.position + transform.forward * 15f);
                attackBool = false;
                timeAttack = 1.5f;
                currentState = newState;
                break;
        }
    }

    public IEnumerator UpdatePlayerDestination()
    {
        while (true)
        {
            agent.SetDestination(playerTransform.position);
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public IEnumerator SeePlayer()
    {
        while (true)
        {
            Vector3 direccionJugador = new Vector3(playerTransform.position.x,
                                                   transform.position.y,
                                                   playerTransform.position.z) - transform.position;

            // Rota el enemigo hacia la dirección del jugador
            transform.rotation = Quaternion.LookRotation(direccionJugador);
            yield return null;
        }
    }

    public void ActiveTagAttack()
    {
        pataFD.tag = "HitAhuizotl";
        pataFD.GetComponent<CapsuleCollider>().isTrigger = true;
        manoFD.tag = "HitAhuizotl";
        manoFD.GetComponent<SphereCollider>().isTrigger = true;
    }

    public void DisableTagAttack()
    {
        pataFD.GetComponent<CapsuleCollider>().isTrigger = false;
        pataFD.tag = "Ahuizotl";
        manoFD.GetComponent<SphereCollider>().isTrigger = false;
        manoFD.tag = "Ahuizotl";
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case AhuizotlState.IDLE:
                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                Debug.Log("DistancaIdle: "+sqDistance2Player);
                timeIdle -= Time.fixedDeltaTime;
                if(timeIdle <= 0)
                {
                    if (sqDistance2Player <= distanceIdleToAttack)
                    {
                        ChangeState(AhuizotlState.TWOATTACK);
                    }
                    else
                    {
                        if (sqDistance2Player >= distanceIdleToRun)
                        {
                            ChangeState(AhuizotlState.RUN);
                            break;
                        }
                        else if (Random.Range(0f, 3f) > 1)
                        {
                            ChangeState(AhuizotlState.CHASE);
                        }
                        else
                        {
                            ChangeState(AhuizotlState.POWER);
                        }
                    }
                }
                break;
            case AhuizotlState.TWOATTACK:
                Debug.Log("DistancaTwoAttack: " + sqDistance2Player);
                bossController.SetTrigger("attack");
                ChangeState(AhuizotlState.IDLE);
                break;
            case AhuizotlState.CHASE:
                Debug.Log("DistancaChase: " + sqDistance2Player);
                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                timeChase-= Time.fixedDeltaTime;
                if (sqDistance2Player <= distanceChaseToAttack)
                {
                    ChangeState(AhuizotlState.TWOATTACK);
                }
                else if (timeChase <= 0)
                {
                    ChangeState(AhuizotlState.IDLE);
                }
                break;
            case AhuizotlState.POWER:
                
                Debug.Log("DistancaPower: " + sqDistance2Player);

                if (agent.velocity.sqrMagnitude <= 1 && seePlayerBool == false)
                {
                    seePlayerBool = true;
                    StartCoroutine("SeePlayer");
                }
                else if (timeSeePlayer > 0)
                {
                    timeSeePlayer -= Time.fixedDeltaTime;
                }
                else if (timeSeePlayer <= 0 && powerB == false)
                {
                    StopCoroutine("SeePlayer");
                    powerB = true;
                    bossController.SetTrigger("power");
                }
                else if (timeOffsetPower > 0)
                {
                    timeOffsetPower -= Time.fixedDeltaTime;
                }
                else if (tornadoB == false)
                {
                    Vector3 offsetPlayer = Random.onUnitSphere * 16f;
                    Vector3 spawnPostiion = playerTransform.position + offsetPlayer;
                    spawnPostiion.y = playerTransform.position.y;
                    Destroy(Instantiate(tornado, spawnPostiion, Quaternion.identity),timePower);
                    tornadoB = true;
                }
                else if (timePower >= 0)
                {
                    timePower -= Time.fixedDeltaTime;
                }
                else
                {
                    ChangeState(AhuizotlState.IDLE);
                }
                break;

            case AhuizotlState.RUN:
                Debug.Log("DistancaRun: " + sqDistance2Player);
                timeRun -= Time.fixedDeltaTime;
                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                if (timeRun <= 0 || sqDistance2Player <= distanceRunToAtack)
                {
                    ChangeState(AhuizotlState.ATTACK);
                }
                break;
            case AhuizotlState.ATTACK:
                Debug.Log("DistancaAttack: " + sqDistance2Player);
                if (attackBool == false)
                {
                    bossController.SetTrigger("attack");
                    attackBool = true;
                }

                timeAttack -= Time.fixedDeltaTime;
                if (timeAttack <= 0)
                {
                    ChangeState(AhuizotlState.IDLE);
                }
                break;
        }

        bossController.SetFloat("speed", agent.velocity.sqrMagnitude);
    }
}

public enum AhuizotlState
{
    NONE,
    IDLE,
    CHASE,
    RUN,
    ATTACK,
    TWOATTACK,
    POWER
}
