using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CombatSystem : MonoBehaviour
{
    //TARGETING
    public GameObject target;
    public GameObject attackTarget;

    //STATS
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private int maxMana;
    private int mana;
    [SerializeField] private float basicAttackRange;
    [SerializeField] private int basicAttackDamage;
    private int SkillTreePoints = 0;

    //UI HEALTH AND MANA
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private Slider manaSlider;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI maxManaText;

    //AUXILIARES
    public bool isAlive = true;

    private void Awake()
    {
        SetUpHealthAndMana();
    }

    //setup inicial y para cuando subimos de nivel
    private void SetUpHealthAndMana()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        healthText.text = health.ToString();
        maxHealthText.text = maxHealth.ToString();

        mana = maxMana;
        manaSlider.maxValue = maxMana;
        manaSlider.value = mana;
        manaText.text = mana.ToString();
        maxManaText.text = maxMana.ToString();
    }

    //para saber si al pulsar en un enemigo estamos a rango
    public bool OnRange()
    {
        return target != null && Vector3.Distance(target.transform.position, transform.position) <= basicAttackRange;
    }

    //hacemos daño a nuestro objetivo
    public void DoBasicAttack()
    {
        attackTarget.GetComponent<EnemyBehaviour>().ReceiveDamage(basicAttackDamage);
    }

    //recivimos daño
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
        healthText.text = health.ToString();
        healthSlider.value = health;
    }
    
    //funcion llamada el mismo frame al morir
    private void Die()
    {
        isAlive = false;
        health = 0;
        StartCoroutine(ResetScene());   //TESTING ONLY
    }

    //llamamos a esta funcion al subir de nivel para mejorar nuestros stats
    public void LevelUp()
    {
        maxHealth = (int)(maxHealth * 1.2f);
        maxMana = (int)(maxMana * 1.2f);
        SkillTreePoints++;
        SetUpHealthAndMana();
    }

    //TESTING ONLY
    private IEnumerator ResetScene()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
