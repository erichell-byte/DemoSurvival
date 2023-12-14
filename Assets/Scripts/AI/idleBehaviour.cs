using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  idleBehaviour : StateMachineBehaviour
{
    private float timer;

    private Transform _player;
    private float _chaseRange = 10f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5)
            animator.SetBool("isPatroling",true);
        float distance = Vector3.Distance(animator.transform.position, _player.position);
        if (distance < _chaseRange)
            animator.SetBool("isChasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
