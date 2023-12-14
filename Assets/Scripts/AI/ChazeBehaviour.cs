using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChazeBehaviour : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private Transform _player;

    private float _attackRange = 2f;

    private float _chaseRange = 10f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponent<NavMeshAgent>();
        _agent.speed = 4;

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_player.position);
        float distance = Vector3.Distance(_player.position, animator.transform.position);
        
        if (distance < _attackRange)
            animator.SetBool("isAttacking", true);
        if (distance > _chaseRange)
            animator.SetBool("isChasing", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_agent.transform.position);
        _agent.speed = 2f;
    }

    
}
