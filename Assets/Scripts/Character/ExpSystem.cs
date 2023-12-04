using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpSystem : MonoBehaviour
{
    //STATS
    private int currentLevel = 1;
    [SerializeField] private int maxLevel;
    private int currentExp = 0;
    private int ExpToLvlUp = 100;

    //COMPONENTES
    private CombatSystem combatSystem;

    //UI LEVEL
    [SerializeField] private Slider ExpbarSlider;
    [SerializeField] private TextMeshProUGUI Exptext;
    [SerializeField] private TextMeshProUGUI MaxExptext;
    [SerializeField] private TextMeshProUGUI LevelText;

    //AUXILIARES
    private bool canGainExp = true;

    private void Awake()
    {
        combatSystem = GetComponent<CombatSystem>();
        ExpbarSlider.value = currentExp;
        ExpbarSlider.maxValue = ExpToLvlUp;
        Exptext.text = currentExp.ToString();
        MaxExptext.text = ExpToLvlUp.ToString();
        LevelText.text = currentLevel.ToString();
    }

    //funcion para subir de nivel, el excedente de experiencia pasa al siguiente. Actualmente la equacion es x *= 1.6
    private void LevelUp(int lastExpGained)
    {
        currentExp = lastExpGained - (ExpToLvlUp - currentExp);
        currentLevel++;
        ExpToLvlUp = (int)((float)ExpToLvlUp * 1.6f);
        combatSystem.LevelUp();

        if (currentLevel == maxLevel)
        {
            canGainExp = false;
            ExpToLvlUp = 0;
            currentExp = 0;
        }

        ExpbarSlider.maxValue = ExpToLvlUp;
        MaxExptext.text = ExpToLvlUp.ToString();
        LevelText.text = currentLevel.ToString();
    }

    //funcion para ganar experiencia
    public void GainExperience(int exp)
    {
        if (canGainExp)
        {
            if (currentExp + exp >= ExpToLvlUp) LevelUp(exp);
            else currentExp += exp;
        }

        ExpbarSlider.value = currentExp;
        Exptext.text = currentExp.ToString();
    }
}
