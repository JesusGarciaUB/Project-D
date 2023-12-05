using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_ability : MonoBehaviour
{
    protected CombatSystem combatSystem;
    public virtual void Performed() { }

    //Siempre llamar a esta funcion en el awake en todas las habilidades
    protected void SetUpAbility()
    {
        combatSystem = Level_Manager._LEVELMANAGER.player.GetComponent<CombatSystem>();
    }
}
