using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    //COMPONENTES
    private Animator animator;
    private MoveOnClick player;
    private CombatSystem cs;

    //AUXILIARES
    private bool triggeredDeath = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<MoveOnClick>();
        cs = GetComponent<CombatSystem>();
    }

    private void Update()
    {
        animator.SetBool("isAttacking", player.isAttacking());
        animator.SetBool("isMoving", player.isMoving());
        if (!cs.isAlive && !triggeredDeath)
        {
            animator.SetTrigger("die");
            triggeredDeath = true;
        }
    }
}
