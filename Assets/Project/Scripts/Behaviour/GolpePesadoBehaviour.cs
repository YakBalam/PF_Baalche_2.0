using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolpePesadoBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Activacion del collider del Machete
        animator.GetComponent<ThirdPersonController>().EnableColliderMachete();

        // Reproduccion de sonido (del machete) al atacar
        if (animator.GetBool("rapido") == false)
        {
            SoundFxManager.Instance.NohekAttackPesado();
        }
        else
        {
            SoundFxManager.Instance.NohekAttackPesadoFast();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Desactivacion del collider del Machete
        animator.GetComponent<ThirdPersonController>().DisableColliderMachete();
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
