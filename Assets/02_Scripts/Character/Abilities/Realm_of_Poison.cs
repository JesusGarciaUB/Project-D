using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Realm_of_Poison : Base_ability
{
    //STATS
    private List<GameObject> enemies = new List<GameObject>();
    private float tickDamageCounter = 0f;
    [SerializeField] private int damage;
    [SerializeField] private float timeBetweenTicks;
    [SerializeField] private int cost;
    [SerializeField] private float timeBetweenCost;

    private void Awake()
    {
        SetUpAbility();
    }

    private void Update()
    {
        tickDamageCounter += Time.deltaTime;

        if (tickDamageCounter >= timeBetweenTicks )
        {
            tickDamageCounter = 0f;
            DealDamage();
        }

        if (combatSystem.WillDieNextFrame(cost))
        {
            gameObject.SetActive(false);
        }
    }

    //funcion para realizar daño a todos los enemigos de la lista si estan vivos
    private void DealDamage()
    {
        List<GameObject> to_remove = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            EnemyBehaviour e = enemy.GetComponent<EnemyBehaviour>();

            if (e.GetisAlive()) e.ReceiveDamage(damage);
            else to_remove.Add(enemy);
        }

        foreach (GameObject remove in to_remove)
        {
            enemies.Remove(remove);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyBehaviour>().GetisAlive()) enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Remove(collision.gameObject);
        }
    }
    private void LateUpdate()
    {
        transform.position = Level_Manager._LEVELMANAGER.player.transform.position;
    }

    private void OnEnable()
    {
        combatSystem.SubHealthRegen(cost);
    }

    private void OnDisable()
    {
        combatSystem.AddHealthRegen(cost);
    }

    public override void LevelUpAbility()
    {
        abilityLevel++;
        damage += 5;
        cost += 1;
    }
}
