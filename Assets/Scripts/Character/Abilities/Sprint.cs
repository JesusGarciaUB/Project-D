using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sprint : Base_ability
{
    //STATS
    [SerializeField] private int cost;
    [SerializeField] private float duration;
    [SerializeField] private int extraSpeed;
    [SerializeField] private float cooldown;

    //TIMERS
    private float durationTimer = 0f;
    private float cooldownTimer;

    //AUXILIARES
    private bool performing = false;
    private bool performedThisFrame = false;

    private void Awake()
    {
        SetUpAbility();
        cooldownTimer = cooldown;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (performing) durationTimer += Time.deltaTime;

        if (durationTimer >= duration && performing)
        {
            performing = false;
            Level_Manager._LEVELMANAGER.player.GetComponent<NavMeshAgent>().speed -= extraSpeed;
        }

        if (!performing && performedThisFrame && cooldownTimer >= cooldown)
        {
            performing = true;
            Level_Manager._LEVELMANAGER.player.GetComponent<NavMeshAgent>().speed += extraSpeed;
            combatSystem.WasteMana(cost);
            cooldownTimer = 0f;
            durationTimer = 0f;
        }
        performedThisFrame = false;
    }

    public override void Performed()
    {
        performedThisFrame = true;
    }
}
