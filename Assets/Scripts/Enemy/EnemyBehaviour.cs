using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    //COMPONENTS
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animator;
    [SerializeField] private Enemy_Healthbar_Manager healthbar_manager;

    //STATS
    [SerializeField] private float attackRange;
    [SerializeField] private float visionRange;
    [SerializeField] private int maxHealth;
    private int health;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private int damage;
    [SerializeField] private int GivenExperience;

    //AUXILIARES
    private bool isAlive = true;
    private bool isChasing = false;
    private bool canAttack = true;
    private bool isAttacking = false;

    private void Awake()
    {
        player = Level_Manager._LEVELMANAGER.player;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //seteamos vida y healthbar del enemigo
        health = maxHealth;
        healthbar_manager.SetMax(maxHealth);
        healthbar_manager.SetValue(health);
    }
    private void Update()
    {
        //si el esquelo ha muerto el anterior frame, no necesitamos nada del update
        if (isAlive)
        {
            //comprovamos que este vivo el jugador
            if (player.GetComponent<CombatSystem>().isAlive)
            {
                if (EnemyInVisionRange() && !isChasing) isChasing = true;

                if (EnemyInAttackRange() && canAttack) Attack();
                else if (isChasing && !EnemyInAttackRange() && !isAttacking) EnemySetChase();
                else
                {
                    agent.isStopped = true;
                    agent.ResetPath();
                    animator.SetBool("chasing", false);
                }
            }
            else
            {
                //bailamos al matar al jugador
                animator.SetTrigger("dance");
            }
        }
    }

    //ataque
    private void Attack()
    {
        isAttacking = true;
        canAttack = false;
        animator.SetTrigger("attack");
    }

    //hacemos daño al jugador en el frame que toca si esta a rango
    public void DealDamage()
    {
        if (EnemyInAttackRange()) player.GetComponent<CombatSystem>().ReceiveDamage(damage);
    }

    //controlamos cuando acaba nuestra animacion de ataque
    public void FinishedAttacking()
    {
        isAttacking = false;
        animator.SetTrigger("attack");
        StartCoroutine(WaitBetweenAttacks());
    }

    //si el enemigo esta a rango de ataque del jugador
    private bool EnemyInAttackRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= attackRange;
    }

    //Devuelve si el enemigo esta a rango de vision del jugador
    private bool EnemyInVisionRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) <= visionRange;
    }

    //configuramos el animator y el destino del enemigo
    private void EnemySetChase()
    {
        agent.SetDestination(player.transform.position);
        animator.SetBool("chasing", true);
    }

    //si nuestro raton esta encima de un enemigo y este esta vivo, el propio enemigo se configura como objetivo para el jugador
    public void MouseOverMob()
    {
        if (isAlive) player.GetComponent<CombatSystem>().target = this.gameObject;
        else player.GetComponent<CombatSystem>().target = null;
    }

    //funcion para recivir daño
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            player.GetComponent<ExpSystem>().GainExperience(GivenExperience);
            health = 0;
            animator.SetTrigger("die");
            isAlive = false;
            Destroy(agent);
            Destroy(GetComponent<CapsuleCollider>());
        }
        healthbar_manager.SetValue(health);
    }

    //auxiliar para saber cuando acaba nuestra animacion de morir
    public void FinishedDieAnimation()
    {
        Destroy(this.gameObject, 3f);
    }

    //controlamos la velocidad de ataque de nuestro esqueleto
    private IEnumerator WaitBetweenAttacks()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        canAttack = true;
    }
}
