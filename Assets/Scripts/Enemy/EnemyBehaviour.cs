using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float range;
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animator;

    private void Awake()
    {
        player = Level_Manager._LEVELMANAGER.player;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!EnemyInRange()) EnemySetChase();
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            animator.SetBool("chasing", false);
        }
    }

    private bool EnemyInRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= range;
    }

    private void EnemySetChase()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("chasing", true);
    }

    public void MouseOverMob()
    {
        
    }
}
