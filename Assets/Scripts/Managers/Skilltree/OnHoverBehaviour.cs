using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnHoverBehaviour : MonoBehaviour
{
    //ABILITY SETUP
    [SerializeField] private GameObject abilityAttached;
    [SerializeField] private int maxLevel;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private bool isActivatable;
    [SerializeField] private bool alwaysOn;

    //AUXILIARES
    public bool canBeLearned;
    private int position;

    //ONHOVERTEXTBOX
    private GameObject textBox;
    [SerializeField] private string text;
    [SerializeField] private List<OnHoverBehaviour> nextSkill;

    private CombatSystem combatSystem;
    private MoveOnClick player;

    private void Awake()
    {
        combatSystem = Level_Manager._LEVELMANAGER.player.GetComponent<CombatSystem>();
        textBox = Level_Manager._LEVELMANAGER.onHoverObject;
        player = Level_Manager._LEVELMANAGER.player.GetComponent<MoveOnClick>();
    }
    public void MouseEnter()
    {
        textBox.SetActive(true);
        textBox.GetComponent<HoverObject>().SetPosition(transform.position + new Vector3(0, 50 + (float)(textBox.GetComponent<RectTransform>().rect.height * 0.5)));
        textBox.GetComponent<TextMeshProUGUI>().text = text;
    }

    public void MouseExit()
    {
        textBox.SetActive(false);
    }

    public void MouseClick()
    {
        if (canBeLearned && combatSystem.SkillTreePoints > 0 && currentLevel < maxLevel)
        {
            combatSystem.SkillTreePoints--;
            if (currentLevel == 0)
            {
                foreach (OnHoverBehaviour skill in nextSkill)
                {
                    skill.canBeLearned = true;
                }
                Debug.Log("Fist iteration");
                position = player.GetNextFreeSlot();
                player.UseNextFreeSlot(position, abilityAttached, isActivatable, alwaysOn);
            } else
            {
                Debug.Log("LevelUp");
                player.LevelUpAbility(position);
            }
            currentLevel++;
        }
    }
}
