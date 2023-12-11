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
        if (canBeLearned && !learned && combatSystem.SkillTreePoints > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            combatSystem.AddSpeed(extraSpeed);
            canBeLearned = false;
            learned = true;

            combatSystem.SkillTreePoints--;
            Level_Manager._LEVELMANAGER.skillpointsText.text = combatSystem.SkillTreePoints.ToString();

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
