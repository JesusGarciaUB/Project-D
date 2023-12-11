using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.AI;

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
    public int SkillTreePoints;
    [SerializeField] private float timeBetweenHealthRegen;
    [SerializeField] private int healthRegenTick;
    [SerializeField] private float timeBetweenManaRegen;
    [SerializeField] private int manaRegenTick;

    //UI HEALTH AND MANA
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private Slider manaSlider;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI maxManaText;

    //AUXILIARES
    public bool isAlive = true;
    private float healthRegenCounter = 0f;
    private float manaRegenCounter = 0f;
    private int toAddThisFrame = 0;

    private void Awake()
    {
        SetUpHealthAndMana();
    }

    private void Update()
    {
        healthRegenCounter += Time.deltaTime;
        manaRegenCounter += Time.deltaTime;

        if (timeBetweenHealthRegen <= healthRegenCounter)
        {
            healthRegenCounter = 0f;
            HealDamage(healthRegenTick);
        }

        if (timeBetweenManaRegen <= manaRegenCounter)
        {
            manaRegenCounter = 0f;
            ReceiveMana(manaRegenTick);
        }

        NormalizeHealth();
    }

    //Modificamos la vida 1 vez por frame
    private void NormalizeHealth()
    {
        health += toAddThisFrame;
        if (health >= maxHealth) health = maxHealth;
        if (health <= 0) Die();

        healthText.text = health.ToString();
        healthSlider.value = health;

        toAddThisFrame = 0;
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
        toAddThisFrame -= damage;
        Camera.main.GetComponent<CameraShake>().timeShake = 0.2f;
    }

    //nos curamos
    public void HealDamage(int damage)
    {
        if (isAlive)
        {
            toAddThisFrame += damage;
        }
    }

    //gastamos mana
    public void WasteMana(int cost)
    {
        mana -= cost;
        manaText.text = mana.ToString();
        manaSlider.value = mana;
    }

    //recuperamos mana
    public void ReceiveMana(int cost)
    {
        if (isAlive)
        {
            mana += cost;
            if (mana > maxMana) mana = maxMana;
            manaText.text = mana.ToString();
            manaSlider.value = mana;
        }
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
        Level_Manager._LEVELMANAGER.skillpointsText.text = SkillTreePoints.ToString();
    }

    //TESTING ONLY
    private IEnumerator ResetScene()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool WillDieNextFrame(int damage)
    {
        return health - damage <= 0;
    }

    public void AddHealthRegen(int val)
    {
        healthRegenTick += val;
    }

    public void SubHealthRegen(int val)
    {
        healthRegenTick -= val;
    }

    public void AddMaxHealth(int val)
    {
        maxHealth += val;
        maxHealthText.text = maxHealth.ToString();
    }

    public void AddSpeed(float val)
    {
        GetComponent<NavMeshAgent>().speed += val;
    }
}
