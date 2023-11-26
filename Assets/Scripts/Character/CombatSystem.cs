using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSystem : MonoBehaviour
{
    //TARGETING
    public GameObject target;
    public GameObject attackTarget;

    //STATS
    [SerializeField] private int health;
    [SerializeField] private float basicAttackRange;
    [SerializeField] private int basicAttackDamage;

    //AUXILIARES
    public bool isAlive = true;

    //para saber si al pulsar en un enemigo estamos a rango
    public bool OnRange()
    {
        return target != null && Vector3.Distance(target.transform.position, transform.position) <= basicAttackRange;
    }

    //hacemos da�o a nuestro objetivo
    public void DoBasicAttack()
    {
        attackTarget.GetComponent<EnemyBehaviour>().ReceiveDamage(basicAttackDamage);
    }

    //recivimos da�o
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        isAlive = false;
        StartCoroutine(ResetScene());
    }

    private IEnumerator ResetScene()
    {
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
