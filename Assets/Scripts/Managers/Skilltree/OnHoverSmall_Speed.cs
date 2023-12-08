using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverSmall_Speed : OnHoverBehaviour
{
    [SerializeField] private float extraSpeed;
    private bool learned = false;

    new public void MouseClick()
    {
        if (canBeLearned && !learned)
        {
            combatSystem.AddSpeed(extraSpeed);
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
