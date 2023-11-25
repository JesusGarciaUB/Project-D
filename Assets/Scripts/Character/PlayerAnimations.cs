using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private MoveOnClick player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<MoveOnClick>();
    }

    private void Update()
    {

        animator.SetBool("isAttacking", player.isAttacking());
        if (!player.attacking) animator.SetBool("isMoving", player.isMoving());
        else animator.SetBool("isMoving", false);
    }
}
