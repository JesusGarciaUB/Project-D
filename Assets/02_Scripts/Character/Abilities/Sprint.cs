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
    private float cooldownTimer = 0f;

    //AUXILIARES
    private bool performing = false;
    private bool performedThisFrame = false;

    private void Awake()
    {
        SetUpAbility();
    }
    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            textcd.text = cooldownTimer.ToString("F2");
        }

        if (cooldownTimer < 0)
        {
            cooldownTimer = 0;
            dark.SetActive(false);
            textcd.gameObject.SetActive(false);
        }

        if (performing) durationTimer += Time.deltaTime;

        if (durationTimer >= duration && performing)
        {
            performing = false;
            Level_Manager._LEVELMANAGER.player.GetComponent<NavMeshAgent>().speed -= extraSpeed;
        }

        if (!performing && performedThisFrame && cooldownTimer <= 0)
        {
            textcd.text = cooldown.ToString();
            textcd.gameObject.SetActive(true);
            dark.SetActive(true);
            performing = true;
            Level_Manager._LEVELMANAGER.player.GetComponent<NavMeshAgent>().speed += extraSpeed;
            combatSystem.WasteMana(cost);
            cooldownTimer = cooldown;
            durationTimer = 0f;
        }
        performedThisFrame = false;
    }

    public override void Performed()
    {
        performedThisFrame = true;
    }

    public override void LevelUpAbility()
    {
        abilityLevel++;
        duration += 0.2f;
    }
}
