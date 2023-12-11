using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OnHoverBehaviour : MonoBehaviour
{
    //ABILITY SETUP
    [SerializeField] private GameObject abilityAttached;
    [SerializeField] private int maxLevel;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private bool isActivatable;
    [SerializeField] private bool alwaysOn;
    [SerializeField] private Sprite icon;

    //AUXILIARES
    public bool canBeLearned;
    private int position;

    //ONHOVERTEXTBOX
    private GameObject textBox;
    [SerializeField] private string text;
    [SerializeField] private string pretext;
    [SerializeField] protected List<OnHoverBehaviour> nextSkill;

    protected CombatSystem combatSystem;
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
        textBox.GetComponent<TextMeshProUGUI>().text = pretext + text;
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
            Level_Manager._LEVELMANAGER.skillpointsText.text = combatSystem.SkillTreePoints.ToString();
            if (currentLevel == 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                foreach (OnHoverBehaviour skill in nextSkill)
                {
                    if (!skill.canBeLearned)
                    {
                        skill.canBeLearned = true;
                        skill.GetComponent<Image>().color = Color.white;
                    }
                }
                position = player.GetNextFreeSlot();
                player.UseNextFreeSlot(position, abilityAttached, isActivatable, alwaysOn, icon);
            } else
            {
                player.LevelUpAbility(position);
            }
            currentLevel++;
            pretext = "(" + currentLevel + "/" + maxLevel + ") ";
            textBox.GetComponent<TextMeshProUGUI>().text = pretext + text;
        }
    }
}
