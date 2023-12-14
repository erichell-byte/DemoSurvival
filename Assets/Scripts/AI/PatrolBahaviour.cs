using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBahaviour : StateMachineBehaviour
{
    private float timer;
    private List<Transform> points = new List<Transform>();
    private NavMeshAgent agent;

    private Transform _player;
    private float _chaseRange = 10f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObject = GameObject.FindGameObjectWithTag("Points").transform;
        foreach (Transform t in pointsObject)
        {
            points.Add(t);
        }
        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(points[Random.Range(0, points.Count)].position);
        timer += Time.deltaTime;
        if (timer > 10)
            animator.SetBool("isPatroling",false);

        float distance = Vector3.Distance(animator.transform.position, _player.position);
        if (distance < _chaseRange)
            animator.SetBool("isChasing", true);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }
}
