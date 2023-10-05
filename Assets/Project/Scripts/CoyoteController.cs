using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class CoyoteController : MonoBehaviour
{
    public Vector2 destinationArea;
    public Rigidbody coyoteRigidBody;

    private Animator enemyController;
    private NavMeshAgent agent;
    private Transform playerTransform;
    private CoyoteState currentState;
    private Vector3 patrolDestination = Vector3.zero;
    private Vector3 escapePoint;
    
    private float waitToPatrol;
    private float timeIdle;
    private float timeEscape = 6f;
    private float timeRunAttack = 4f;
    private float timeAttack = 1.2f;

    private float sqDistance2Player;
    private float distancePatrol2Chase = 15f;
    private float distanceIdle2Chase = 8f;
    private float distance2RunAttack = 5f;
    private float distanceIdleToAttack = 2f;
    private float distanceRun2Attack = 2f;
    private float distanceEscape = 4.5f;

    private float speedIdle = 0f;
    private float speedPatrol = 3.5f;
    private float speedChase = 6f;
    private float speedRunAttack = 7f;

    private bool banderaEstado = false;

    void Start()
    {
        enemyController = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;

        currentState = CoyoteState.NONE;
        ChangeState(CoyoteState.PATROL);
        //ChangeState(CoyoteState.TEST);
    }

    void ChangeState(CoyoteState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case CoyoteState.PATROL:
                if (currentState == CoyoteState.CHASE)
                    StopCoroutine("UpdatePlayerDestination");
                currentState = newState;
                StartCoroutine("GeneratePatrolDestination");
                break;
            case CoyoteState.CHASE:
                if (currentState == CoyoteState.PATROL)
                    StopCoroutine("GeneratePatrolDestination");
                else if (currentState == CoyoteState.IDLEATTACK)
                    StopCoroutine("SeePlayer");
                currentState = newState;
                StartCoroutine("UpdatePlayerDestination");
                break;
            case CoyoteState.IDLEATTACK:
                if(currentState == CoyoteState.RUNATTACK)
                    StopCoroutine("UpdatePlayerDestination");
                currentState = newState;
                StartCoroutine("SeePlayer");
                break;
            case CoyoteState.RUNATTACK:
                if (currentState == CoyoteState.IDLEATTACK)
                {
                    StopCoroutine("SeePlayer");
                    StartCoroutine("UpdatePlayerDestination");
                }
                currentState = newState;
                break;
            case CoyoteState.ATTACK:
                if (currentState == CoyoteState.RUNATTACK)
                {
                    StopCoroutine("UpdatePlayerDestination");
                    StartCoroutine("SeePlayer");
                    StopCoroutine("SeePlayer");
                }
                else if (currentState == CoyoteState.IDLEATTACK)
                    StopCoroutine("SeePlayer");
                currentState = newState;
                break;
            case CoyoteState.ESCAPE:
                currentState = newState;
                break;
            case CoyoteState.DAMAGE:
                break;
            case CoyoteState.DEATH:
                break;
            case CoyoteState.TEST:
                StartCoroutine("SeePlayer");
                break;
        }
    }
     
    public IEnumerator GeneratePatrolDestination()
    {
        while (true)
        {
            patrolDestination = transform.position 
                                + new Vector3(Random.Range(-destinationArea.x, destinationArea.x),
                                              0,
                                              Random.Range(-destinationArea.y, destinationArea.y));
            agent.SetDestination(patrolDestination);
            waitToPatrol = Random.Range(1f, 3f);
            //waitToPatrol = 1f;
            yield return new WaitForSeconds(waitToPatrol);
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

    // Se genera un punto a una distanca de 15 unidades a partir del punto del jugador
    public Vector3 GenerateEscapePoint()
    {
        Vector3 escapeDireccion = Random.insideUnitSphere * distanceEscape;
        Vector3 puntoEscape = new Vector3(playerTransform.position.x + escapeDireccion.x, 
                                          transform.position.y, 
                                          playerTransform.position.z + escapeDireccion.z);

        return puntoEscape;        
    }


    void FixedUpdate()
    {
        switch (currentState)
        {
            case CoyoteState.PATROL:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("PATROL");
                }
                if (agent.speed != speedPatrol)
                {
                    agent.speed = speedPatrol;
                }
                if (enemyController.GetBool("correr") == true)
                {
                    enemyController.SetBool("correr", false);
                }
                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                if (sqDistance2Player <= distancePatrol2Chase * distancePatrol2Chase)
                {
                    banderaEstado = false;
                    ChangeState(CoyoteState.CHASE);
                }
                break;
            case CoyoteState.CHASE:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("CHASE");
                }
                if (agent.speed != speedChase)
                {
                    agent.speed = speedChase;
                }
                if (enemyController.GetBool("correr") == false)
                {
                    enemyController.SetBool("correr", true);
                }

                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;

                if (sqDistance2Player > distancePatrol2Chase * distancePatrol2Chase)
                {
                    banderaEstado = false;
                    ChangeState(CoyoteState.PATROL);
                }
                else if (sqDistance2Player <= distance2RunAttack * distance2RunAttack)
                {
                    banderaEstado = false;
                    ChangeState(CoyoteState.RUNATTACK);
                }
                break;
            case CoyoteState.IDLEATTACK:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("IDLE ATTACK");
                    agent.SetDestination(transform.position);
                }
                if (agent.speed != speedIdle)
                {
                    agent.speed = speedIdle;
                }
                timeIdle -= Time.fixedDeltaTime;

                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                if (sqDistance2Player > distanceIdle2Chase * distanceIdle2Chase)
                {
                    agent.speed = speedChase;
                    banderaEstado = false;
                    ChangeState(CoyoteState.CHASE);
                }
                else
                {
                    if (timeIdle <= 0)
                    {
                        if (sqDistance2Player <= distanceIdleToAttack * distanceIdleToAttack)
                        {
                            banderaEstado = false;
                            ChangeState(CoyoteState.ATTACK);
                        }
                        else
                        {
                            banderaEstado = false;
                            agent.speed = speedRunAttack;
                            ChangeState(CoyoteState.RUNATTACK);
                        }
                    }
                }
                break;
            case CoyoteState.RUNATTACK:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("RUN ATTACK");
                }
                if (agent.speed != speedRunAttack)
                {
                    agent.speed = speedRunAttack;
                }

                sqDistance2Player = (playerTransform.position - transform.position).sqrMagnitude;
                if (sqDistance2Player <= distanceRun2Attack * distanceRun2Attack)
                {
                    banderaEstado = false;
                    timeRunAttack = 4f;
                    ChangeState(CoyoteState.ATTACK);
                }
                timeRunAttack -= Time.fixedDeltaTime;
                if (timeRunAttack <= 0)
                {
                    banderaEstado = false;
                    agent.speed = speedIdle;
                    timeRunAttack = 4f;
                    ChangeState(CoyoteState.IDLEATTACK);
                }              
                break;
            case CoyoteState.ATTACK:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("ATTACK");
                }

                if (agent.speed == speedChase)
                    agent.SetDestination(transform.position + transform.forward * 2.5f);
                else
                    agent.SetDestination(transform.position);
                
                enemyController.SetTrigger("mordida");
                timeAttack -= Time.fixedDeltaTime;
                if (timeAttack <= 0)
                {
                    timeAttack = 1.2f;
                    if (Random.Range(2, 3) > 1f)
                    {
                        timeEscape = 4f;
                        banderaEstado = false;
                        escapePoint = GenerateEscapePoint();
                        ChangeState(CoyoteState.ESCAPE);
                    }
                    else
                    {
                        banderaEstado = false;
                        agent.speed = speedIdle;
                        timeIdle = 10f;
                        ChangeState(CoyoteState.IDLEATTACK);
                    }
                }
                
                break;
            case CoyoteState.ESCAPE:
                if (!banderaEstado)
                {
                    banderaEstado = true;
                    Debug.Log("ESCAPE: " + escapePoint);
                }
                if (agent.speed != speedChase)
                {
                    agent.speed = speedChase;
                }

                agent.SetDestination(escapePoint);
                timeEscape -= Time.fixedDeltaTime;

                if ((escapePoint.x == transform.position.x && escapePoint.z == transform.position.z) 
                    || timeEscape <= 0)
                {
                    banderaEstado = false;
                    timeIdle = 5f;
                    timeEscape = 4f;
                    ChangeState(CoyoteState.IDLEATTACK);
                    Debug.Log("pointCoyote: " + transform.position);
                }
                //Debug.Log("timeEscape: " + timeEscape);
                break;
            case CoyoteState.DAMAGE:
                break;
            case CoyoteState.DEATH:
                break;

            case CoyoteState.TEST:
                agent.speed = speedIdle;


                break;

        }
        
        enemyController.SetFloat("caminar", agent.velocity.sqrMagnitude);
    }
}

public enum CoyoteState
{
    NONE,
    PATROL,
    CHASE,
    IDLEATTACK,
    RUNATTACK,
    ATTACK,
    ESCAPE,
    
    DAMAGE,
    DEATH,

    TEST
}
