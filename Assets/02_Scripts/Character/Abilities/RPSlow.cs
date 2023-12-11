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
    private float cooldownCounter = 0f;

    private void Start()
    {
        SetUpAbility();
    }

    private void Update()
    {
        PurgeList();
        transform.position = Level_Manager._LEVELMANAGER.player.transform.position;

        if (cooldownCounter > 0)
        {
            cooldownCounter -= Time.deltaTime;
            textcd.text = cooldownCounter.ToString("F2");
        }

        if (performing) durationCounter += Time.deltaTime;

        if (durationCounter >= duration && performing)
        {
            performing = false;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().GetisAlive()) enemy.GetComponent<NavMeshAgent>().speed += slow;
            }
        }

        if (cooldownCounter < 0)
        {
            cooldownCounter = 0;
            textcd.gameObject.SetActive(false);
            dark.SetActive(false);
        }

        if (!performing && performedThisFrame && cooldownCounter <= 0)
        {
            cooldownCounter = cooldown;
            durationCounter = 0f;
            performing = true;
            combatSystem.WasteMana(cost);
            dark.SetActive(true);
            textcd.gameObject.SetActive(true);
            textcd.text = cooldown.ToString();

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<EnemyBehaviour>().GetisAlive()) enemy.GetComponent<NavMeshAgent>().speed -= slow;
            }
        }

        performedThisFrame = false;
    }

    private void PurgeList()
    {
        List<int> toremove = new List<int>();
        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.GetComponent<EnemyBehaviour>().GetisAlive()) toremove.Add(count);
            count++;
        }

        for(int x = toremove.Count; x > 0; x--)
        {
            enemies.Remove(enemies[toremove[x - 1]]);
        }
    }
    public override void Performed()
    {
        performedThisFrame = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<EnemyBehaviour>().GetisAlive())
            {
                enemies.Add(other.gameObject);
                if (performing) other.GetComponent<NavMeshAgent>().speed -= slow;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.gameObject);
            if (performing) other.GetComponent<NavMeshAgent>().speed += slow;
        }
    }

    public override void LevelUpAbility()
    {
        abilityLevel++;
        duration += 0.2f;
    }
}
