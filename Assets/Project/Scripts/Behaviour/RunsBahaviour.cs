using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunsBahaviour : StateMachineBehaviour
{
    // Velocidad de rotación en grados por segundo.
    private float rotationSpeed = 170f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundsRun.Instance.NohekRunOn(animator.GetBool("rapido"));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Rotacion actual en Y del jugador
        float currentRotation = animator.gameObject.transform.eulerAngles.y;

        // Transform del jugador
        Transform playerTransform = animator.gameObject.transform;

        // Ajuste en la rotacion de la animacion cuando corre a la izquierda o a la derecha
        if (animator.GetFloat("direction") > 0)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
        } else if (animator.GetFloat("direction") < 0)
        {
            currentRotation -= rotationSpeed * Time.deltaTime;
        }

        // Se actualiza la rotacion del jugador
        playerTransform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundsRun.Instance.NohekRunOff();
    }

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
