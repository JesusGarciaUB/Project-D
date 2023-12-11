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
    private float cooldownTimer = 0f;

    //AUXILIARES
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

        if (performedThisFrame && cooldownTimer <= 0)
        {
            textcd.text = cooldown.ToString();
            textcd.gameObject.SetActive(true);
            dark.SetActive(true);
            combatSystem.WasteMana(cost);
            GameObject ins = Instantiate(trap);
            ins.transform.position = Level_Manager._LEVELMANAGER.GetPlayerPositionGround0();
            cooldownTimer = cooldown;
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
