using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhanced_Healing : Base_ability
{
    //STATS
    [SerializeField] private int extraHealingRegen;
    [SerializeField] private float timeBetweenHealing;

    //TIMERS
    private float tickCounter = 0f;
    private void Awake()
    {
        SetUpAbility();
    }

    private void Update()
    {
        tickCounter += Time.deltaTime;
        if (tickCounter >= timeBetweenHealing)
        {
            combatSystem.HealDamage(extraHealingRegen);
            tickCounter = 0f;
        }
    }
}
