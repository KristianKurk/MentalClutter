using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNextReadyPhase : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.instance.DisplayNextReadyPhase();
        Destroy(animator.gameObject);
    }
}
