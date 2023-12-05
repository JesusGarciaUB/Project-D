using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enhanced_Healing : Base_ability
{
    //STATS
    [SerializeField] private int extraHealingRegen;
    private void Awake()
    {
        SetUpAbility();
        combatSystem.AddHealthRegen(extraHealingRegen);
    }
}
