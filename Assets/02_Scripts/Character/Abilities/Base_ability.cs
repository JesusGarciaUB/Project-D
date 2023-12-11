using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Base_ability : MonoBehaviour
{
    protected CombatSystem combatSystem;
    protected int abilityLevel = 1;
    private Sprite icon;
    protected TextMeshProUGUI textcd;
    private GameObject iconHandler;
    protected GameObject dark;
    public virtual void Performed() { }
    public virtual void LevelUpAbility() { }

    //Siempre llamar a esta funcion en el awake en todas las habilidades
    protected void SetUpAbility()
    {
        combatSystem = Level_Manager._LEVELMANAGER.player.GetComponent<CombatSystem>();
    }

    public void SetUI(Sprite _icon, TextMeshProUGUI _textcd, GameObject _iconHandler, GameObject _dark)
    {
        icon = _icon;
        textcd = _textcd;
        iconHandler = _iconHandler;
        dark = _dark;

        iconHandler.GetComponent<Image>().sprite = icon;
        iconHandler.SetActive(true);
    }
}
