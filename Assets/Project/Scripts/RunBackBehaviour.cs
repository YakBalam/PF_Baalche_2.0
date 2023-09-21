using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrerAtrasBehaviour : StateMachineBehaviour
{
    private float currentRotationAngle;

    // Velocidad de rotación en grados por segundo.
    private float rotationSpeed = 280f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Rotacion en Y actual del jugador
        currentRotationAngle = animator.gameObject.transform.eulerAngles.y;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Nueva rotacion en Y del jugador
        currentRotationAngle += rotationSpeed * Time.deltaTime;

        // Accede al transform del Jugador
        Transform objectTransform = animator.gameObject.transform;

        // Aplica la rotación al Jugador
        objectTransform.rotation = Quaternion.Euler(0f, currentRotationAngle, 0f);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
