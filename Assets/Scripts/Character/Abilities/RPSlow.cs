using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RPSlow : Base_ability
{
    [SerializeField] private int slow;
    [SerializeField] private float cooldown;
    [SerializeField] private float duration;
    [SerializeField] private int cost;

    private List<GameObject> enemies = new List<GameObject>();
    private bool performedThisFrame = false;
    private bool performing = false;
    private float durationCounter = 0f;
    private float cooldownCounter;

    private void Start()
    {
        SetUpAbility();
        cooldownCounter = cooldown;
    }

    private void Update()
    {
        transform.position = Level_Manager._LEVELMANAGER.player.transform.position;

        cooldownCounter += Time.deltaTime;
        if (performing) durationCounter += Time.deltaTime;

        if (durationCounter >= duration && performing)
        {
            performing = false;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().GetisAlive()) enemy.GetComponent<NavMeshAgent>().speed += slow;
            }
        }

        if (!performing && performedThisFrame && cooldownCounter >= cooldown)
        {
            cooldownCounter = 0f;
            durationCounter = 0f;
            performing = true;
            combatSystem.WasteMana(cost);

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().GetisAlive()) enemy.GetComponent<NavMeshAgent>().speed -= slow;
            }
        }

        performedThisFrame = false;
    }
    public override void Performed()
    {
        performedThisFrame = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Add(other.gameObject);
            if (performing) other.GetComponent<NavMeshAgent>().speed -= slow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
            if (performing) other.GetComponent<NavMeshAgent>().speed += slow;
            if (!other.GetComponent<EnemyBehaviour>().GetisAlive()) enemies.Remove(other.gameObject);
        }
    }

    public override void LevelUpAbility()
    {
        abilityLevel++;
        duration += 0.2f;
    }
}
