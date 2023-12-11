using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverMoreArea : OnHoverBehaviour
{
    private bool learned = false;
    private Vector3 addToParticle = new Vector3(0.2f, 0.2f, 0.2f);

    new public void MouseClick()
    {
        if (canBeLearned && !learned && combatSystem.SkillTreePoints > 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            canBeLearned = false;
            learned = true;

            combatSystem.SkillTreePoints--;
            Level_Manager._LEVELMANAGER.skillpointsText.text = combatSystem.SkillTreePoints.ToString();

            GameObject main = Level_Manager._LEVELMANAGER.main_ability;
            main.transform.localScale = new Vector3(main.transform.localScale.x + 1, main.transform.localScale.y, main.transform.localScale.z + 1);

            ParticleSystem.ShapeModule module = main.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().shape;
            module.scale += addToParticle;

            module = main.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().shape;
            module.scale += addToParticle;

            if (main.activeSelf)
            {
                main.SetActive(false);
                main.SetActive(true);
            }

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
