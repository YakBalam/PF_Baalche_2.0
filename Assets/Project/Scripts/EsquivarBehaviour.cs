using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsquivarBehaviour : StateMachineBehaviour
{
    // Velocidad de rotación en grados por segundo.
    private float rotationSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rotationSpeed = 180f;

        // if para controlar si esquiva para atras, para la izquierda o para la derecha
        if (animator.GetFloat("direction") < 0)
        {
            animator.SetFloat("directionEsquivar", -1f);
        }
        else if (animator.GetFloat("direction") > 0)
        {
            animator.SetFloat("directionEsquivar", 1f);
        }
        else
        {
            animator.SetFloat("directionEsquivar", 0f);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Transform actual del jugador
        Transform playerTransform = animator.gameObject.transform;

        // Rotacion en Y actual del jugador
        float currentRotation = animator.gameObject.transform.eulerAngles.y;

        // Ajuste en la rotacion de la animacion cuando esquiva a la izquierda o a la derecha
        if (animator.GetFloat("directionEsquivar") < 0)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
        }
        else if (animator.GetFloat("directionEsquivar") > 0)
        {
            currentRotation -= rotationSpeed * Time.deltaTime;
        }

        // Se actualiza la rotacion
        playerTransform.rotation = Quaternion.Euler(0f, currentRotation, 0f);

        // Con el tiempo se disminuye la rotacion dada
        if (rotationSpeed > 0f)
        {
            rotationSpeed -= 2f;
        }



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
