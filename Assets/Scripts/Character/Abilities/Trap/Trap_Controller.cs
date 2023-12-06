using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Controller : Base_ability
{
    //STATS
    [SerializeField] private GameObject trap;
    [SerializeField] private int cost;
    [SerializeField] private float cooldown;

    //TIMERS
    private float cooldownTimer;

    //AUXILIARES
    private bool performedThisFrame = false;

    private void Awake()
    {
        SetUpAbility();
        cooldownTimer = cooldown;
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (performedThisFrame && cooldownTimer >= cooldown)
        {
            combatSystem.WasteMana(cost);
            GameObject ins = Instantiate(trap);
            ins.transform.position = Level_Manager._LEVELMANAGER.GetPlayerPositionGround0();
            cooldownTimer = 0f;
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
        cooldown -= 0.2f;
    }
}
