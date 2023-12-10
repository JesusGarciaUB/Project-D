using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverSmall_Health : OnHoverBehaviour
{
    [SerializeField] private int extraHealth;
    private bool learned = false;

    new public void MouseClick()
    {
        if (canBeLearned && !learned)
        {
            combatSystem.AddMaxHealth(extraHealth);
            canBeLearned = false;
            learned = true;

            foreach (OnHoverBehaviour skill in nextSkill)
            {
                if (!skill.canBeLearned)
                {
                    skill.canBeLearned = true;
                    skill.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
}
